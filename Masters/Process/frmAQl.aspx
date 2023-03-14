<%@ Page Title="ACCEPTENCE QUALITY LEVEL (AQL)" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmAQl.aspx.cs" Inherits="Masters_Process_frmAQl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmaql.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function validator() {
            var Message = "";
            var selectedindex = $("#<%=DDCompany.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select Company Name!!\n";
            }
            selectedindex = $("#<%=DDUnit.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please Select Unit. !!\n";
            }
            selectedindex = $("#<%=DDJobname.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please Select Job Name !!\n";
            }
            selectedindex = $("#<%=DDAql.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please Select AQL !!\n";
            }
            var txtempcode = document.getElementById('<%=txtempcode.ClientID %>');
            if (txtempcode.value == "") {
                Message = Message + "Please Enter Emp. Code !!\n";
            }
            var txttotalpcs = document.getElementById('<%=txttotalpcs.ClientID %>');
            if (txttotalpcs.value == "") {
                Message = Message + "Please Enter Total Pcs !!\n";
            }
            if (Message != "") {
                alert(Message);
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btngetstockno.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDCompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDUnit.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Unit. !!\n";
                    }
                    selectedindex = $("#<%=DDJobname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Job Name !!\n";
                    }

                    var txttotalpcs = document.getElementById('<%=txttotalpcs.ClientID %>');
                    if (txttotalpcs.value == "") {
                        Message = Message + "Please Enter Total Pcs !!\n";
                    }
                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
                
            </script>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%" valign="top">
                        <table border="1" cellspacing="2" style="width: 100%">
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label Text="Company Name" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDCompany" CssClass="dropdown" Width="90%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label1" Text="Unit Name" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDUnit" CssClass="dropdown" Width="90%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label8" Text="Job Name" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDJobname" CssClass="dropdown" Width="90%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label2" Text="Item Name" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDItemname" CssClass="dropdown" Width="90%" AutoPostBack="true"
                                        runat="server" OnSelectedIndexChanged="DDItemname_SelectedIndexChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label3" Text="Quality Name" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDQuality" CssClass="dropdown" Width="90%" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDQuality_SelectedIndexChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label4" Text="Design" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDDesign" CssClass="dropdown" Width="90%" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDDesign_SelectedIndexChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label5" Text="Color" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDColor" CssClass="dropdown" Width="90%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label6" Text="Shape" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDShape" CssClass="dropdown" Width="90%" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDShape_SelectedIndexChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label7" Text="Size" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDSize" CssClass="dropdown" Width="90%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label9" Text="AQL" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDAql" CssClass="dropdown" Width="50%" runat="server">
                                        <asp:ListItem>2.5</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label10" Text="AQL Lot No." CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:TextBox ID="txtaqllotno" CssClass="textb" Width="90%" Enabled="false" runat="server"
                                        BackColor="LightGray" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label11" Text="Emp. Code" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <asp:TextBox ID="txtempcode" CssClass="textb" Width="90%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label12" Text="Total No. of Pcs" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txttotalpcs" CssClass="textb" Width="90%" runat="server" onkeypress="return isNumberKey(event);"
                                                    AutoPostBack="true" OnTextChanged="txttotalpcs_TextChanged" />
                                            </td>
                                            <td style="width: 50%">
                                                <asp:Button ID="btngetstockno" Text="Get available Stock no." CssClass="buttonnorm"
                                                    runat="server" OnClick="btngetstockno_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%" valign="top">
                        <fieldset>
                            <legend>
                                <asp:Label CssClass="labelbold" ForeColor="Red" Text="STOCK PCS" runat="server" />
                            </legend>
                            <div style="max-height: 400px; overflow: auto">
                                <asp:GridView ID="DGStock" runat="server" EmptyDataText="No data fetched." CssClass="grid-views"
                                    AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" Text='<%#Container.DataItemIndex+1 %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkitem" Text="" runat="server" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltstockno" Text='<%#Bind("Tstockno") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstockno" Text='<%#Bind("stockno") %>' runat="server" />
                                                <asp:Label ID="lblfromprocessid" Text='<%#Bind("fromprocessid") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%" align="right">
                        <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                        <asp:Button ID="btnsave" Text="AQL PASS/FAIL" runat="server" CssClass="buttonnorm"
                            OnClick="btnsave_Click" UseSubmitBehavior="false" OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';" />
                        <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                    </td>
                    <td style="width: 50%">
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
