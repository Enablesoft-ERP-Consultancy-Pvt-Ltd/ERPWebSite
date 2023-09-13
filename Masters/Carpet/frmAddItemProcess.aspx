<%@ Page Language="C#" MasterPageFile="~/PopUp.master" AutoEventWireup="true" CodeFile="frmAddItemProcess.aspx.cs"
    Inherits="Masters_Process_frmAddItemProcess" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBody" runat="Server">



    <!--BEGIN PAGE WRAPPER-->
    <div id="title-breadcrumb-option-demo" class="page-title-breadcrumb">
        <div class="page-header pull-left">
            <div class="page-title">Add Jobs</div>
        </div>
        <ol class="breadcrumb page-breadcrumb">
        </ol>
        <div class="clearfix"></div>
    </div>


    <!--END TITLE & BREADCRUMB PAGE-->
    <!--BEGIN CONTENT-->


    <div class="page-content">
        <div class="card">
            <div class="card-body">
                <div class="row">
                    <div class="col-lg-4">
                        <div class="mb-3">
                            Item Description:
                                <asp:DropDownList ID="ddlItemList" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlItemList_SelectedIndexChanged"
                                    runat="server">
                                </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="mb-3">
                            Quality:
                                <asp:DropDownList ID="ddlQuality" CssClass="form-select" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="ddlQuality_SelectedIndexChanged">
                                </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="mb-3">
                            Design:
                                <asp:DropDownList ID="ddlDesign" CssClass="form-select" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="ddlDesign_SelectedIndexChanged">
                                </asp:DropDownList>

                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-12">
                        <div class="mb-3">
                            Process Type:
                                        <asp:RadioButtonList ID="rdbtnLst" runat="server" RepeatDirection="Horizontal" DataValueField="ItemId"
                                            DataTextField="ItemName">
                                        </asp:RadioButtonList>
                        </div>
                    </div>

                </div>
            </div>


            <div class="card-body">


                <div class="row">
                    <div class="col-lg-5">
                        <asp:ListBox ID="lstProcess" CssClass="form-select" runat="server" Height="200px" SelectionMode="Single">
                        </asp:ListBox>
                    </div>
                    <div class="col-lg-2 text-center pt-0 pt-md-5">
             <asp:Button ID="btngo" runat="server" Text=">>"  OnClick="btngo_Click" /><br />
        <br />
        <asp:Button ID="btnDelete" runat="server" Text="<<" OnClick="btnDelete_Click" />
                    </div>
                    <div class="col-lg-5">
                        <asp:ListBox ID="lstSelectProcess" CssClass="form-select" runat="server" Height="200px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </div>
                </div>

                <div class="row mt-5">
                    <div class="col-md-offset-3 col-md-6">

                        <asp:Button ID="btnsave" runat="server" CssClass="btn btn-primary" Text="Save" Width="75px"
                            OnClick="btnsave_Click" />
                        &nbsp;&nbsp;<asp:Button ID="btnClose" runat="server" CssClass="btn btn-default" Text="Close"
                            Width="75px" OnClientClick="return CloseForm();" />
                    </div>

                </div>

            </div>
        </div>
    </div>





    <div style="float: left; margin-left: 30px; margin-top: 50px">

    </div>


    <asp:Label ID="lblItemName" runat="server" Style="font-weight: bold; font-size: 20px;
        color: Red"
        Text="MK"></asp:Label>


    <div id="TDquality" runat="server">
        <asp:Label ID="lblquality" runat="server" Text="QUALITY NAME" CssClass="labelbold"></asp:Label>

    </div>

    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="labelnormalMM" ForeColor="Red"></asp:Label>


    <div id="TDDesign" runat="server" visible="false">
        <asp:Label ID="Label1" runat="server" Text="DESIGN NAME" CssClass="labelbold"></asp:Label>

    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="Server">
</asp:Content>







