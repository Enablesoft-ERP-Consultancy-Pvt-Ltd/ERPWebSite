<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPurchaseReceiveStockEntry.aspx.cs"
    Title="" MasterPageFile="~/ERPmaster.master" Inherits="Masters_Purchase_FrmPurchaseReceiveStockEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "PurchaseReceive");
        }
        function NewForm() {
            window.location.href = "FrmPurchaseReceiveStockEntry.aspx";
        }
        function validate() {
            if (document.getElementById("<%=DDCompanyName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=DDCompanyName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDPartyName.ClientID %>").value <= "0") {
                alert("Pls Select Vendor Name");
                document.getElementById("<%=DDPartyName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDChallanNo.ClientID %>").value <= "0") {
                alert("Pls Select Order No.");
                document.getElementById("<%=DDChallanNo.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDCompanyName"
                                ErrorMessage="please fill Company......." ForeColor="Red" SetFocusOnError="true"
                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged"
                                AutoPostBack="true" Width="170px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDBranchName" runat="server" class="tdstyle">
                            <asp:Label ID="Label64" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDBranchName" Enabled="false" runat="server" CssClass="dropdown"
                                Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label6" runat="server" Text="VendorName" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDPartyName" runat="server" Width="200px"
                                OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label7" runat="server" Text="Order No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDChallanNo" runat="server" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged"
                                AutoPostBack="True" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label8" runat="server" Text="Challan No." CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="ddlrecchalanno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlrecchalanno_SelectedIndexChanged" Width="150px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="7" align="left">
                            <div style="width: 500px; height: 250px; overflow: auto;">
                                <asp:GridView ID="DGPurchaseReceiveDetail" AutoGenerateColumns="False" OnRowDataBound="DGPurchaseReceiveDetail_RowDataBound"
                                    CellPadding="4" runat="server" DataKeyNames="PurchaseReceiveDetailId" Width="100%">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="PurchaseReceiveDetailId" HeaderText="PurchaseReceiveDetailId"
                                            Visible="false" />
                                        <asp:TemplateField HeaderText="Item Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDGItemDescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDGQty" Text='<%#Bind("Qty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 800px">
                    <tr>
                        <td align="right" colspan="7">
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClientClick="return validate()"
                                OnClick="BtnSave_Click" ValidationGroup="f1" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
