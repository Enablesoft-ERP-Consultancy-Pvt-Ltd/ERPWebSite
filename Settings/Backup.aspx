<%@ Page Title="" Language="C#" MasterPageFile="~/App.master" AutoEventWireup="true"
    CodeFile="Backup.aspx.cs" Inherits="Settings_Backup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="styleSection" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodySection" runat="Server">
    <div id="title-breadcrumb-option-demo" class="page-title-breadcrumb">
        <div class="row">
            <div class="col-lg-12">
                <div class="page-header pull-left">
                    <div class="page-title">BackUP Database</div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
    </div>
    <div class="page-content">
        <div class="card">
            <div class="card-body">
                <div class="row">
                    <div class="col-lg-6">
                        <div class="mb-3">
                            <label class="form-label">Database:</label>
                            <asp:DropDownList ID="ddlDatabase" CssClass="form-control required" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="mb-3">
                            <asp:LinkButton ID="btnSave" CssClass="btn btn-md btn-black mrl" runat="server" OnClick="btnSave_Click">Save Changes</asp:LinkButton>
                        </div>
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
                                <asp:Repeater ID="rptBackup" runat="server" OnItemCommand="rptBackup_ItemCommand">
                                    <HeaderTemplate>
                                        <thead>
                                            <tr class="bg-table-header">
                                                <th width="80%">Database Name</th>
                                                <th width="20%"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("DatabaseName") %>
                                                <asp:HiddenField ID="hdnBackupId" runat="server" Value='<%# Eval("BackupId") %>' />
                                                <asp:HiddenField ID="hdnPath" runat="server" Value='<%# Eval("BackupPath") %>' />
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


