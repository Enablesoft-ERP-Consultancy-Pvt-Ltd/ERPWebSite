<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FRMVENDORMANAGEMENT.aspx.cs" Inherits="Masters_Order_FRMVENDORMANAGEMENT" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmRawMaterialReturned.aspx";
        }

        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 60) {
                alert("Please Enter Only Numeric Value:");
                return false;
            }
            return true;
        }
        function validate() {
            if (document.getElementById('CPH_Form_ddcompany').selectedIndex <= "0") {
                alert('Plz Select Job...')
                document.getElementById('CPH_Form_ddcompany').focus()
                return false;
            }
            if (document.getElementById("<%=ddcustomer.ClientID %>").value <= "0") {
                if (document.getElementById("<%=ddcustomer.ClientID %>").value == "" || document.getElementById("<%=ddcustomer.ClientID %>").value == "0") {
                    alert("Please enter the Customer");
                    document.getElementById("<%=ddcustomer.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddorderno.ClientID %>").value <= "0") {
                if (document.getElementById("<%=ddorderno.ClientID %>").value == "" || document.getElementById("<%=ddorderno.ClientID %>").value == "0") {
                    alert("Please enter the OrderNo");
                    document.getElementById("<%=ddorderno.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddvendor.ClientID %>").value <= "0") {
                if (document.getElementById("<%=ddvendor.ClientID %>").value == "" || document.getElementById("<%=ddvendor.ClientID %>").value == "0") {
                    alert("Please enter the Vendor Name");
                    document.getElementById("<%=ddvendor.ClientID %>").focus();
                    return false;
                }

            }
        }
     
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table width="80%">
                <tr>
                    <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                        <div id="1" style="height: auto" align="left">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div style="width: 70%; margin: auto">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblcompany" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddcompany" runat="server" AutoPostBack="True" Width="200px"
                                                        CssClass="dropdown">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblcustomer" runat="server" Text="Customer" CssClass="labelbold"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddcustomer" runat="server" Width="200px" CssClass="dropdown"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblorder" runat="server" Text="Order No" CssClass="labelbold"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddorderno" runat="server" AutoPostBack="True" Width="200px"
                                                        CssClass="dropdown" OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblallocatedate" runat="server" Text="Fabric Issue Date" CssClass="labelbold"></asp:Label>
                                                    <br />
                                                    <asp:TextBox runat="server" ID="txtallocatedate" Width="120px" CssClass="textb" />
                                                    <asp:CalendarExtender ID="calendardate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtallocatedate">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblvendor" runat="server" Text="Vendor Name" CssClass="labelbold"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddvendor" runat="server" AutoPostBack="True" Width="200px"
                                                        CssClass="dropdown" OnSelectedIndexChanged="ddvendor_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCapacity" runat="server" Text="Capacity" CssClass="labelbold"></asp:Label>
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lablcapacity" runat="server" ForeColor="red" Font-Size="Medium" CssClass="labelbold"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblmsg" runat="server" ForeColor="red" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="60%">
                                            <tr>
                                                <td>
                                                    <div style="width: 600px; height: 150px; overflow: auto" align="center">
                                                        <asp:GridView ID="GDview" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                                            OnRowCommand="GDview_RowCommand" RowHeaderColumn="ItemDescription">
                                                            <HeaderStyle CssClass="gvheaders" />
                                                            <AlternatingRowStyle CssClass="gvalts" />
                                                            <RowStyle CssClass="gvrow" />
                                                            <Columns>
                                                                <asp:BoundField HeaderText="ItemDescription" DataField="ItemDescription"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="Order Quantity">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblorderQty" runat="server" Text='<%#Bind("OrderQty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="item_finished_id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblitem_finished_id" runat="server" Text='<%#Bind("item_finished_id") %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Already Allocation" DataField="AlreadyAllocation" ConvertEmptyStringToNull="False">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                                                    <ItemStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Allocation Qty">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtallocationqty" runat="server" AutoPostBack="true" CssClass="textb"
                                                                            BackColor="Yellow" onkeypress="return isNumberKey(event)" Width="85px"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CMT">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtcmt" runat="server" AutoPostBack="true" CssClass="textb" BackColor="Yellow"
                                                                            onkeypress="return isNumberKey(event)" Width="85px"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ShowHeader="False">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnsave" runat="server" CausesValidation="False" CommandName="Save"
                                                                            Text="Save" CssClass="buttonnorm" OnClientClick="return validate()"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
