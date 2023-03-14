<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmReportPackingRegister.aspx.cs"
    Inherits="Masters_ReportForms_frmReportPackingRegister" MasterPageFile="~/ERPmaster.master"
    Title="PACKING REGISTER" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            if (document.getElementById('CPH_Form_DDUnit').selectedIndex < "0") {
                alert('Plz Select Unit...')
                document.getElementById('CPH_Form_DDUnit').focus()
                return false;
            }
            if (document.getElementById('CPH_Form_DDArticle').selectedIndex <= "0") {
                alert('Plz Select Article...')
                document.getElementById('CPH_Form_DDArticle').focus()
                return false;
            }

        }
    </script>
    <asp:UpdatePanel ID="updatpanel1" runat="server">
        <ContentTemplate>
            <div style="width: 1000px; height: 1000px;">
                <div style="width: 900px; height: 1000px">
                    <div style="width: 400px; margin-left: 300px; height: 146px; margin-top: 20px">
                        <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; background-color: #DEB887;"
                            Width="400px">
                            <div style="padding-left: 20px">
                                <table style="height: 158px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUnits" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDUnit" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblArticle" runat="server" Text="Articles" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDArticle" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblColorname" runat="server" Text="Colour Name" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDColor" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Shape Name" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDShape" runat="server" Width="150px" CssClass="dropdown" OnSelectedIndexChanged="DDShape_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSize" runat="server" Text="SIZE" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddSize" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromDate" runat="server" Text="From Date" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromdate" runat="server" Width="95px" CssClass="textb" Height="23px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalenderExtendertxtdate" runat="server" TargetControlID="txtFromdate"
                                                Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblTodate" runat="server" Text="To Date" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" runat="server" Width="95px" CssClass="textb" Height="23px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                                Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right" colspan="3">
                                            <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" Text="Print" Width="50px"
                                                OnClientClick="return validate();" OnClick="btnprint_Click" />
                                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" Width="50px"
                                                OnClientClick="return CloseForm();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
