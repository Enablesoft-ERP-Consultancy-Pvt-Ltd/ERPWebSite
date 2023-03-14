<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmVendorCapacity.aspx.cs" Inherits="FrmVendorCapacity" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmVendorCapacity.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            if (document.getElementById('CPH_Form_DDVendor').selectedIndex <= "0") {
                alert('Plz Select VendorName...')
                document.getElementById('CPH_Form_DDVendor').focus()
                return false;
            }
            if (document.getElementById("<%=DDMonth.ClientID %>").value <= "0") {
                if (document.getElementById("<%=DDMonth.ClientID %>").value == "" || document.getElementById("<%=DDMonth.ClientID %>").value == "0") {
                    alert("Please select Month");
                    document.getElementById("<%=DDMonth.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=DDyear.ClientID %>").value <= "0") {
                if (document.getElementById("<%=DDyear.ClientID %>").value == "" || document.getElementById("<%=DDyear.ClientID %>").value == "0") {
                    alert("Please select Year");
                    document.getElementById("<%=DDyear.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtcapacity.ClientID %>").value <= "0") {
                if (document.getElementById("<%=txtcapacity.ClientID %>").value == "" || document.getElementById("<%=txtcapacity.ClientID %>").value == "0") {
                    alert("Please enter the Capacity");
                    document.getElementById("<%=txtcapacity.ClientID %>").focus();
                    return false;
                }
            }

        }

        function isnumeric(evt) {
            var charcode = (evt.which) ? evt.which : evt.keycode
            if (charcode > 31 && (charcode < 48 || charcode > 57)) {
                alert('Plz Enter Numeric Value Only')
                return false
            }
            else {
                return true
            }
        }
    </script>
    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="width: 400px; margin-left: 220px; height: 300px; margin-top: 20px">
                <asp:Panel runat="server" ID="pp" Style="border: 1px groove Teal; background-color: #D3D3D3"
                    Width="370px" Height="159px">
                    <div style="padding-left: 5px">
                        <table style="width: 335px; height: 168px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblVendor" runat="server" Text="VendorName" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDVendor" runat="server" Width="150px" CssClass="dropdown"
                                        OnSelectedIndexChanged="DDVendor_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDate" runat="server" Text="Month" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDMonth" runat="server" CssClass="dropdown" Width="100px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblyear" runat="server" Text="Year" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDyear" runat="server" CssClass="dropdown" Width="100px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcapacity" runat="server" Text="Capacity" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcapacity" runat="server" Width="142px" CssClass="textb" Height="20px"
                                        onkeypress="return isnumeric(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="4">
                                    <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" Width="50px"
                                        OnClientClick="return validate()" OnClick="btnsave_Click" />
                                    <asp:Button ID="btnnew" runat="server" CssClass="buttonnorm" Text="New" Width="50px"
                                        OnClientClick="return NewForm()" />
                                    <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" Width="50px"
                                        OnClientClick="return CloseForm()" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="width: 370px; height: 250px; overflow: auto" align="left">
                            <asp:GridView ID="GDview" runat="server" OnRowDataBound="GDview_RowDataBound" OnPageIndexChanging="GDview_PageIndexChanging"
                                OnSelectedIndexChanged="GDview_SelectedIndexChanged" DataKeyNames="Sr_No" AllowPaging="true"
                                AutoGenerateSelectButton="True" OnRowDeleting="GDview_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" Text="Delete"
                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <%-- <Columns>
                                   <asp:BoundField HeaderText="VendorName" DataField="EmpName"></asp:BoundField>
                                   <asp:BoundField HeaderText="Month" DataField="Month_Name"></asp:BoundField>
                                   <asp:BoundField HeaderText="Year" DataField="Year"></asp:BoundField>
                                   <asp:BoundField HeaderText="Capacity" DataField="Capacity"></asp:BoundField>

                                </Columns>--%>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
