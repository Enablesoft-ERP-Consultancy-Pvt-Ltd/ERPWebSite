<%@ Page Title="NewUser" Language="C#" AutoEventWireup="true" CodeFile="NewUser.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="NewUser" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="Scripts/JScript.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript"></script>
    <div id="divNewUser" runat="server" style="height: 450px; width: 100%; background-attachment: fixed">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="margin-left: 25%;">
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label Text="User Name" runat="server" ID="lbl" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtUser" CssClass="textb" runat="server" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqUser" runat="server" ControlToValidate="txtUser"
                                CssClass="errormsg" ErrorMessage="Please, Enter User Name!">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label Text=" Designation" runat="server" ID="Label1" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesignation" CssClass="textb" runat="server" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqDesignation" runat="server" ControlToValidate="txtDesignation"
                                CssClass="errormsg" ErrorMessage="Please , Enter Designation!">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label Text="  Login Name" runat="server" ID="Label2" CssClass="labelbold" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlogin" CssClass="textb" runat="server" Width="150px" AutoPostBack="True"
                                OnTextChanged="txtlogin_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtlogin"
                                CssClass="errormsg" ErrorMessage="Please , Enter login Name!">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <%--<td colspan="2">
                            <asp:Label ID="LblLogin" runat="server" ForeColor="Red" Visible="false" Text="Login Name is not available....."></asp:Label>
                        </td>--%>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label Text="  Password" runat="server" ID="Label3" CssClass="labelbold" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="textb" TextMode="Password"
                                Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                                CssClass="errormsg" ErrorMessage="Please , Enter Password!">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label Text="   DeptId" runat="server" ID="Label4" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtdepartment" CssClass="textb" runat="server" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label Text="User Type" runat="server" ID="Label5" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddUserType" runat="server" Width="158px" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddUserType"
                                CssClass="errormsg" ErrorMessage="Please , Select User Type!">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:RegularExpressionValidator ID="regexpName" Display="None" runat="server" ErrorMessage="Department Id should be numeric and Between 1 to 40"
                                Font-Bold="true" ForeColor="Red" ControlToValidate="txtdepartment" ValidationExpression="^[0-9]{1,40}$" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                         <asp:CheckBox Text="Production Consumption" Visible="false" Checked="false" ID="chkprodcons" CssClass="labelbold" runat="server" />
                        </td>
                          <td>
                            <asp:CheckBox Text="Development Consumption" Visible="false" Checked="false" ID="chkdevcons" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox Text="Can Edit" Checked="true" ID="chkcanedit" CssClass="labelbold" runat="server" />
                        </td>
                         <td>
                            <asp:CheckBox Text="Advance User" Visible="false" Checked="true" ID="chkbackentry" CssClass="labelbold" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Label ID="lblErr" runat="server" CssClass="errormsg"></asp:Label>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnSave_Click" />
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" HeaderText="Following error occurs:"
                                ShowMessageBox="true" ShowSummary="false" />
                            &nbsp;<asp:Button ID="Btnew" runat="server" Text="New" CssClass="buttonnorm" OnClick="Btnew_Click"
                                ValidationGroup="a" />
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnlog" runat="server" Text="Goto Login Page&lt;&lt;" CssClass="buttonnorm"
                                OnClick="btnlog_Click" Visible="false"  />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div style="width: 500px; height: 300px; overflow: scroll">
                                <asp:GridView ID="DG" runat="server" AutoGenerateColumns="False" DataKeyNames="SrNo"
                                    Width="100%" OnRowDataBound="DG_RowDataBound" OnRowDeleting="DG_RowDeleting"
                                    OnSelectedIndexChanged="DG_SelectedIndexChanged" CssClass="grid-views" AutoGenerateSelectButton="true">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="UserName" HeaderText="UserName">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Designation" HeaderText="Designation">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LoginName" HeaderText="LoginName">
                                            <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PassWord" HeaderText="PassWord" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Del" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
