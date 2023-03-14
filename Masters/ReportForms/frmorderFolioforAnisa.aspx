<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmorderFolioforAnisa.aspx.cs"
    Inherits="Masters_ReportForms_frmorderFolioforAnisa" MasterPageFile="~/ERPmaster.master"
    Title="ORDER FOLIO" %>

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
            //            if (document.getElementById('CPH_Form_DDjob').selectedIndex <= "0") {
            //                alert('Plz Select Job...')
            //                document.getElementById('CPH_Form_DDjob').focus()
            //                return false;
            //            }
            if (document.getElementById('CPH_Form_txtFolioNo').value == "") {
                alert('Plz Enter Folio No...')
                document.getElementById('CPH_Form_txtFolioNo').focus()
                return false;
            }

        }
    </script>
    <asp:UpdatePanel ID="updatpanel1" runat="server">
        <ContentTemplate>
            <div style="width: 1000px; height: 1000px;">
                <div style="width: 900px; height: 1000px">
                    <div style="width: 391px; margin-left: 300px; height: 146px; margin-top: 20px">
                        <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; text-align: center;
                            background-color: #DEB887;" Width="320px">
                            <table style="width: 305px; height: 138px;">
                                <tr id="Tr1">
                                    <td runat="server" visible="false">
                                        <asp:Label ID="lbljob" runat="server" Text="Job" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:DropDownList ID="DDjob" runat="server" Width="150px" CssClass="dropdown" TabIndex="1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFolioNo" runat="server" Text="Enter Folio No." CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFolioNo" runat="server" Width="150px" CssClass="textb" TabIndex="2"
                                            Height="23px" OnTextChanged="txtFolioNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" Text="Print" Width="50px"
                                            OnClientClick="return validate();" OnClick="btnprint_Click" TabIndex="3" />
                                        <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" Width="50px"
                                            OnClientClick="return CloseForm();" TabIndex="4" />
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 335px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
