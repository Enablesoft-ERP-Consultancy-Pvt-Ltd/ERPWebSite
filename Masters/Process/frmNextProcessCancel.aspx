<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmNextProcessCancel.aspx.cs"
    Inherits="Masters_Process_frmNextProcessCancel" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "frmNextProcessCancel.aspx";
        }
        function Preview() {
            window.open('../../reportViewer1.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Validate() {
            if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                alert('Plz Select CompanyName....');
                document.getElementById('CPH_Form_DDCompanyName').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                alert('Plz select ProcessName....');
                document.getElementById('CPH_Form_DDProcessName').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDEmployeeName').options.length == 0) {
                alert('Employee must a vlaue....');
                document.getElementById('CPH_Form_DDEmployeeName').focus();
                return false;
            }
            else if (document.getElementById('CPH_Form_DDEmployeeName').options[document.getElementById('CPH_Form_DDEmployeeName').selectedIndex].value == 0) {
                alert('Plz select EmployeeName....');
                document.getElementById('CPH_Form_DDEmployeeName').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDPOrderNo').options[document.getElementById('CPH_Form_DDPOrderNo').selectedIndex].value == 0) {
                alert('Plz select PO Number....');
                document.getElementById('CPH_Form_DDPOrderNo').focus();
                return false;
            }
            return confirm('Do you Want cancel this Stock No..');
        }
    </script>
    <div id="maindiv" style="height: 600px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="70%">
                    <tr>
                        <td>
                            <span class="labelbold">CompanyName</span>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server"
                                AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <span class="labelbold">ProcessName</span>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged1">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDProcessName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <span class="labelbold">Emp. Name</span>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDEmployeeName" Width="150px" runat="server"
                                OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDEmployeeName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <span class="labelbold">P.O OrderNo</span>
                            <asp:DropDownList CssClass="dropdown" ID="DDPOrderNo" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDPOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDPOrderNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <span class="labelbold">Enter StockNo</span>
                            <br />
                            <asp:TextBox ID="TxtStockno" runat="server" CssClass="textb" OnTextChanged="TxtStockno_TextChanged"
                                AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="right">
                            <asp:Button ID="BtnCancel" runat="server" Text="Cancel No." OnClientClick="return Validate();"
                                CssClass="buttonnorm" OnClick="BtnCancel_Click" />
                            &nbsp;<asp:Button ID="BtnNew" runat="Server" Text="New" CssClass="buttonnorm" OnClientClick="return reloadPage();" />
                            &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="true" ForeColor="RED"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div style="width: 284px; height: 250px; overflow: auto; margin-left: 150px">
                                <asp:GridView ID="DGStockNo" runat="server" AutoGenerateColumns="False" DataKeyNames="Issue_Detail_Id"
                                    CssClass="grid-view" Width="265px">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="TStockNo" HeaderText="Stock No">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
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
