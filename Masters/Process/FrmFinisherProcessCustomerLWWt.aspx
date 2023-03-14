<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" CodeFile="FrmFinisherProcessCustomerLWWt.aspx.cs"
    Inherits="Masters_Process_FrmFinisherProcessCustomerLWWt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open("../../ReportViewer.aspx", "FrmFinisherProcessCustomerLWWt");
        }      
    </script>
    <div>
        <table>
            <tr>
                <td class="tdstyle">
                    Customer Code<br />
                    <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" Width="250px" runat="server"
                        AutoPostBack="true" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCustomerCode"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    Process Name
                    <br />
                    <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="250px" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDProcessName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="buttonnorm" />
                    <asp:Button ID="BtnClose" runat="server" Text="Close" OnClick="BtnClose_Click" CssClass="buttonnorm" />
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:GridView ID="DGDetail" AutoGenerateColumns="False" OnRowDataBound="DGDetail_RowDataBound"
                        runat="server" DataKeyNames="ID" CssClass="grid-view" OnRowDeleting="DGDetail_RowDeleting">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <PagerStyle CssClass="PagerStyle" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                            <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                            <asp:BoundField DataField="ProcessName" HeaderText="Process Name" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="Del" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
