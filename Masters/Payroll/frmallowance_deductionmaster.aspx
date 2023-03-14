<%@ Page Title="Allowances/Deductions" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmallowance_deductionmaster.aspx.cs" Inherits="Payroll_frmallowance_deductionmaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">

        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDtype.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Type !!\n";
                    }

                    var txtparametername = document.getElementById('<%=txtparametername.ClientID %>');
                    if (txtparametername.value == "") {
                        Message = Message + "Please Enter Parameter Name !!\n";
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
            <div style="margin: 1% 20% 0% 20%">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbltype" Text="Type" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDtype" CssClass="dropdown" Width="200px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="DDtype_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="--Select--" />
                                <asp:ListItem Value="1" Text="Allowance" />
                                <asp:ListItem Value="2" Text="Deduction" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Text="Parameter Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtparametername" CssClass="textb" Width="200px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" ForeColor="Red" CssClass="labelbold" Font-Size="Small" Text=""
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <div style="max-height: 500px; overflow: auto">
                                            <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records found."
                                                CssClass="grid-views" OnRowCancelingEdit="DGDetail_RowCancelingEdit" OnRowEditing="DGDetail_RowEditing"
                                                OnRowUpdating="DGDetail_RowUpdating">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Parameter Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblparametername" Text='<%#Bind("parametername") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txteditparametername" Text='<%#Bind("parametername") %>' Width="200px"
                                                                runat="server" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField DeleteText="" ShowEditButton="True" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
