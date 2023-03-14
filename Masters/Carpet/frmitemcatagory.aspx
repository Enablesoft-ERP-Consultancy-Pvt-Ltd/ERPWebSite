<%@ Page Title="ItemCategory" Language="C#" AutoEventWireup="true" CodeFile="frmitemcatagory.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="frmitemcatagory" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <table width="100%">
        <tr>
        <td>
        <table width="100%">
        <tr>
         <td>
                        <div style="width:100%; height: 600px; overflow: auto">
                            <asp:GridView ID="gditemcatagory" runat="server" DataKeyNames="Sr_No" OnRowDataBound="gditemcatagory_RowDataBound"
                                OnSelectedIndexChanged="gditemcatagory_SelectedIndexChanged">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                            </asp:GridView>
                        </div>
                    </td>
        </tr>
        </table>
        </td>
        <td>
            <table width="100%">
                <tr>
                
                    <td class="tdstyle">
                        <asp:Label ID="lblcategoryname" runat="server" Text="CategoryName" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtcatagory" runat="server" Width="176px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label1" runat="server" Text=" Code" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtode" runat="server" Width="72px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td id="tdHSCode" runat="server" class="tdstyle">
                        <asp:Label ID="LblHSCode" runat="server" Text="H.S.Code" Font-Bold="true"></asp:Label>
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
                                    <asp:Label ID="lblqualityname1" runat="server" Text="Quality" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="tdstyle">
                                    <asp:CheckBox ID="chk_2" Text="" runat="server" />
                                    <asp:Label ID="lbldesignname" runat="server" Text="Design" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="tdstyle">
                                    <asp:CheckBox ID="chk_3" Text="" runat="server" />
                                    <asp:Label ID="lblcolorname" runat="server" Text="Color" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="tdstyle">
                                    <asp:CheckBox ID="chk_4" Text="" runat="server" />
                                    <asp:Label ID="lblshapename" runat="server" Text="Shape" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="tdstyle">
                                    <asp:CheckBox ID="chk_5" Text="" runat="server" />
                                    <asp:Label ID="lblsizename" runat="server" Text="Size" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="tdstyle">
                                    <asp:CheckBox ID="chk_6" runat="server" Text="" />
                                    <asp:Label ID="lblshqadename" runat="server" Text="ShadeColor" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="ChkBoxList" runat="server">
                            <asp:ListItem Value="0">Finished Item</asp:ListItem>
                            <asp:ListItem Value="1">RawMaterial</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkPoufTypeCategory" Text="Pouf Type Category" runat="server" />
                    </td>
                </tr>
                <%--<tr>
            <td></td>
            <td class="tdstyle"> <asp:CheckBox ID="chk_2" Text="" runat="server" />
                <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>
                </td>
            </tr>
            <tr>
            <td></td>
            <td class="tdstyle"> <asp:CheckBox ID="chk_3" Text="" runat="server" />
                <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>
                </td>
            </tr>
            <tr>
            <td></td>
            <td class="tdstyle"> <asp:CheckBox ID="chk_4" Text="" runat="server" />
                <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>
                </td>
            </tr>
            <tr>
            <td></td>
            <td class="tdstyle"> <asp:CheckBox ID="chk_5"  Text="" runat="server" />
                <asp:Label ID="lblsizename" runat="server" Text="Size"></asp:Label>
                </td>
            </tr>--%>
                <%--<tr>
            <td></td>
                <td class="tdstyle">
                    <asp:CheckBox ID="chk_6" runat="server" Text="" />
                    <asp:Label ID="lblshqadename" runat="server" Text="ShadeColor"></asp:Label>
                </td>
            <tr>--%>
                <td colspan="3">
                    <asp:Label ID="Lblerrer" runat="server" ForeColor="Red" Text=""></asp:Label>
                </td>
                <tr>
                
                    <td colspan="2" style="text-align: right">
                        <asp:Button ID="btndelete" runat="server" CssClass="buttonnorm" OnClick="btndelete_Click"
                            OnClientClick="return confirm('Do you want to Delete data?')" Text="Delete" Visible="False" />
                        <asp:Button ID="btnSave" runat="server" CssClass="buttonnorm" OnClick="btnSave_Click"
                            OnClientClick="return confirm('Do you want to save data?')" Text="Save" />
                        <asp:Button ID="btnClear" runat="server" CssClass="buttonnorm" OnClick="btnClear_Click"
                            Text="New" />
                        <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return  CloseForm();"
                            Text="Close" />
                    </td>
                </tr>
                <tr>
                   
                </tr>
                <tr>
                    <td colspan="3" style="font-family: Times New Roman; font-size: 18px">
                        <asp:Label ID="lblMessage" runat="server" Text=" "></asp:Label>
                    </td>
                </tr>
            </table>
            </td>
            </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
