<%@ Page Title="" Language="C#" MasterPageFile="~/App.master" AutoEventWireup="true"
    CodeFile="Newtheme.aspx.cs" Inherits="Settings_Newtheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="styleSection" runat="Server">
</asp:Content>

<asp:Content ID="page" ContentPlaceHolderID="bodySection" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--BEGIN TITLE & BREADCRUMB PAGE-->
            <div id="title-breadcrumb-option-demo" class="page-title-breadcrumb">
                <div class="page-header pull-left">
                    <div class="page-title">Bank </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <!--END TITLE & BREADCRUMB PAGE-->
            <!--BEGIN CONTENT-->
            <div class="page-content">
                <div class="card mb-3">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-xl-6">
                                <div class="mb-3">
                                    <asp:Label ID="LblItem" runat="server" Text="Bank Name" CssClass="form-label"></asp:Label>
                                    <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" required></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBankName"
                                        CssClass="errormsg" ErrorMessage="Please, Enter Bank Name!" ValidationGroup="m"
                                        SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-xl-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label1" runat="server" Text="Address" CssClass="form-label" />
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" required></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddress"
                                        ErrorMessage="Please,Enter Bank Address" ForeColor="Red" SetFocusOnError="true"
                                        ValidationGroup="m">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label2" runat="server" Text="City" CssClass="form-label" />
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"
                                        ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCity"
                                        ErrorMessage="Please,Enter City" ForeColor="Red" SetFocusOnError="true" ValidationGroup="m">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label3" runat="server" Text="State" CssClass="form-label" />
                                    <asp:TextBox ID="txtState" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label4" runat="server" Text="Country" CssClass="form-label" />
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label5" runat="server" Text="Fax No." CssClass="form-label" />
                                    <asp:TextBox ID="txtFaxNo" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label6" runat="server" Text="Phone No." CssClass="form-label" />
                                    <asp:TextBox ID="txtPhn" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label7" runat="server" Text="Currency Name" CssClass="form-label" />
                                    <asp:DropDownList ID="DDcurrency" runat="server" CssClass="form-select">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label8" runat="server" Text="A/C. No." CssClass="form-label" />
                                    <asp:TextBox ID="txtAcc" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label9" runat="server" Text="ADI Code" CssClass="form-label" />
                                    <asp:TextBox ID="TxtAdCode" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label10" runat="server" Text="Swift Code" CssClass="form-label" />
                                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"
                                        required></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCode"
                                        ErrorMessage="Please,Enter Swift Code " ForeColor="Red" SetFocusOnError="true"
                                        ValidationGroup="m">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label11" runat="server" Text="E-Mail" CssClass="form-label" />
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label12" runat="server" Text="Contact Person" CssClass="form-label" />
                                    <asp:TextBox ID="txtMisc" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label13" runat="server" Text="IBAN" CssClass="form-label" />
                                    <asp:TextBox ID="txtiban" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label14" runat="server" Text="BIC" CssClass="form-label" />
                                    <asp:TextBox ID="txtbic" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label15" runat="server" Text="IFSC CODE" CssClass="form-label" />
                                    <asp:TextBox ID="txtifscode" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label16" runat="server" Text="AD CODE" CssClass="form-label" />
                                    <asp:TextBox ID="txtacode" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label17" runat="server" Text="BRANCH CODE" CssClass="form-label" />
                                    <asp:TextBox ID="txtbranchcode" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label18" runat="server" Text="MICR CODE" CssClass="form-label" />
                                    <asp:TextBox ID="txtmicrcode" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label19" runat="server" Text="A/C Type" CssClass="form-label" />
                                    <asp:DropDownList ID="DDActype" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="1">CURRENT</asp:ListItem>
                                        <asp:ListItem Value="2">SAVING</asp:ListItem>
                                        <asp:ListItem Value="3">DRAWBACK</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label21" runat="server" Text=" Account Name" CssClass="form-label" />
                                    <asp:TextBox ID="txtaccountname" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-4 col-6">
                                <div class="mb-3">
                                    <asp:Label ID="Label22" runat="server" Text=" Post Code" CssClass="form-label" />
                                    <asp:TextBox ID="txtPostcode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                        </div>





                        <div class="card mb-3">
                            <div class="card-header">
                                <asp:Label ID="Label20" runat="server" Text="Categories" CssClass="form-label" />
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-xl-12 mb-3 li-colums-3">
                                        <asp:CheckBoxList ID="chkBankCategory" runat="server" RepeatLayout="UnorderedList">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                        </div>





                        <div class="row mt-3">
                            <div class="col-xl-12 mb-5 text-center">
                                <asp:Button ID="BtnNew" runat="server" CssClass="btn btn-lg btn-dark" Text="New"
                                    OnClientClick="NewForm();" />
                                <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-lg btn-success" OnClick="BtnSave_Click"
                                    Text="Save" ValidationGroup="m" OnClientClick="return validate();"></asp:Button>






                                <asp:Button ID="BtnPreview" runat="server" CssClass="btn btn-lg btn-info" Text="Preview"
                                    OnClick="BtnPreview_Click" />
                                <asp:Button ID="BtnClose" OnClientClick="CloseForm();" CssClass="btn btn-lg btn-danger"
                                    runat="server"
                                    Text="Close" />
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errormsg"
                                    ShowMessageBox="True" ShowSummary="False" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-xl-12">
                                <div class="table-responsive">
                                    <asp:Label ID="lbblerr" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:GridView ID="dgBank" DataKeyNames="BankId" runat="server" EmptyDataText="No Data Found!"
                                        OnRowDataBound="dgBank_RowDataBound" OnSelectedIndexChanged="dgBank_SelectedIndexChanged"
                                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover"
                                        OnRowCreated="dgBank_RowCreated"
                                        OnRowDeleting="dgBank_RowDeleting">
                                        <Columns>
                                            <asp:BoundField DataField="BankId" HeaderText="Sr.No." />
                                            <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                                            <asp:BoundField DataField="Street" HeaderText="Street" />
                                            <asp:BoundField DataField="City" HeaderText="City" />
                                            <asp:BoundField DataField="State" HeaderText="State" />
                                            <asp:BoundField DataField="Country" HeaderText="Country" />
                                            <asp:BoundField DataField="ACNo" HeaderText="AC No" />
                                            <asp:BoundField DataField="SwiftCode" HeaderText="Swift Code" />
                                            <asp:BoundField DataField="PhoneNo" HeaderText="Phone No" />
                                            <asp:BoundField DataField="FaxNo" HeaderText="Fax No" />
                                            <asp:BoundField DataField="Email" HeaderText="Email" />
                                            <asp:BoundField DataField="ContectPerson" HeaderText="Contact Person" />
                                            <asp:BoundField DataField="ADICode" HeaderText="ADI Code" />
                                            <asp:BoundField DataField="iban" Visible="false" HeaderText="Iban" />
                                            <asp:BoundField DataField="Bic" HeaderText="Bic" />
                                            <asp:BoundField DataField="Ifscode" HeaderText="IfSC Code" />
                                            <asp:BoundField DataField="Adcode" HeaderText="Adcode" />
                                            <asp:BoundField DataField="Branchcode" HeaderText="Branch Code" />
                                            <asp:BoundField DataField="Micrcode" HeaderText="MICR Code" />
                                            <asp:BoundField DataField="currencyname" HeaderText="Currency Name" />
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="btn btn-danger btn-xs mbs" ID="LinkButton1" runat="server"
                                                        CausesValidation="False" CommandName="Delete"
                                                        Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"><i class="fa fa-trash-o"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>



                    </div>

                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>




<asp:Content ID="Content5" ContentPlaceHolderID="scriptSection" runat="server">

    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmBank1.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function validate() {

            if (Page_ClientValidate())
                return confirm('Do you Want to Save Data ?');
        }
    </script>
</asp:Content>
