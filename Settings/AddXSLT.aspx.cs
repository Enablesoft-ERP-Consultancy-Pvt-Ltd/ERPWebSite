using IExpro.Core.Interfaces.Service;
using IExpro.Infrastructure.Repository;
using IExpro.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Settings_AddXSLT : System.Web.UI.Page
{
    ICommonService CommSrv;
    int ClientId;

    public Settings_AddXSLT()
    {
        this.CommSrv = new CommonService(new UnitOfWork());
     
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }




        if (!IsPostBack)
        {
            ClientId = Convert.ToInt32(Session["varcompanyNo"].ToString());
            ddlCustomer.DataSource = this.CommSrv.GetCustomerList(ClientId);
            ddlCustomer.DataTextField = "ItemName";
            ddlCustomer.DataValueField = "ItemId";
            ddlCustomer.DataBind();


            ddlDocument.DataSource = this.CommSrv.GetDocTypeList(ClientId);
            ddlDocument.DataTextField = "ItemName";
            ddlDocument.DataValueField = "ItemId";
            ddlDocument.DataBind();
        }
    }

 



}