<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddItemCategory.aspx.cs"
    Inherits="Masters_Carpet_AddItemCategory" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {

            if (window.opener.document.getElementById('CPH_Form_refreshcategory')) {
                window.opener.document.getElementById('CPH_Form_refreshcategory').click();
                self.close();
            }
            else if (window.opener.document.getElementById('refreshcategory')) {
                window.opener.document.getElementById('refreshcategory').click();
                self.close();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategoryname" runat="server" Text="CategoryName" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtcatagory" runat="server" Width="176px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" id="tdcode" runat="server">
                            <asp:Label ID="Label1" runat="server" Text="Code" CssClass="labelbold"></asp:Label>
                        </td>
                        <td id="txtcode" runat="server">
                            <asp:TextBox CssClass="textb" ID="txtode" runat="server" Width="72px"></asp:TextBox>
                        </td>
                        <td id="tdHSCode" runat="server" class="tdstyle">
                            <asp:Label ID="LblHSCode" runat="server" Text="H.S.Code"></asp:Label>
                        </td>
                        <td id="tdtxtHSCode" runat="server">
                            <asp:TextBox CssClass="textb" ID="TxtHSCode" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="chk_1" Text="" runat="server" />
                                        <asp:Label ID="lblqualityname1" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="chk_2" Text="" runat="server" />
                                        <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="chk_3" Text="" runat="server" />
                                        <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="chk_4" Text="" runat="server" />
                                        <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="chk_5" Text="" runat="server" />
                                        <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="chk_6" runat="server" Text="" />
                                        <asp:Label ID="lblshqadename" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="ChkBoxList" runat="server" CssClass="checkboxbold">
                                <asp:ListItem Value="0">Finished Item</asp:ListItem>
                                <asp:ListItem Value="1">RawMaterial</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                        <td>
                            <asp:CheckBox ID="ChkPoufTypeCategory" Text="Pouf Type Category" runat="server" />
                            <br />
                            <br />
                            <asp:Label ID="Label2" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="Lblerrer" runat="server" ForeColor="Red" Text="" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2" style="text-align: right">
                            <asp:Button ID="btndelete" runat="server" CssClass="buttonnorm" OnClick="btndelete_Click"
                                OnClientClick="return confirm('Do you want to Delete data?')" Text="Delete" Visible="False" />
                            <asp:Button ID="btnSave" runat="server" CssClass="buttonnorm" OnClick="btnSave_Click"
                                OnClientClick="return confirm('Do you want to save data?')" Text="Save" Width="56px" />
                            <asp:Button ID="btnClear" runat="server" CssClass="buttonnorm" OnClick="btnClear_Click"
                                Text="New" Width="56px" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return  CloseForm();"
                                Text="Close" Width="48px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="width: 100%; height: 200px; overflow: scroll">
                                <asp:GridView ID="gditemcatagory" runat="server" DataKeyNames="Sr_No" OnRowDataBound="gditemcatagory_RowDataBound"
                                    OnSelectedIndexChanged="gditemcatagory_SelectedIndexChanged" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="font-family: Times New Roman; font-size: 18px">
                            <asp:Label ID="lblMessage" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
