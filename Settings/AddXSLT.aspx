<%@ Page Title="" EnableSessionState="True" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="AddXSLT.aspx.cs" Inherits="Settings_AddXSLT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageStyles" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageBody" runat="Server">
    <div id="title-breadcrumb-option-demo" class="page-title-breadcrumb">
        <div class="row">
            <div class="col-lg-12">
                <div class="page-header pull-left">
                    <div class="page-title">Add formate for Document</div>
                </div>
            </div>

        </div>

        <div class="clearfix"></div>
    </div>
    <div class="page-content">
        <div class="panel">
            <div class="panel-body">

                <div class="row">
                    <div class="col-lg-3">
                        <div class="form-group">
                            <label>DocumentType:</label>
                            <asp:DropDownList ID="ddlDocument" CssClass="form-control required" runat="server">
                            </asp:DropDownList>

                        </div>
                    </div>
                    <div class="col-lg-3">
                        <div class="form-group">
                            <label>Customer :</label>
                            <asp:DropDownList ID="ddlCustomer" CssClass="form-control required" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="form-group">
                            <label>Document Formate :</label>
                            
                            <asp:FileUpload ID="flpFormate" CssClass="form-control" runat="server" />
                            
                        </div>
                    </div>
                </div>
                

                <div class="row">
                    <div class="col-lg-12">
                        <button type="button" class="btn btn-md btn-red mrl">Cancel</button>
                        <button type="submit" class="btn btn-md btn-black mrl">Save Changes</button>



                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageScripts" runat="Server">
</asp:Content>

