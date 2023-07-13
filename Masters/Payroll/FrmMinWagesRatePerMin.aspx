<%@ Page Language="C#" Title="Min Wages Rate Per Min" AutoEventWireup="true" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" CodeFile="FrmMinWagesRatePerMin.aspx.cs"
    Inherits="Masters_Payroll_FrmMinWagesRatePerMin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmMinWagesRatePerMin.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; float: left; background-color: #E3E3E3">
                <div style="float: left; width: 450px; margin: 0% 10% 3% 20%">
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label4" Text="Minimum Wages Rate" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMinWagesRate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label11" Text="Effective Date" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                            </td>
                            <td>
                                <asp:TextBox ID="txtEffectiveDate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtEffectiveDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span style="margin-left: 80px;">
                                    <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click"
                                        OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                        Width="50px" />
                                    &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                        Width="50px" />
                                    &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                        runat="server" Text="Close" />
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="llMessageBox" runat="server" Text="" ForeColor="Red"></asp:Label>
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="height: 250px; overflow: auto; width: 100%;">
                                    <asp:GridView ID="GVMinWagesRate" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        DataKeyNames="MinWagesRateId" OnRowDataBound="GVMinWagesRate_RowDataBound">
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
                                            <asp:TemplateField HeaderText="MinWages Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMinWagesRatePerMin" runat="server" Text='<%#Bind("MinWagesRatePerMin") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Effective Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#Bind("EffectiveDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMinWagesRateId" runat="server" Text='<%# Bind("MinWagesRateId") %>'
                                                        Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--<table width="100%">
                    <tr>
                        <td>
                            
                        </td>
                    </tr>
                </table>--%>
            </div>
            <asp:HiddenField ID="hnMinWagesRateId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
