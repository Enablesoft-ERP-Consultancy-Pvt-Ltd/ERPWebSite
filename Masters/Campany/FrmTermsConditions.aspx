<%@ Page Title="Terms & Conditions" Language="C#" AutoEventWireup="true" CodeFile="FrmTermsConditions.aspx.cs"
    Inherits="Masters_Campany_FrmTermsConditions" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "FrmTermsConditions.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function validate() {

            if (Page_ClientValidate())
                return confirm('Do you Want to Save Data ?');
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr align="left">
                    <td>
                        <asp:Label ID="LblItem" runat="server" Text="Format Type" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDFormatType" AutoPostBack="true" runat="server" Width="150px"
                            CssClass="dropdown" OnSelectedIndexChanged="DDFormatType_SelectedIndexChanged"
                            ValidationGroup="m">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text=" Terms & Conditions" Font-Bold="true" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtTermsConditions" runat="server" CssClass="textb" Width="800px"
                            Height="400px"></asp:TextBox>
                        <asp:HtmlEditorExtender ID="HtmlEditorExtender1" runat="server" TargetControlID="txtTermsConditions"
                            EnableSanitization="false">
                        </asp:HtmlEditorExtender>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <tr>
                        <td colspan="2">
                            <span style="margin-left: 300px">
                            <asp:Button ID="BtnNew" runat="server" CssClass="buttonnorm" 
                                OnClientClick="NewForm();" Text="New" Width="70px" />
                            <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" 
                                OnClick="BtnSave_Click" OnClientClick="return validate();" Text="Save" 
                                ValidationGroup="m" Width="70px" />
                            <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" 
                                OnClientClick="CloseForm();" Text="Close" Width="70px" />
                            </span>
                        </td>
                    </tr>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lbblerr" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="width: 100%; max-height: 300px; overflow: auto; margin-left: 200px">
                            <asp:GridView ID="dgTermsConditions" DataKeyNames="TCID" runat="server" EmptyDataText="No Data Found!"
                                OnRowDataBound="dgTermsConditions_RowDataBound" OnSelectedIndexChanged="dgTermsConditions_SelectedIndexChanged"
                                AutoGenerateColumns="False" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FormatType">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFormatType" runat="server" Text='<%#Bind("FormatName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Terms Conditions">
                                        <ItemTemplate>
                                            <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                <asp:Label ID="lblTermsConditions" runat="server" ToolTip='<%#Bind("ToolTipTermsConditions") %>'
                                                    Text='<%# Eval("TermsConditions").ToString().Length>100 ? (Eval("TermsConditions") as string).Substring(0,100) : Eval("TermsConditions")  %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="500px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate> 
                                            <asp:Label ID="lblTCID" runat="server" Text='<%# Bind("TCID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblTCFID" runat="server" Text='<%# Bind("TCFID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                  
                                    <%--  <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>--%>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                        <asp:HiddenField ID="hnid" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
