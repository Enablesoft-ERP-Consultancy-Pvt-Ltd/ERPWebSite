using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class UserRigets : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDlCompanyName, "Select CompanyId,CompanyName from Companyinfo where MasterCompanyId=" + Session["varCompanyId"] + "", true, "----Select Company----");
            if (DDlCompanyName.Items.Count > 0)
            {
                DDlCompanyName.SelectedIndex = 1;
                CompanySelectedChange();
            }
            UtilityModule.ConditonalChkBoxListFill(ref ChLProcess, "Select Process_Name_id,Process_Name from Process_Name_Master where MasterCompanyId=" + Session["varCompanyId"] + " order by Process_Name");
            UtilityModule.NewChkBoxListFill(ref ChlCategory, "select Category_ID,Category_Name From ITEM_CATEGORY_Master where MasterCompanyId=" + Session["varCompanyId"] + "  Order by  Category_Name");
            UtilityModule.NewChkBoxListFill(ref CHKCompany, "select CompanyId,CompanyName From CompanyInfo Where MasterCompanyId=" + Session["varCompanyId"] + "  Order by  CompanyName");
            UtilityModule.NewChkBoxListFill(ref ChkGodown, "select GodownId,GodownName from GodownMaster Where MasterCompanyId=" + Session["varCompanyId"] + "  Order by  GodownName");
            UtilityModule.NewChkBoxListFill(ref ChkForBranch, "Select ID, BranchName From BRANCHMASTER(Nolock) Where MasterCompanyID = " + Session["varCompanyId"] + " Order By BranchName ");
            UtilityModule.NewChkBoxListFill(ref ChkForCustomerCode, "Select CustomerID,CONCAT(CustomerCode,'(',customername,')') as CustomerCode  From CustomerInfo(Nolock) Where MasterCompanyID = " + Session["varCompanyId"] + " and isnull(CustomerCode,'')<>'' Order By CustomerCode");

            switch (Session["varcompanyNo"].ToString())
            {
                case "8"://ANISA
                case "14":
                    tdunits.Visible = true;
                    UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    break;
                case "7":
                    TRChLProcess.Visible = true;
                    break;
                case "9":
                    tdunits.Visible = true;
                    break;
                case "38":
                    tdunits.Visible = true;
                    UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    break;
                case "39":
                    tdunits.Visible = true;
                    UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    break;
                case "40":
                    tdunits.Visible = true;
                    UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    break;
                case "41":
                    tdunits.Visible = true;
                    UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    break;
                case "42":
                    tdunits.Visible = true;
                    UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    break;
                case "29":
                    tdunits.Visible = true;
                    UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    break;
                case "20":
                    tdunits.Visible = true;
                    UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    break;
                case "37":
                    tdunits.Visible = true;
                    UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    break;
                default:
                    if (variable.VarFinishingNewModuleWise == "1")
                    {
                        tdunits.Visible = true;
                        UtilityModule.NewChkBoxListFill(ref chkunits, "select  UnitsId,UnitName from Units Where MasterCompanyId=" + Session["varCompanyId"] + "");
                    }
                    break;
            }
            UserNameSlectedChange();
            DDUserName.Focus();

        }
    }
    protected void DDlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    private void CompanySelectedChange()
    {
        string str = "select UserId,UserName+' -- '+LoginName from NewUserDetail where CompanyId=" + Session["varCompanyId"] + "";
        if (Session["varCompanyId"].ToString() == "21")
        {
            str = str + " and USERType not in(1,6)";
        }
        else
        {
            str = str + " and USERType not in(1)";
        }
        str = str + " order by username";
        UtilityModule.ConditionalComboFill(ref DDUserName, str, true, "----Select UserLoginName----");
                
    }
    protected void DDUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UserNameSlectedChange();
        CheckedCheckBoxChlCategory();
        CheckedCheckBoxChlProcess();
        CheckedCheckBoxChkCompany();
        CheckedCheckBoxChlUnits();
        CheckedCheckBoxChkGodown();
        CheckedCheckBoxChkForBranch();
        CheckedCheckBoxChkForCustomer();

        filltree();
    }
    protected void CheckedCheckBoxChkForCustomer()
    {
        for (int c = 0; c < ChkForCustomerCode.Items.Count; c++)
        {
            ChkForCustomerCode.Items[c].Selected = false;
        }
        string str = "Select Distinct CustomerID From CustomerUser(Nolock) Where UserId=" + DDUserName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int i = 0; i < ChkForCustomerCode.Items.Count; i++)
                {
                    if (ChkForCustomerCode.Items[i].Value == ds.Tables[0].Rows[j]["CustomerID"].ToString())
                    {
                        ChkForCustomerCode.Items[i].Selected = true;
                    }
                }
            }
        }
    }
    protected void CheckedCheckBoxChkForBranch()
    {
        for (int c = 0; c < ChkForBranch.Items.Count; c++)
        {
            ChkForBranch.Items[c].Selected = false;
        }
        string str = "Select Distinct BranchID From BranchUser(Nolock) where UserId=" + DDUserName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int i = 0; i < ChkForBranch.Items.Count; i++)
                {
                    if (ChkForBranch.Items[i].Value == ds.Tables[0].Rows[j]["BranchID"].ToString())
                    {
                        ChkForBranch.Items[i].Selected = true;
                    }
                }
            }
        }
    }

    private void UserNameSlectedChange()
    {
        DivMenu.Visible = true;
        
        string Str = "select MenuId,DisplayName from FormName where ParentId is null";
        if (Session["varcompanyNo"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
        {
            Str = Str + " And MenuID <> 191";
        }
        if (Session["varcompanyNo"].ToString() == "44" )
        {
            Str = Str + " And MenuID not in (48,49) ";
        }
        ConditionalTreeViewWithChkbox(ref TVMenues, Str);

        for (int i = 0; i < ChLProcess.Items.Count; i++)
        {
            ChLProcess.Items[i].Selected = false;
            ChkForAllProcess.Checked = false;
        }
        for (int i = 0; i < ChlCategory.Items.Count; i++)
        {
            ChlCategory.Items[i].Selected = false;
            ChkForAllCategory.Checked = false;
        }
        for (int i = 0; i < CHKCompany.Items.Count; i++)
        {
            CHKCompany.Items[i].Selected = false;
            ChkForAllCompany.Checked = false;
        }
    }
    protected void CheckedCheckBoxChlCategory()
    {
        for (int c = 0; c < ChlCategory.Items.Count; c++)
        {
            ChlCategory.Items[c].Selected = false;
        }
        string str = "Select Distinct CategoryId From UserRights_Category  where UserId=" + DDUserName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int i = 0; i < ChlCategory.Items.Count; i++)
                {
                    if (ChlCategory.Items[i].Value == ds.Tables[0].Rows[j]["CategoryId"].ToString())
                    {
                        ChlCategory.Items[i].Selected = true;
                    }
                }
            }
        }
        else
        {
            for (int c = 0; c < ChlCategory.Items.Count; c++)
            {
                ChlCategory.Items[c].Selected = false;
            }
        }
    }
    protected void CheckedCheckBoxChkCompany()
    {
        string str = "select Distinct CompanyId from [Company_Authentication] Where UserId=" + DDUserName.SelectedValue + " And  MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int i = 0; i < CHKCompany.Items.Count; i++)
                {
                    if (CHKCompany.Items[i].Value == ds.Tables[0].Rows[j]["CompanyId"].ToString())
                    {
                        CHKCompany.Items[i].Selected = true;
                    }
                }
            }
        }
    }
    protected void CheckedCheckBoxChlProcess()
    {
        for (int c = 0; c < ChLProcess.Items.Count; c++)
        {
            ChLProcess.Items[c].Selected = false;
        }
        string str = "Select Distinct processid From UserRightsProcess  where UserId=" + DDUserName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int i = 0; i < ChLProcess.Items.Count; i++)
                {
                    if (ChLProcess.Items[i].Value == ds.Tables[0].Rows[j]["processid"].ToString())
                    {
                        ChLProcess.Items[i].Selected = true;
                    }
                }
            }
        }
        else
        {
            for (int c = 0; c < ChLProcess.Items.Count; c++)
            {
                ChLProcess.Items[c].Selected = false;
            }
        }
    }
    protected void CheckedCheckBoxChlUnits()
    {
        for (int c = 0; c < chkunits.Items.Count; c++)
        {
            chkunits.Items[c].Selected = false;
        }
        string str = "Select Distinct UnitsId From Units_Authentication  where UserId=" + DDUserName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int i = 0; i < chkunits.Items.Count; i++)
                {
                    if (chkunits.Items[i].Value == ds.Tables[0].Rows[j]["UnitsId"].ToString())
                    {
                        chkunits.Items[i].Selected = true;
                    }
                }
            }
        }
        else
        {
            for (int c = 0; c < chkunits.Items.Count; c++)
            {
                chkunits.Items[c].Selected = false;
            }
        }
    }
    protected void CheckedCheckBoxChkGodown()
    {
        for (int c = 0; c < ChkGodown.Items.Count; c++)
        {
            ChkGodown.Items[c].Selected = false;
        }
        string str = "Select Distinct GodownId From Godown_Authentication  where UserId=" + DDUserName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int i = 0; i < ChkGodown.Items.Count; i++)
                {
                    if (ChkGodown.Items[i].Value == ds.Tables[0].Rows[j]["GodownID"].ToString())
                    {
                        ChkGodown.Items[i].Selected = true;
                    }
                }
            }
        }
        else
        {
            for (int c = 0; c < ChkGodown.Items.Count; c++)
            {
                ChkGodown.Items[c].Selected = false;
            }
        }
    }
    private void filltree()
    {
        DivMenu.Visible = true;
        string str=string.Empty;
        if (Session["varcompanyNo"].ToString() == "44" )
        {
            str = "Select Distinct MenuId From UserRights  where menuid not in (48,49) and userid=" + DDUserName.SelectedValue;
        }
        else {
            str = "Select Distinct MenuId From UserRights  where userid=" + DDUserName.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            foreach (TreeNode nd in TVMenues.Nodes)
            {
                str = @"Select FN.* 
                    From FormName FN
                    JOIN UserRights UR(Nolock) ON UR.MenuId = FN.MenuID And UR.UserID = " + DDUserName.SelectedValue + @" 
                    Where DisplayName='" + nd.Text + "'";
               
                    

                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str );
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    nd.Checked = true;
                }
                checknodes(nd, ds.Tables[0].Rows[i]["MenuId"].ToString());
            }
        }
    }
    void checknodes(TreeNode tnode, string myvalue)
    {
        tnode.ChildNodes.Cast<TreeNode>().Where(node => node.Value == myvalue).ToList().ForEach((node => node.Checked = true));
        foreach (TreeNode nd in tnode.ChildNodes)
        {
            checknodes(nd, myvalue);
        }
    }
    protected void ConditionalTreeViewWithChkbox(ref TreeView TreeViewName, string strsql)
    {
        try
        {
            TreeViewName.Nodes.Clear();
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            string[,] ParentNode = new string[100, 2];
            int count = 0;
            int n = ds.Tables[0].Rows.Count;
            while (n > 0)
            {
                ParentNode[count, 0] = ds.Tables[0].Rows[count][0].ToString();
                ParentNode[count, 1] = ds.Tables[0].Rows[count][1].ToString();
                n--;
                count++;
            }
            for (int loop = 0; loop < count; loop++)
            {
                TreeNode root = new TreeNode();
                root.Text = ParentNode[loop, 1];
                root.Value = ParentNode[loop, 0];
                string Sqlst = "select MenuId,DisplayName from FormName where ParentId=" + ParentNode[loop, 0];
                if (Session["varCompanyId"].ToString() == "21")
                {
                    if (Session["usertype"].ToString() == "6")
                    {
                        Sqlst = Sqlst + " And MenuID in(37,38,90,136,162)";
                        //Sqlst = Sqlst + " And MenuID = 92";
                    }
                   
                    else
                    {
                        Sqlst = Sqlst + " And MenuID not in(37,38,90,136,162)";
                        //Sqlst = Sqlst + " And MenuID <> 92";
                    }
                }
                else if (Session["varCompanyId"].ToString() == "44")
                {
                    //Sqlst = Sqlst + " And MenuID not in(48,49)";
                    //Sqlst = Sqlst + " And MenuID = 92";
                }

                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Sqlst);
                int i = ds1.Tables[0].Rows.Count;
                int a = 0;
                while (i > 0)
                {
                    
                    TreeNode child = new TreeNode();
                    //if (Session["varCompanyId"].ToString()!="21" && ds1.Tables[0].Rows[a][1].ToString() != "PACKING")
                    //{
                    child.Text = ds1.Tables[0].Rows[a][1].ToString();
                    child.Value = ds1.Tables[0].Rows[a][0].ToString();
                    //}
                    string Sqlst1 = "select MenuId,DisplayName from FormName where ParentId=" + ds1.Tables[0].Rows[a][0].ToString();

                    if (Session["varCompanyId"].ToString() == "21")
                    {
                        if (Session["usertype"].ToString() == "6")
                        {                           

                            Sqlst1 = Sqlst1 + " And MenuID in(93,94,95,113,115,117,118,121,204,205,206)";                            
                        }
                        else
                        {                            
                            Sqlst1 = Sqlst1 + " And MenuID not in(93,113,115,117,118,121,204,205,206)";
                        }
                    }

                    DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Sqlst1);
                    int j = ds2.Tables[0].Rows.Count;
                    int k = 0;
                    while (j > 0)
                    {
                        TreeNode GrandSon = new TreeNode();
                        GrandSon.Text = ds2.Tables[0].Rows[k][1].ToString();
                        GrandSon.Value = ds2.Tables[0].Rows[k][0].ToString();
                        string Sqlst2 = "select MenuId,DisplayName from FormName where ParentId=" + ds2.Tables[0].Rows[k][0].ToString();
                        DataSet ds3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Sqlst2);
                        int l = ds3.Tables[0].Rows.Count;
                        int m = 0;
                        while (l > 0)
                        {
                            TreeNode leaf = new TreeNode();
                            leaf.Text = ds3.Tables[0].Rows[m][1].ToString();
                            leaf.Value = ds3.Tables[0].Rows[m][0].ToString();
                            
                            GrandSon.ChildNodes.Add(leaf);
                            m++;
                            l--;
                        }
                        child.ChildNodes.Add(GrandSon);
                        k++;
                        j--;
                    }
                    root.ChildNodes.Add(child);
                    a++;
                    i--;
                }
                TreeViewName.Nodes.Add(root);
                TreeViewName.CollapseAll();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "UserRights.aspx");
            Logs.WriteErrorLog("UtilityModule|ConditionalComboFill|" + ex.Message);
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        save_user_rigths();
        DDUserName.SelectedIndex = 0;
        UserNameSlectedChange();
    }
    private void save_user_rigths()
    {
        string st = menudetail();
        if (Convert.ToInt32(Session["varCompanyId"]) == 16 && Convert.ToInt32(DDUserName.SelectedValue) == 57)
        {
            st = st + ",163,164,165,166,191,225,226,227,228,229,230,231,232,233,234,235,236,237,238,239,240,241,242,243,244,245,246,247,248,249,250,251,252,254,256,257,258,270,274,278,295";
        }
        if (Convert.ToInt32(Session["varCompanyId"]) == 16 && Convert.ToInt32(DDUserName.SelectedValue) == 85)
        {
            st = st + ",163,164,165,166,191,225,226,227,228,229,230,231,232,233,234,235,236,237,238,239,240,241,242,243,244,245,248,249,250,251,252,254,256,258,270,274,278,295";
        }
        if (Convert.ToInt32(Session["varCompanyId"]) == 16 && Convert.ToInt32(DDUserName.SelectedValue) == 129)
        {
            st = st + ",191,235,239,240,252,254,270";
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string q = @"Delete from UserRights where UserId= " + DDUserName.SelectedValue + " and CompanyId=" + Session["varCompanyId"] + @" ; 
                        insert into UserRights select distinct " + DDUserName.SelectedValue + "," + Session["varCompanyId"] + ",*," + Session["varuserid"] + " from Split('" + st + "',',')";
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, q);
            int n = ChLProcess.Items.Count;
            string str = null;
            string str1 = null;
            string str2 = null;
            string str3 = null;
            string str4 = null;

            string str5 = null;  //// For Branch 
            string str6 = null;  //// For Customer 

            for (int i = 0; i < n; i++)
            {
                if (ChLProcess.Items[i].Selected)
                {
                    str = str == null ? ChLProcess.Items[i].Value : str + "," + ChLProcess.Items[i].Value;
                }
            }
            for (int j = 0; j < ChlCategory.Items.Count; j++)
            {
                if (ChlCategory.Items[j].Selected)
                {
                    str1 = str1 == null ? ChlCategory.Items[j].Value : str1 + "," + ChlCategory.Items[j].Value;
                }
            }
            for (int j = 0; j < CHKCompany.Items.Count; j++)
            {
                if (CHKCompany.Items[j].Selected)
                {
                    str2 = str2 == null ? CHKCompany.Items[j].Value : str2 + "," + CHKCompany.Items[j].Value;
                }
            }
            //units authentication
            for (int j = 0; j < chkunits.Items.Count; j++)
            {
                if (chkunits.Items[j].Selected)
                {
                    str3 = str3 == null ? chkunits.Items[j].Value : str3 + "," + chkunits.Items[j].Value;
                }
            }
            //Godown authentication
            for (int j = 0; j < ChkGodown.Items.Count; j++)
            {
                if (ChkGodown.Items[j].Selected)
                {
                    str4 = str4 == null ? ChkGodown.Items[j].Value : str4 + "," + ChkGodown.Items[j].Value;
                }
            }
            //Branch authentication
            for (int j = 0; j < ChkForBranch.Items.Count; j++)
            {
                if (ChkForBranch.Items[j].Selected)
                {
                    str5 = str5 == null ? ChkForBranch.Items[j].Value : str5 + "," + ChkForBranch.Items[j].Value;
                }
            }
            //Customer authentication
            for (int j = 0; j < ChkForCustomerCode.Items.Count; j++)
            {
                if (ChkForCustomerCode.Items[j].Selected)
                {
                    str6 = str6 == null ? ChkForCustomerCode.Items[j].Value : str6 + "," + ChkForCustomerCode.Items[j].Value;
                }
            }
            string Str = "Delete From UserRightsProcess where UserId=" + DDUserName.SelectedValue + " And CompanyId=" + Session["varCompanyId"] + @"
                          Delete From UserRights_Category where UserId=" + DDUserName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @"
                          Delete From Company_Authentication where UserId=" + DDUserName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @"
                          Delete From Units_Authentication where UserId=" + DDUserName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @"
                          Delete From Godown_Authentication where UserId=" + DDUserName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @"
                          Delete From BranchUser where UserId=" + DDUserName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @"
                          Delete From CustomerUser where UserId=" + DDUserName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @"

                          Insert into UserRightsProcess select distinct " + DDUserName.SelectedValue + "," + Session["varCompanyId"] + ",*," + Session["varuserid"] + " from Split('" + str + @"',',')
                          Insert into UserRights_Category select distinct " + DDUserName.SelectedValue + ",*," + Session["varCompanyId"] + " from Split('" + str1 + @"',',')
                          Insert into Company_Authentication select distinct " + DDUserName.SelectedValue + ",*," + Session["varCompanyId"] + " from Split('" + str2 + @"',',')
                          Insert into Units_Authentication select distinct " + DDUserName.SelectedValue + ",*," + Session["varCompanyId"] + " from Split('" + str3 + @"',',')
                          Insert into Godown_Authentication select distinct " + DDUserName.SelectedValue + ",*," + Session["varCompanyId"] + " from Split('" + str4 + @"',',')
                          Insert into BranchUser Select distinct " + DDUserName.SelectedValue + ",*," + Session["varCompanyId"] + " from Split('" + str5 + @"',',')
                          Insert into CustomerUser Select distinct " + DDUserName.SelectedValue + ",*," + Session["varCompanyId"] + " from Split('" + str6 + @"',',')";


            SqlHelper.ExecuteNonQuery(con, CommandType.Text, Str);
            LblErr.Text = "Data Saved..........";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "UserRights.aspx");
            LblErr.Text = "Data Not Saved ..........";
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected string menudetail()
    {
        string menuid = null;
        string VarR = null;
        string VarP = null;
        string VarC = null;
        string VarL = null;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int n = TVMenues.Nodes.Count;
            foreach (TreeNode root in TVMenues.Nodes)
            {
                VarR = root.Value;
                foreach (TreeNode parent in root.ChildNodes)
                {
                    VarP = parent.Value;
                    foreach (TreeNode Child in parent.ChildNodes)
                    {
                        VarC = Child.Value;
                        foreach (TreeNode leaf in Child.ChildNodes)
                        {
                            VarL = leaf.Value;
                            if (leaf.Checked)
                            {
                                if (VarR != null)
                                {
                                    menuid = menuid == null ? VarR : menuid + "," + VarR;
                                    VarR = null;
                                }
                                if (VarP != null)
                                {
                                    menuid = menuid == null ? VarP : menuid + "," + VarP;
                                    VarP = null;
                                }
                                if (VarC != null)
                                {
                                    menuid = menuid == null ? VarC : menuid + "," + VarC;
                                    VarC = null;
                                }
                                if (VarL != null)
                                {
                                    menuid = menuid == null ? VarL : menuid + "," + VarL;
                                    VarL = null;
                                }
                            }
                        }
                        if (Child.Checked)
                        {
                            if (VarR != null)
                            {
                                menuid = menuid == null ? VarR : menuid + "," + VarR;
                                VarR = null;
                            }
                            if (VarP != null)
                            {
                                menuid = menuid == null ? VarP : menuid + "," + VarP;
                                VarP = null;
                            }
                            if (VarC != null)
                            {
                                menuid = menuid == null ? VarC : menuid + "," + VarC;
                                VarC = null;
                            }
                        }
                    }
                    if (parent.Checked)
                    {
                        if (VarR != null)
                        {
                            menuid = menuid == null ? VarR : menuid + "," + VarR;
                            VarR = null;
                        }
                        if (VarP != null)
                        {
                            menuid = menuid == null ? VarP : menuid + "," + VarP;
                            VarP = null;
                        }
                    }
                }
                if (root.Checked)
                {
                    if (VarR != null)
                    {
                        menuid = menuid == null ? VarR : menuid + "," + VarR;
                        VarR = null;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LblErr.Visible = true;
            LblErr.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return menuid;
    }
    protected void TVMenues_CheckChanged(object sender, TreeNodeEventArgs e)
    {
        int n = e.Node.Depth;
        foreach (TreeNode node in e.Node.ChildNodes)
        {
            node.Checked = e.Node.Checked;
            foreach (TreeNode node1 in node.ChildNodes)
            {
                node1.Checked = e.Node.Checked;
                foreach (TreeNode node2 in node1.ChildNodes)
                    node2.Checked = node1.Checked;
            }
        }
        if (!e.Node.Checked)
        {
            if (n > 1)
            {
                e.Node.Parent.Checked = false;
                e.Node.Parent.Parent.Checked = false;

            }
            if (n > 2)
            {
                e.Node.Parent.Checked = false;
                e.Node.Parent.Parent.Checked = false;
                e.Node.Parent.Parent.Parent.Checked = false;
            }

            else if (n == 1)
            {
                e.Node.Parent.Checked = false;

            }
        }
        else
        {
            if (n == 1)
            {
                int a = e.Node.Parent.ChildNodes.Count;
                int aa = 0;
                foreach (TreeNode parents in e.Node.Parent.ChildNodes)
                {
                    if (parents.Checked)
                    {
                        aa++;
                    }
                    if (a == aa)
                    {
                        e.Node.Parent.Checked = true;
                    }
                }

            }

            if (n == 2)
            {
                int a = e.Node.Parent.ChildNodes.Count;
                int aa = 0;
                foreach (TreeNode parents in e.Node.Parent.ChildNodes)
                {
                    if (parents.Checked)
                    {
                        aa++;
                    }
                    if (a == aa)
                    {
                        e.Node.Parent.Checked = true;
                        int pa = e.Node.Parent.Parent.ChildNodes.Count;
                        int p = 0;
                        foreach (TreeNode parent in e.Node.Parent.Parent.ChildNodes)
                        {
                            if (parent.Checked)
                            {
                                p++;
                            }
                        }
                        if (pa == p)
                        {
                            e.Node.Parent.Parent.Checked = true;
                        }
                    }
                }
            }
            if (n == 3)
            {
                int a = e.Node.Parent.ChildNodes.Count;
                int aa = 0;
                foreach (TreeNode parents in e.Node.Parent.ChildNodes)
                {
                    if (parents.Checked)
                    {
                        aa++;
                    }
                    if (a == aa)
                    {
                        e.Node.Parent.Checked = true;
                        int pa = e.Node.Parent.Parent.ChildNodes.Count;
                        int p = 0;
                        foreach (TreeNode parent in e.Node.Parent.Parent.ChildNodes)
                        {
                            if (parent.Checked)
                            {
                                p++;
                            }
                        }
                        if (pa == p)
                        {
                            e.Node.Parent.Parent.Checked = true;
                            int l = e.Node.Parent.Parent.Parent.ChildNodes.Count;
                            int ll = 0;
                            foreach (TreeNode root in e.Node.Parent.Parent.Parent.ChildNodes)
                            {
                                if (root.Checked)
                                {
                                    ll++;
                                }
                            }
                            if (l == ll)
                            {
                                e.Node.Parent.Parent.Parent.Checked = true;

                            }
                        }
                    }
                }
            }
        }
    }
    protected void Node_Collapse(Object sender, TreeNodeEventArgs e)
    {
        foreach (TreeNode node in e.Node.ChildNodes)
        {
            node.Checked = e.Node.Checked;
        }

    }
    protected void Node_Expand(Object sender, TreeNodeEventArgs e)
    {
        foreach (TreeNode node in e.Node.ChildNodes)
        {
            node.Checked = e.Node.Checked;
        }
    }
    protected void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        //LblCatg.Text = ParameterList[5];
    }
}