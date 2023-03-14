<%@ Page Title="Calc Options" Language="C#" AutoEventWireup="true" CodeFile="CalcOptions.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Carpet_CalcOptions" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function CloseForm() {

            window.location.href = "../../main.aspx";
        }

        function addpriview() {
            window.open("../../ReportViewer.aspx")
        }        
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 15%; margin-right: 15%">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" width="30%" style="text-align: right">
                            <asp:Label ID="lblCalcOption" runat="server" Text="Calc Option" CssClass="labelbold"></asp:Label>
                        </td>
                        <td width="50%" style="text-align: left; padding-left: 10px">
                            <asp:TextBox CssClass="textb" ID="txtCalcOption" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="font-family: Times New Roman; font-size: 18px">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="text-align: left">
                            <asp:Button ID="Button1" runat="server" Text="New" CssClass="buttonnorm" OnClick="Button1_Click" />
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" Width="53px"
                                OnClientClick="return confirm('Do you want to save data?')" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClear" Text="Close" runat="server" CssClass="buttonnorm" Width="53px"
                                OnClientClick="return CloseForm();" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            <div style="height: 400px; overflow: auto">
                                <asp:GridView ID="gdCalcOption" runat="server" DataKeyNames="Sr_No" OnRowDataBound="gdCalcOption_RowDataBound"
                                    OnSelectedIndexChanged="gdCalcOption_SelectedIndexChanged" Width="400px">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
