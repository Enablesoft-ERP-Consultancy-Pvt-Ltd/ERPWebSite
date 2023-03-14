<%@ Page Title="SubQuality" Language="C#" AutoEventWireup="true" CodeFile="Additemcode.aspx.cs"
    Inherits="Masters_Carpet_itemcode" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            if (window.opener.document.getElementById('btnrefreshsbqlt')) {
                window.opener.document.getElementById('btnrefreshsbqlt').click();
                self.close();
            }
            else if (window.opener.document.getElementById('fillitemcode')) {
                window.opener.document.getElementById('fillitemcode').click();
                self.close();
            }
            else { }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td>
                            <div>
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblcategoryname" runat="server" Text="Category"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCategory" runat="server" Width="150px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged"
                                                AutoPostBack="True" CssClass="dropdown">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCategory"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDItemName" runat="server" AutoPostBack="True" Width="150px"
                                                OnSelectedIndexChanged="DDItemName_SelectedIndexChanged" CssClass="dropdown">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDItemName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdQuality" runat="server" Width="150px" AutoPostBack="True"
                                                OnSelectedIndexChanged="DdQuality_SelectedIndexChanged" CssClass="dropdown">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DdQuality"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            QUANTITY
                                            <asp:Label ID="subquality" runat="server" Text="Label" Visible="False"></asp:Label>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:TextBox ID="TxtQuantity" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            Unit
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDUnit" Width="100px" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td class="style1">
                            <div style="height: 200px; width: 500px; overflow: scroll;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="Gvitemdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="Quality_Id"
                                            CssClass="grid-view" OnRowCreated="Gvitemdetail_RowCreated">
                                            <HeaderStyle CssClass="gvheader" />
                                            <AlternatingRowStyle CssClass="gvalt" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:BoundField DataField="Quality_Id" HeaderText="Sr No" />
                                                <asp:BoundField DataField="item_name" HeaderText="Item Name">
                                                    <ControlStyle Height="17px" />
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SubItemName" HeaderText="SubItem Name" />
                                                <asp:TemplateField HeaderText="Percentage">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtp_tage" runat="server" Width="70px" AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr id="ErrorMessage" runat="server" visible="false">
                        <td colspan="2">
                            <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="true" blink="true" ForeColor="Red"
                                Text=""></asp:Label>
                            <asp:Label ID="qualitycode" runat="server" Text="Label" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtCode" runat="server" Width="300px" ForeColor="Black" Enabled="false"></asp:TextBox>
                            <asp:TextBox ID="TxtFinishedid" runat="server" Height="0px" Width="0px" BorderStyle="None"
                                BorderColor="White" ForeColor="White"></asp:TextBox>
                            <asp:TextBox ID="txtqlt" runat="server" Height="0px" Width="0px" BorderStyle="None"
                                BorderColor="White" ForeColor="White"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="Txtitemcode" runat="server" Width="500px" Height="30px" ForeColor="Black"
                                Enabled="false"></asp:TextBox>
                            <asp:Button ID="TxtCHECK" runat="server" Visible="false" Text="CHECK" Width="55px"
                                Height="23px" CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <div id="divitemcode" runat="server" style="height: 150px; overflow: scroll;">
                                <asp:GridView ID="Gditemcode" runat="server" AutoGenerateColumns="true" Height="175px"
                                    OnSelectedIndexChanged="Gditemcode_SelectedIndexChanged" OnRowDataBound=" Gditemcode_RowDataBound"
                                    DataKeyNames="Sr_No" CssClass="grid-view" OnRowCreated="Gditemcode_RowCreated">
                                    <HeaderStyle CssClass="gvheader" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" />
                            <asp:Button ID="Btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="Btnsave_Click" />
                            <asp:Button ID="Button1" runat="server" Text="Preview" CssClass="buttonnorm" OnClientClick="return priview();" />
                            <asp:Button ID="Btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
