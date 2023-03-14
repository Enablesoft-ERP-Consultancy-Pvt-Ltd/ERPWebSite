<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmOrderDelete.aspx.cs" Inherits="Masters_Order_frmOrderDelete"
    MasterPageFile="~/ERPmaster.master" Title="DELETE ORDER" %>

<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validate() {
            $("#CPH_Form_btndel").click(function () {
                var message = "";
                if ($("#CPH_Form_ddcustomercode")) {
                    var selectindex = $("#CPH_Form_ddcustomercode").attr('selectedIndex');
                    if (selectindex <= 0) {
                        message = message + "Please,select customer code!!!\n";
                    }
                }
                if ($("#CPH_Form_txtorderNo").val() == "") {
                    message = message + "Please enter customer order No.!!!\n";
                }
                if (message == "") {
                    return true;
                }
                else {
                    alert(message);
                    return false;
                }
            });

        }

    </script>
    <div>
        <asp:UpdatePanel ID="updat1" runat="server">
            <ContentTemplate>
                <script type="text/javascript" language="javascript">
                    Sys.Application.add_load(validate);
                </script>
                <div style="width: 50%; margin: auto">
                    <table style="width: 100%; height: 200px">
                        <tr>
                            <td>
                                <span class="labelbold">CompanyName</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddcompany" runat="server" CssClass="dropdown" Width="160px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="labelbold">Customer Code</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddcustomercode" runat="server" CssClass="dropdown" Width="160px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="labelbold">Order No.</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtorderNo" CssClass="textb" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btndel" Text="Delete Order" Style="margin-left: 60px" runat="server"
                                    CssClass="buttonnorm" OnClick="btndel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label Text="" ID="lblmsg" runat="server" CssClass="lblcss" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <div style="height: 250px; overflow: auto">
                                    <asp:GridView runat="server" ID="gvorders" AutoGenerateColumns="false" EmptyDataText="No Records Found.....">
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <HeaderStyle CssClass="gvheaders" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderId" Text='<%#Bind("orderid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CustomerCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcustomer" Text='<%#Bind("Customercode") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderNo" Text='<%#Bind("OrderNo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderDate" Text='<%#Bind("orderdate") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDel" runat="server" CausesValidation="False" CommandName="Delete"
                                                        Text="Delete" OnClientClick="return doConfirm();"></asp:LinkButton>
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
</asp:Content>
