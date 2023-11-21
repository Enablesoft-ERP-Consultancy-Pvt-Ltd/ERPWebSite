<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.master" AutoEventWireup="true"
    CodeFile="frmAddItemProcess.aspx.cs" Inherits="Masters_Carpet_frmAddItemProcess" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentBody" runat="Server">

        <div class="card">

            <div class="card-header">
                   <div class="card-title">
                <asp:Label ID="lblItemName" runat="server" Text="MK"></asp:Label>
            </div>
                </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-sm-12 text-center">
                        <asp:Label ID="lblMessage" CssClass="label label-danger" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row">

                    <div class="col-sm-6" id="TDquality" runat="server">
                        <div class="mb-3">
                            <label class="form-label">Quality:</label>
                            <asp:DropDownList ID="DDQuality" AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged"
                                CssClass="form-select required" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-sm-6" id="TDDesign" runat="server" visible="false">
                        <div class="mb-3">
                            <label class="form-label">Design:</label>
                            <asp:DropDownList ID="DDDesign" AutoPostBack="true" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged"
                                CssClass="form-select required" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="mb-3">
                            <label class="form-label">Process Type:</label>
                            <asp:RadioButtonList ID="rdbtnLst" runat="server" RepeatDirection="Horizontal" DataValueField="ItemId"
                                DataTextField="ItemName">
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-5">
                        <label class="form-label">Jobs:</label>
                        <asp:ListBox ID="lstProcess" CssClass="form-select" runat="server" Height="200px"
                            SelectionMode="Single"></asp:ListBox>
                    </div>

                    <div class="col-sm-2 text-center" style="margin-top: 8rem!important;">

                        <div class="btn-group-vertical mbm">

                            <asp:LinkButton ID="btngo" CssClass="btn btn-green" runat="server" OnClick="btngo_Click">  <i class="fa fa-angle-double-left"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" CssClass="btn btn-primary" runat="server"
                                OnClick="btnDelete_Click">        <i class="fa fa-angle-double-right"></i></asp:LinkButton>
                        </div>



                    </div>

                    <div class="col-sm-5">
                        <label class="form-label">Selected Jobs:</label>
                        <asp:ListBox ID="lstSelectProcess" CssClass="form-select" runat="server" Height="200px"
                            SelectionMode="Multiple"></asp:ListBox>
                    </div>
                </div>
                <div class="row mt-5">
                    <div class="col-lg-12">
                        <asp:LinkButton ID="btnClose" CssClass="btn btn-md btn-red" runat="server" OnClientClick="return CloseForm();">Close</asp:LinkButton>
                        <asp:LinkButton ID="btnSave" CssClass="btn btn-md btn-black" runat="server" OnClick="btnsave_Click">Save Changes</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="Server">
</asp:Content>

