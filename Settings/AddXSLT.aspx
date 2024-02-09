<%@ Page Title="" EnableSessionState="True" Language="C#" MasterPageFile="~/App.master"
    AutoEventWireup="true" ValidateRequest="false"
    CodeFile="AddXSLT.aspx.cs" Inherits="Settings_AddXSLT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="styleSection" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodySection" runat="Server">
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
        <div class="card">
            <div class="card-body">
                <div class="row">
                    <div class="col-lg-3">
                        <div class="mb-3">
                            <label class="form-label">DocumentType:</label>
                            <asp:DropDownList ID="ddlDocument" CssClass="form-control required" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-3">
                        <div class="mb-3">
                            <label class="form-label">Print Type :</label>


                            <asp:DropDownList ID="ddlPrintType" runat="server" CssClass="form-control required">
                                <asp:ListItem Value="1">Type 1</asp:ListItem>
                                <asp:ListItem Value="2">Type 2</asp:ListItem>
                                <asp:ListItem Value="3">Type 3</asp:ListItem>
                                <asp:ListItem Value="4">Type 4</asp:ListItem>
                                <asp:ListItem Value="5">Type 5</asp:ListItem>
                            </asp:DropDownList>



                        </div>
                    </div>



                    <div class="col-lg-3">
                        <div class="mb-3">
                            <label class="form-label">Customer :</label>
                            <asp:DropDownList ID="ddlCustomer" CssClass="form-control required" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>














                    <div class="col-lg-3">
                        <div class="mb-3">
                            <label class="form-label">Document Formate :</label>

                            <asp:FileUpload ID="flpContent" CssClass="form-control" runat="server" />

                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-12">


                        <div class="mb-3">
                            <label class="form-label">Document Formate :</label>
                            <asp:TextBox ID="txtContent" TextMode="MultiLine" CssClass="form-control" Rows="10"
                                runat="server"></asp:TextBox>

                        </div>







                    </div>
                </div>



                <div class="row">
                    <div class="col-lg-12">
                        <asp:LinkButton ID="btnCancel" CssClass="btn btn-md btn-red mrl" runat="server">Cancel</asp:LinkButton>
                        <asp:LinkButton ID="btnSave" CssClass="btn btn-md btn-black mrl" runat="server" OnClick="btnSave_Click">Save Changes</asp:LinkButton>

                    </div>
                </div>
            </div>
        </div>
        <div class="card">
            <div class="card-body">
                <div class="row">

                    <div class="col-lg-4">
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered table-striped mbn">
                                <asp:Repeater ID="rptDoc" runat="server" OnItemCommand="rptDoc_ItemCommand">
                                    <HeaderTemplate>
                                        <thead>
                                            <tr class="bg-table-header">
                                                <th width="80%">Document Type</th>
                                                <th width="20%"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>

                                                <%# Eval("Title") %>

                                                <%# Eval("DocumentType") %>
                                                <asp:HiddenField ID="hdnDocId" runat="server" Value='<%# Eval("XsltId") %>' />

                                            </td>
                                            <td>
                                                <asp:Literal ID="lblText" runat="server"></asp:Literal>
                                                <asp:LinkButton ID="lbtnDel" Text="<i class='fa fa-trash-o text-red mrm mediumtxt' title='Detail'></i>View"
                                                    CommandName="Delete" CssClass="btn btn-xs btn-default" runat="server"></asp:LinkButton>
                                                <asp:LinkButton ID="lbtnView" Text="<i class='fa fa-info-circle mrm mediumtxt' title='Detail'></i>View"
                                                    CommandName="View" CssClass="btn btn-xs btn-default" runat="server"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                    </FooterTemplate>

                                </asp:Repeater>
                            </table>
                        </div>
                    </div>


                    <div class="col-lg-8">

                        <asp:Literal ID="lblText" runat="server"></asp:Literal>
                        <asp:Xml ID="xmlText" runat="server" Visible="false"></asp:Xml>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptSection" runat="Server">
</asp:Content>

