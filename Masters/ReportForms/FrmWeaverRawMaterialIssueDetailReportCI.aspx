<%@ Page Title="WEAVER RAW MATERIAL ISSUE REPORT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmWeaverRawMaterialIssueDetailReportCI.aspx.cs" Inherits="Masters_ReportForms_FrmWeaverRawMaterialIssueDetailReportCI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table>
                    <tr id="TRDDCompany" runat="server">
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRDDCustName" runat="server">
                        <td>
                            <asp:Label ID="lblChallanNo" runat="server" CssClass="labelbold" Text="ChallanNo"></asp:Label>
                        </td>
                        <td colspan="3" class="style2">
                           <asp:TextBox ID="txtChallanNo" CssClass="textb" Width="80px" runat="server"  />
                        </td>
                    </tr>                   
                    <tr>
                        <td colspan="4" align="right">
                            &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                OnClick="BtnPreview_Click" />
                            &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="CLOSE"
                                OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblMessage" runat="server" CssClass="labelnormalMM" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            
        </ContentTemplate>
       <%-- <Triggers>
            <asp:PostBackTrigger ControlID="btnexport" />
            <asp:AsyncPostBackTrigger ControlID="GVDetails" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
