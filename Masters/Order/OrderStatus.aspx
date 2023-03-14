<%@ Page Title="ORDER STATUS UPDATE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="OrderStatus.aspx.cs" Inherits="Masters_Order_OrderStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">

        function Validate() {
            if (document.getElementById("<%=ddCatagory.ClientID %>").value < "0") {
                alert("Pls Select Catagory Name");
                document.getElementById("<%=ddCatagory.ClientID %>").focus();
                return false;
            }
            else {
                var isValid = false;
                var gridView = document.getElementById('<%= DGOrderDetail.ClientID %>');
                for (var i = 1; i < gridView.rows.length; i++) {
                    var inputs = gridView.rows[i].getElementsByTagName('input');
                    if (inputs != null) {
                        if (inputs[0].type == "checkbox") {
                            if (inputs[0].checked) {
                                isValid = true;
                                return confirm('Do You Want To Save?')
                            }
                        }
                    }
                }
                alert("Please select atleast one checkbox");
                return false;
            }
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;
                    }
                    else {
                        inputlist[i].checked = false;

                    }
                }
            }

        }
    </script>
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td style="width: 50%" valign="top">
                        <table style="width: 100%" border="1" cellpadding="0" cellspacing="5">
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="lblordertype" Text="Order Type" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList CssClass="dropdown" ID="ddordertype" runat="server" Width="90%"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddordertype_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Order Status</asp:ListItem>
                                        <asp:ListItem Value="1">Purchase Status</asp:ListItem>
                                        <asp:ListItem Value="2">Production Status</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label4" Text="Buyer" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList CssClass="dropdown" ID="DDbuyer" runat="server" Width="90%" AutoPostBack="True"
                                        OnSelectedIndexChanged="DDbuyer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="Tdemp" runat="server" visible="false">
                                <td style="width: 30%">
                                    <asp:Label ID="Label5" Text="Weaver Name" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList CssClass="dropdown" ID="DDweaver" runat="server" AutoPostBack="true"
                                        Width="90%" OnSelectedIndexChanged="DDweaver_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label1" Text="Category" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList CssClass="dropdown" ID="ddCatagory" runat="server" Width="90%"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label2" Text="Check Orders" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList CssClass="dropdown" ID="DDStatus" runat="server" Width="90%" OnSelectedIndexChanged="DDStatus_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label3" Text="Change Status" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList CssClass="dropdown" ID="ddststatuschange" runat="server" Width="90%">
                                        <%--   <asp:ListItem Value="0">Pending</asp:ListItem>
                        <asp:ListItem Value="1">Complete</asp:ListItem>
                        <asp:ListItem Value="2">Cancel</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="right" colspan="2">
                                    <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Change Status" runat="server"
                                        OnClick="BtnSave_Click" OnClientClick="return Validate();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%" valign="top">
                        <table style="width: 100%">
                            <tr id="trdrig" runat="server">
                                <td style="width: 100%">
                                    <div style="width: 100%; max-height: 400px; overflow: auto">
                                        <asp:GridView ID="DGOrderDetail" runat="server" DataKeyNames="orderid" AutoGenerateColumns="False"
                                            EmptyDataText="No records fetched..." CssClass="grid-views" Width="100%">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chkbox" runat="server" />
                                                        <%-- <asp:CheckBox ID="CheckBox1" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="OrderNo" HeaderText="OrderNo">
                                                    <HeaderStyle Width="20%" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Order Remark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemark" runat="server" align="right" TextMode="MultiLine" Width="90%"
                                                            Text='<%# Bind("Remark") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="70%" />
                                                    <ItemStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
