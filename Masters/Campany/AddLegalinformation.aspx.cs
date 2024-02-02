using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO.Compression;
using System.IO;
using System.Data.SqlTypes;
using System.Net;
public partial class Masters_Campany_AddLegalinformation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDbankname, "select bankid,bankname from bank where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + "order by bankname", true, "--plz select bank");
            fill_grid();

            int VarCompanyNo = Convert.ToInt32(Session["varMasterCompanyIDForERP"]);
            switch (VarCompanyNo)
            {
                case 30:
                    TRRex.Visible = true;
                    break;               

            }

        }

    }
    protected void fill_grid()
    {

        string strsql = @"select Legalname,RegisterAddress,Legalstatus,Replace(convert(nvarchar(11),Dateofestablishment,106),' ','-') As Dateofestablishment,IncorporationNumber,PlaceofIncorporation,TinNo,AssociationName,
                          Registrationno,Replace(convert(nvarchar(11),RegDate,106),' ','-') As Regdate,Replace(convert(nvarchar(11),Regexpdate,106),' ','-') As Regexpdate,IecNumber,Issuingauth_iec,CommissionRate,Circle,A.Bankid,panGirno ,Replace(convert(nvarchar(11),Dateofiss_Pan,106),' ','-') As DateofIss_pan,IssuingAuth_pan,
                          SsiNumber,Replace(convert(nvarchar(11),DateofIss_ssi,106),' ','-') As DateofIss_ssi,IssueAuth_ssi,IndustrialLicenceIem,Replace(convert(nvarchar(11),DateofIss_ind,106),' ','-') As DateofIss_ind,IssuingAuth_ind,CstNo,Replace(convert(nvarchar(11),Dateofiss_cst,106),' ','-') As  Dateofiss_cst,ServiceTaxRegno,
                          StNo,Replace(convert(nvarchar(11),DateofIss_st,106),' ','-') As DateofIss_st,Replace(convert(nvarchar(11),ServiceTaxregDate,106),' ','-') As ServiceTaxRegDate,GspRegistrationNo,Replace(convert(nvarchar(11),DateofIss_gsp,106),' ','-') As DateofIss_gsp,Place_gsp,GspAccountNo,Replace(convert(nvarchar(11),DateofIss_gspac,106),' ','-') As DateofIss_gspac,Place
                           ,TypeofCertificate,Replace(convert(nvarchar(11),IssueDate,106),' ','-') As IssueDate,CertificateNo,Replace(convert(nvarchar(11),ValidityDate,106),' ','-') As ValidityDate ,companyid
                        ,B.BankName,A.RexNo, Replace(convert(nvarchar(11),A.RexIssueDate,106),' ','-') As RexIssueDate,Replace(convert(nvarchar(11),A.RexExpiryDate,106),' ','-') As RexExpiryDate,A.IssueBodyNo
                        from Addlegalinformation A left outer join Bank B on A.BankId=B.BankId Where
                         A.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And A.CompanyId=" + Request.QueryString["a"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);


        if (ds.Tables[0].Rows.Count > 0)
        {
            Dglegal.DataSource = ds.Tables[0];
            Dglegal.DataBind();
        }
        else
        {
            Dglegal.DataSource = null;
            Dglegal.DataBind();
        }
    }
    protected void Refresh_form()
    {
        txtlegalName.Text = "";
        txtregisteraddress.Text = "";
        DDlegalstatus.SelectedIndex = -1;
        txtdoe.Text = "";
        txtin.Text = "";
        txtpoi.Text = "";
        txttinno.Text = "";
        txtassociationname.Text = "";
        txtregistrationno.Text = "";
        txtregdate.Text = "";
        txtregexdate.Text = "";
        txtiecnumber.Text = "";
        txtissauthority.Text = "";
        txtcr.Text = "";
        txtcircle.Text = "";
        DDbankname.SelectedIndex = 0;
        txtpanno.Text = "";
        txtpandate.Text = "";
        txtissuingauth.Text = "";
        txtssino.Text = "";
        txtssidate.Text = "";
        txtissueauth.Text = "";
        txtil.Text = "";
        txtindustrialdate.Text = "";
        txtissuingauthority.Text = "";
        txtcstno.Text = "";
        txtcstdate.Text = "";
        txtservicetaxregno.Text = "";
        txtstno.Text = "";
        txtstdate.Text = "";
        txtservicetrd.Text = "";
        txtgsprno.Text = "";
        txtgspdate.Text = "";
        txtplace.Text = "";
        txtgspaccountno.Text = "";
        txtgspacdate.Text = "";
        txtplac.Text = "";
        txttypeoc.Text = "";
        txtcertificatedate.Text = "";
        txtcertificateno.Text = "";
        txtcertificatevaldate.Text = "";
        txtRexNo.Text = "";
        txtRexIssueDate.Text = "";
        txtRexExpiryDate.Text = "";
        txtIssueBodyNo.Text = "";
    }
    protected void lnkbtnName_Click(object sender, EventArgs e)
    {

        ModalPopupExtender1.Show();
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            int CompanyId = Convert.ToInt16(((Label)Dglegal.Rows[grv.RowIndex].FindControl("lblcompanyid")).Text);
            //int ProcessId = Convert.ToInt16(((Label)DGStock.Rows[grv.RowIndex].FindControl("lblProcessId")).Text);
            // lblAgentName.Text = "Agent-" + DGStock.Rows[grv.RowIndex].Cells[1].Text;
            string str = @"select certificateid,filename from companycertificate where companyid=" + CompanyId;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            GDLinkedtoCustomer.DataSource = ds;
            GDLinkedtoCustomer.DataBind();
        }
    }
    protected void lnkbtnName1_Click(object sender, EventArgs e)
    {
        try
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            LinkButton lnk = sender as LinkButton;
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            int certificateid = Convert.ToInt16(((Label)GDLinkedtoCustomer.Rows[grv.RowIndex].FindControl("lblcertificateid")).Text);

            string FileName = ((Label)GDLinkedtoCustomer.Rows[grv.RowIndex].FindControl("lblFileName")).Text;
            if (File.Exists(Server.MapPath("~/Certificate_Image/" + certificateid)))
            {
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                Response.TransmitFile(HttpContext.Current.Server.MapPath("~/Certificate_Image/" + certificateid));

                Response.Flush();
                Response.End();
            }
        }
        catch
        {
        }
        finally
        {
        }



    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[49];
            _arrpara[0] = new SqlParameter("@Legalname", SqlDbType.VarChar, 50);
            _arrpara[1] = new SqlParameter("@RegisterAddress", SqlDbType.VarChar, 50);
            _arrpara[2] = new SqlParameter("@Legalstatus", SqlDbType.VarChar, 50);
            _arrpara[3] = new SqlParameter("@Dateofestablishment", SqlDbType.SmallDateTime);
            _arrpara[4] = new SqlParameter("@IncorporationNumber", SqlDbType.VarChar, 50);
            _arrpara[5] = new SqlParameter("@PlaceofIncorporation", SqlDbType.VarChar, 50);
            _arrpara[6] = new SqlParameter("@TinNo", SqlDbType.VarChar, 50);
            _arrpara[7] = new SqlParameter("@AssociationName", SqlDbType.VarChar, 50);
            _arrpara[8] = new SqlParameter("@Registrationno", SqlDbType.VarChar, 50);
            _arrpara[9] = new SqlParameter("@Regdate", SqlDbType.SmallDateTime);
            _arrpara[10] = new SqlParameter("@Regexpdate", SqlDbType.SmallDateTime);
            _arrpara[11] = new SqlParameter("@IecNumber", SqlDbType.VarChar, 50);
            _arrpara[12] = new SqlParameter("@Issuingauth_iec", SqlDbType.VarChar, 50);
            _arrpara[13] = new SqlParameter("@CommissionRate", SqlDbType.VarChar, 50);
            _arrpara[14] = new SqlParameter("@Circle", SqlDbType.VarChar, 50);
            _arrpara[15] = new SqlParameter("@Bankid", SqlDbType.Int);
            _arrpara[16] = new SqlParameter("@panGirno", SqlDbType.VarChar, 50);
            _arrpara[17] = new SqlParameter("@DateofIss_pan", SqlDbType.SmallDateTime);
            _arrpara[18] = new SqlParameter("@IssuingAuth_pan", SqlDbType.VarChar, 50);
            _arrpara[19] = new SqlParameter("@SsiNumber", SqlDbType.VarChar, 50);
            _arrpara[20] = new SqlParameter("@DateofIss_ssi", SqlDbType.SmallDateTime);
            _arrpara[21] = new SqlParameter("@IssueAuth_ssi", SqlDbType.VarChar, 50);
            _arrpara[22] = new SqlParameter("@IndustrialLicenceIem", SqlDbType.VarChar, 50);
            _arrpara[23] = new SqlParameter("@DateofIss_ind", SqlDbType.SmallDateTime);
            _arrpara[24] = new SqlParameter("@IssuingAuth_ind", SqlDbType.VarChar, 50);
            _arrpara[25] = new SqlParameter("@CstNo", SqlDbType.VarChar, 50);
            _arrpara[26] = new SqlParameter("@Dateofiss_cst", SqlDbType.SmallDateTime);
            _arrpara[27] = new SqlParameter("@ServiceTaxRegno", SqlDbType.VarChar, 50);
            _arrpara[28] = new SqlParameter("@StNo", SqlDbType.VarChar, 50);
            _arrpara[29] = new SqlParameter("@DateofIss_st", SqlDbType.SmallDateTime);
            _arrpara[30] = new SqlParameter("@ServiceTaxRegDate", SqlDbType.SmallDateTime);
            _arrpara[31] = new SqlParameter("@GspRegistrationNo", SqlDbType.VarChar, 50);
            _arrpara[32] = new SqlParameter("@DateofIss_gsp", SqlDbType.SmallDateTime);
            _arrpara[33] = new SqlParameter("@Place_gsp", SqlDbType.VarChar, 50);
            _arrpara[34] = new SqlParameter("@GspAccountNo", SqlDbType.VarChar, 50);
            _arrpara[35] = new SqlParameter("@DateofIss_gspac", SqlDbType.SmallDateTime);
            _arrpara[36] = new SqlParameter("@Place", SqlDbType.VarChar, 50);
            _arrpara[37] = new SqlParameter("@TypeofCertificate", SqlDbType.VarChar, 50);
            _arrpara[38] = new SqlParameter("@IssueDate", SqlDbType.SmallDateTime);
            _arrpara[39] = new SqlParameter("@CertificateNo", SqlDbType.VarChar, 50);
            _arrpara[40] = new SqlParameter("@ValidityDate", SqlDbType.SmallDateTime);
            _arrpara[41] = new SqlParameter("@companyid", SqlDbType.Int);
            _arrpara[42] = new SqlParameter("@userid", SqlDbType.Int);
            _arrpara[43] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
            _arrpara[44] = new SqlParameter("@msg", SqlDbType.VarChar, 100);

            _arrpara[45] = new SqlParameter("@RexNo", SqlDbType.VarChar, 50);
            _arrpara[46] = new SqlParameter("@RexIssueDate", SqlDbType.DateTime);
            _arrpara[47] = new SqlParameter("@RexExpiryDate", SqlDbType.DateTime);
            _arrpara[48] = new SqlParameter("@IssueBodyNo", SqlDbType.VarChar, 50);

            System.Data.SqlTypes.SqlDateTime getDate;
            //set DateTime null
            getDate = SqlDateTime.Null;

            _arrpara[0].Value = txtlegalName.Text.ToUpper();
            _arrpara[1].Value = txtregisteraddress.Text.ToUpper();
            _arrpara[2].Value = DDlegalstatus.SelectedItem.Text.ToUpper();
            _arrpara[3].Value = txtdoe.Text == "" ? getDate : Convert.ToDateTime(txtdoe.Text);
            _arrpara[4].Value = txtin.Text.ToUpper();
            _arrpara[5].Value = txtpoi.Text.ToUpper();
            _arrpara[6].Value = txttinno.Text.ToUpper();
            _arrpara[7].Value = txtassociationname.Text.ToUpper();
            _arrpara[8].Value = txtregistrationno.Text.ToUpper();
            _arrpara[9].Value = txtregdate.Text == "" ? getDate : Convert.ToDateTime(txtregdate.Text);
            _arrpara[10].Value = txtregexdate.Text == "" ? getDate : Convert.ToDateTime(txtregexdate.Text);
            _arrpara[11].Value = txtiecnumber.Text.ToUpper();
            _arrpara[12].Value = txtissauthority.Text.ToUpper();
            _arrpara[13].Value = txtcr.Text.ToUpper();
            _arrpara[14].Value = txtcircle.Text.ToUpper();
            _arrpara[15].Value = DDbankname.SelectedValue.ToUpper();
            _arrpara[16].Value = txtpanno.Text.ToUpper();
            _arrpara[17].Value = txtpandate.Text == "" ? getDate : Convert.ToDateTime(txtpandate.Text);
            _arrpara[18].Value = txtissuingauth.Text.ToUpper();
            _arrpara[19].Value = txtssino.Text.ToUpper();
            _arrpara[20].Value = txtssidate.Text == "" ? getDate : Convert.ToDateTime(txtssidate.Text);
            _arrpara[21].Value = txtissueauth.Text.ToUpper();
            _arrpara[22].Value = txtil.Text.ToUpper();
            _arrpara[23].Value = txtindustrialdate.Text == "" ? getDate : Convert.ToDateTime(txtindustrialdate.Text);
            _arrpara[24].Value = txtissuingauthority.Text.ToUpper();
            _arrpara[25].Value = txtcstno.Text.ToUpper();
            _arrpara[26].Value = txtcstdate.Text == "" ? getDate : Convert.ToDateTime(txtcstdate.Text);
            _arrpara[27].Value = txtservicetaxregno.Text.ToUpper();
            _arrpara[28].Value = txtstno.Text.ToUpper();
            _arrpara[29].Value = txtstdate.Text == "" ? getDate : Convert.ToDateTime(txtstdate.Text);
            _arrpara[30].Value = txtservicetrd.Text == "" ? getDate : Convert.ToDateTime(txtservicetrd.Text);
            _arrpara[31].Value = txtgsprno.Text.ToUpper();
            _arrpara[32].Value = txtgspdate.Text == "" ? getDate : Convert.ToDateTime(txtgspdate.Text);
            _arrpara[33].Value = txtplace.Text.ToUpper();
            _arrpara[34].Value = txtgspaccountno.Text.ToUpper();
            _arrpara[35].Value = txtgspacdate.Text == "" ? getDate : Convert.ToDateTime(txtgspacdate.Text);
            _arrpara[36].Value = txtplac.Text.ToUpper();
            _arrpara[37].Value = txttypeoc.Text.ToUpper();
            _arrpara[38].Value = txtcertificatedate.Text == "" ? getDate : Convert.ToDateTime(txtcertificatedate.Text);
            _arrpara[39].Value = txtcertificateno.Text.ToUpper();
            _arrpara[40].Value = txtcertificatevaldate.Text == "" ? getDate : Convert.ToDateTime(txtcertificatevaldate.Text);
            _arrpara[41].Value = Request.QueryString["a"].ToString();
            _arrpara[42].Value = Session["varuserid"].ToString();
            _arrpara[43].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrpara[44].Direction = ParameterDirection.Output;

            _arrpara[45].Value = txtRexNo.Text.ToUpper();
            _arrpara[46].Value = txtRexIssueDate.Text == "" ? getDate : Convert.ToDateTime(txtRexIssueDate.Text);
            _arrpara[47].Value = txtRexExpiryDate.Text == "" ? getDate : Convert.ToDateTime(txtRexExpiryDate.Text);           
            _arrpara[48].Value = txtIssueBodyNo.Text;


            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Addlegalinformation", _arrpara);
            lblErr.Visible = true;
            lblErr.Text = _arrpara[44].Value.ToString();
            Tran.Commit();
            btnSave.Text = "Save";
            fill_grid();
        }
        catch (Exception ex)
        {
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
            Tran.Rollback();
            Logs.WriteErrorLog("Master_Company_Customer_AddLegalinfomation|BtnSave_Click|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master_Company_Customer_AddLegalinfomation.aspx");

        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (con != null)
            {
                con.Dispose();
            }
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["id"] == "1")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "OnLoad", "onSuccess();", true);

                }
            }
            fill_grid();
        }
        Refresh_form();
    }
    protected void UploadButton_Click(object sender, EventArgs e)
    {
        if (FileUploadControl.HasFile)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();

            try
            {

                string filename = Path.GetFileName(FileUploadControl.FileName);
                SqlParameter[] _arrpara = new SqlParameter[5];
                _arrpara[0] = new SqlParameter("@CertificateId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@filename", SqlDbType.VarChar, 50);
                _arrpara[3] = new SqlParameter("@userid", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@mastercompanyid", SqlDbType.Int);

                _arrpara[0].Direction = ParameterDirection.Output;
                _arrpara[1].Value = Request.QueryString["a"].ToString();
                _arrpara[2].Value = filename;
                _arrpara[3].Value = Session["varuserid"].ToString();
                _arrpara[4].Value = Session["varMasterCompanyIDForERP"].ToString();

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Company_Certificate", _arrpara);
                Tran.Commit();
                string filepath = Path.GetFullPath(FileUploadControl.PostedFile.FileName);
                string targetpath = Server.MapPath("~/Certificate_Image/" + _arrpara[0].Value + "");
                //Save File into a Folder
                FileUploadControl.PostedFile.SaveAs(targetpath);
                //RegularExpressionValidator1.ErrorMessage = "";
                StatusLabel.Visible = true;
                StatusLabel.Text = "Upload Successfully";
            }
            catch (Exception ex)
            {

                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddLegalinformation.aspx");
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void Dglegal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Dglegal, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < Dglegal.Columns.Count; i++)
            {              

                if (Session["varMasterCompanyIDForERP"].ToString() == "30")
                {
                    if (Dglegal.Columns[i].HeaderText == "RexNo" || Dglegal.Columns[i].HeaderText == "Rex IssueDate" || Dglegal.Columns[i].HeaderText == "Rex ExpiryDate" || Dglegal.Columns[i].HeaderText == "Issue BodyNo")
                    {
                        Dglegal.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (Dglegal.Columns[i].HeaderText == "RexNo" || Dglegal.Columns[i].HeaderText == "Rex IssueDate" || Dglegal.Columns[i].HeaderText == "Rex ExpiryDate" || Dglegal.Columns[i].HeaderText == "Issue BodyNo")
                    {
                        Dglegal.Columns[i].Visible = false;
                    }

                }
            }
        }
    }
    protected void Dglegal_RowCreated(object sender, GridViewRowEventArgs e)
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


    protected void Dglegal_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, @"select Legalname,RegisterAddress,Legalstatus,Replace(convert(nvarchar(11),Dateofestablishment,106),' ','-') As Dateofestablishment,IncorporationNumber,PlaceofIncorporation,TinNo,AssociationName,
                          Registrationno,Replace(convert(nvarchar(11),RegDate,106),' ','-') As Regdate,Replace(convert(nvarchar(11),Regexpdate,106),' ','-') As Regexpdate,IecNumber,Issuingauth_iec,CommissionRate,Circle,A.Bankid,panGirno ,Replace(convert(nvarchar(11),Dateofiss_Pan,106),' ','-') As DateofIss_pan,IssuingAuth_pan,
                          SsiNumber,Replace(convert(nvarchar(11),DateofIss_ssi,106),' ','-') As DateofIss_ssi,IssueAuth_ssi,IndustrialLicenceIem,Replace(convert(nvarchar(11),DateofIss_ind,106),' ','-') As DateofIss_ind,IssuingAuth_ind,CstNo,Replace(convert(nvarchar(11),Dateofiss_cst,106),' ','-') As  Dateofiss_cst,ServiceTaxRegno,
                          StNo,Replace(convert(nvarchar(11),DateofIss_st,106),' ','-') As DateofIss_st,Replace(convert(nvarchar(11),ServiceTaxregDate,106),' ','-') As ServiceTaxRegDate,GspRegistrationNo,Replace(convert(nvarchar(11),DateofIss_gsp,106),' ','-') As DateofIss_gsp,Place_gsp,GspAccountNo,Replace(convert(nvarchar(11),DateofIss_gspac,106),' ','-') As DateofIss_gspac,Place
                           ,TypeofCertificate,Replace(convert(nvarchar(11),IssueDate,106),' ','-') As IssueDate,CertificateNo,Replace(convert(nvarchar(11),ValidityDate,106),' ','-') As ValidityDate ,companyid
                        ,B.BankName ,A.RexNo, Replace(convert(nvarchar(11),A.RexIssueDate,106),' ','-') As RexIssueDate,Replace(convert(nvarchar(11),A.RexExpiryDate,106),' ','-') As RexExpiryDate,A.IssueBodyNo
                         from Addlegalinformation A left outer join Bank B on A.BankId=B.BankId Where 
                         A.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And A.CompanyId=" + Request.QueryString["a"]);

            if (ds.Tables[0].Rows.Count > 0)
            {

                txtlegalName.Text = ds.Tables[0].Rows[0]["Legalname"].ToString();
                txtregisteraddress.Text = ds.Tables[0].Rows[0]["RegisterAddress"].ToString();
                DDlegalstatus.SelectedItem.Text = ds.Tables[0].Rows[0]["Legalstatus"].ToString();
                txtdoe.Text = ds.Tables[0].Rows[0]["Dateofestablishment"].ToString();
                txtin.Text = ds.Tables[0].Rows[0]["IncorporationNumber"].ToString();
                txtpoi.Text = ds.Tables[0].Rows[0]["PlaceofIncorporation"].ToString();
                txttinno.Text = ds.Tables[0].Rows[0]["TinNo"].ToString();
                txtassociationname.Text = ds.Tables[0].Rows[0]["AssociationName"].ToString();
                txtregistrationno.Text = ds.Tables[0].Rows[0]["Registrationno"].ToString();
                txtregdate.Text = ds.Tables[0].Rows[0]["Regdate"].ToString();
                txtregexdate.Text = ds.Tables[0].Rows[0]["Regexpdate"].ToString();
                txtiecnumber.Text = ds.Tables[0].Rows[0]["IecNumber"].ToString();
                txtissauthority.Text = ds.Tables[0].Rows[0]["Issuingauth_iec"].ToString();
                txtcr.Text = ds.Tables[0].Rows[0]["CommissionRate"].ToString();
                txtcircle.Text = ds.Tables[0].Rows[0]["Circle"].ToString();
                DDbankname.SelectedValue = ds.Tables[0].Rows[0]["BankId"].ToString();
                txtpanno.Text = ds.Tables[0].Rows[0]["panGirno"].ToString();
                txtpandate.Text = ds.Tables[0].Rows[0]["DateofIss_pan"].ToString();
                txtissuingauth.Text = ds.Tables[0].Rows[0]["IssuingAuth_pan"].ToString();
                txtssino.Text = ds.Tables[0].Rows[0]["SsiNumber"].ToString();
                txtssidate.Text = ds.Tables[0].Rows[0]["DateofIss_ssi"].ToString();
                txtissueauth.Text = ds.Tables[0].Rows[0]["IssueAuth_ssi"].ToString();
                txtil.Text = ds.Tables[0].Rows[0]["IndustrialLicenceIem"].ToString();
                txtindustrialdate.Text = ds.Tables[0].Rows[0]["DateofIss_ind"].ToString();
                txtissuingauthority.Text = ds.Tables[0].Rows[0]["IssuingAuth_ind"].ToString();
                txtcstno.Text = ds.Tables[0].Rows[0]["CstNo"].ToString();
                txtcstdate.Text = ds.Tables[0].Rows[0]["Dateofiss_cst"].ToString();
                txtservicetaxregno.Text = ds.Tables[0].Rows[0]["ServiceTaxRegno"].ToString();
                txtstno.Text = ds.Tables[0].Rows[0]["StNo"].ToString();
                txtstdate.Text = ds.Tables[0].Rows[0]["DateofIss_st"].ToString();
                txtservicetrd.Text = ds.Tables[0].Rows[0]["ServiceTaxRegDate"].ToString();
                txtgsprno.Text = ds.Tables[0].Rows[0]["GspRegistrationNo"].ToString();
                txtgspdate.Text = ds.Tables[0].Rows[0]["DateofIss_gsp"].ToString();
                txtplace.Text = ds.Tables[0].Rows[0]["Place_gsp"].ToString();
                txtgspaccountno.Text = ds.Tables[0].Rows[0]["GspAccountNo"].ToString();
                txtgspacdate.Text = ds.Tables[0].Rows[0]["DateofIss_gspac"].ToString();
                txtplac.Text = ds.Tables[0].Rows[0]["Place"].ToString();
                txttypeoc.Text = ds.Tables[0].Rows[0]["TypeofCertificate"].ToString();
                txtcertificatedate.Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
                txtcertificateno.Text = ds.Tables[0].Rows[0]["CertificateNo"].ToString();
                txtcertificatevaldate.Text = ds.Tables[0].Rows[0]["ValidityDate"].ToString();
                txtRexNo.Text = ds.Tables[0].Rows[0]["RexNo"].ToString();
                txtRexIssueDate.Text = ds.Tables[0].Rows[0]["RexIssueDate"].ToString();
                txtRexExpiryDate.Text = ds.Tables[0].Rows[0]["RexExpiryDate"].ToString();
                txtIssueBodyNo.Text= ds.Tables[0].Rows[0]["IssueBodyNo"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddLegalinformation.aspx");
            Logs.WriteErrorLog("Masters_Campany_AddLegalinformation|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        btnSave.Text = "Update";
    }
    protected void Dglegal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (hnforDelete.Value == "true")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                string str = "";
                str = "Delete from addlegalinformation where companyid=" + Request.QueryString["a"];
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                str = "select certificateid from companycertificate Where CompanyId=" + Request.QueryString["a"];
                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                string targetpath = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        targetpath = Server.MapPath("~/Certificate_Image/" + ds.Tables[0].Rows[i]["certificateid"] + "");
                    if (File.Exists(targetpath))
                    {
                        File.Delete(targetpath);
                    }
                }
                Tran.Commit();
                lblErr.Text = "Data Deleted successfully........";


            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddLegalinformation.aspx");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            fill_grid();
            Refresh_form();
        }

    }
}
