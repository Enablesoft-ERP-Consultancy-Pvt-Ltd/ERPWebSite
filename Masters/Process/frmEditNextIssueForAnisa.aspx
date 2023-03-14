<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmEditNextIssueForAnisa.aspx.cs"
    Inherits="Masters_Process_frmEditNextIssueForAnisa" MasterPageFile="~/ERPmaster.master"
    Title="Edit Next Issue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript">
        function EmpSelected(source, eventArgs) {
            document.getElementById('txtgetvalue').value = eventArgs.get_value();
        }
    </script>
    <script type="text/javascript">
        function validate() {

            $("#CPH_Form_btnshowDetail").click(function () {
                
                var Message = "";
                if ($("#CPH_Form_ddUnits")) {
                    var selectedIndex = $("#CPH_Form_ddUnits").attr('selectedIndex');
                    if (selectedIndex < 0) {
                        Message = Message + "Please select Unit Name!!!\n";
                    }
                }
                if ($("#CPH_Form_DDTOProcess")) {
                    var selectedIndex = $("#CPH_Form_DDTOProcess").attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please select Unit Name!!!\n";
                    }
                }
                if ($("#CPH_Form_lstWeaverName").children().length == 0) {
                    Message = Message + "Please Enter ID No!!! \n";
                }
                if (Message == "") {
                    return true;
                }
                else {
                    alert(Message);
                    return false;
                }
            });
        }

    </script>
    <asp:UpdatePanel ID="update" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(validate);
            </script>
            <div>
                <div style="width: 650px; height: 179px; border-style: groove; background-color: #DEB887;
                    margin-left: 50px">
                    <div style="float: left; margin-left: 10px; margin-top: 80px; width: 295px">
                        <table>
                            <tr>
                                <td class="tdstyle">
                                    <span class="labelbold">Job</span>
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDTOProcess" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <span class="labelbold">Enter ID No.</span>
                                    <br />
                                    <asp:TextBox ID="txtWeaverIdNo" runat="server" Width="112px" Height="20px" CssClass="textb"
                                        AutoPostBack="true" OnTextChanged="txtWeaverIdNo_TextChanged"></asp:TextBox>
                                    <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="txtWeaverIdNo_AutoCompleteExtender" runat="server"
                                        BehaviorID="SrchAutoComplete" CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeForJob"
                                        EnableCaching="true" CompletionSetCount="20" OnClientItemSelected="EmpSelected"
                                        ServicePath="~/Autocomplete.asmx" TargetControlID="txtWeaverIdNo" UseContextKey="True"
                                        ContextKey="0#0#0" MinimumPrefixLength="2">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: left; margin-top: 30px; overflow: auto; width: 250px">
                        <table>
                            <tr>
                                <td>
                                    <div style="overflow: auto; width: 200px">
                                        <asp:ListBox ID="lstWeaverName" runat="server" Width="200px" Height="100px" SelectionMode="Multiple">
                                        </asp:ListBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnDeleteName" Text="Delete" CssClass="buttonnorm" runat="server"
                                        OnClick="btnDeleteName_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="margin-left: 50px; width: 280px; height: 50px; display: block;">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnshowDetail" runat="server" Text="Click to Get Detail" Width="150px"
                                    CssClass="buttonnorm" OnClick="btnshowDetail_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblErrorMessage" runat="server" CssClass="labelnormalMM" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <table width="75%">
                    <tr>
                        <td>
                            <div style="max-height: 500px; width: 610px; margin-left: 100px; background-color: Gray;
                                overflow: auto">
                                <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" OnRowDeleting="DGDetail_RowDeleting"
                                    EmptyDataText="No Records found.....">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="Item_Name" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ColorName" HeaderText="ColorName">
                                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Size" HeaderText="Size">
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TStockNo" HeaderText="StockNo">
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcessid" runat="server" Text='<%#Bind("ToProcessid") %>'></asp:Label>
                                                <asp:Label ID="lblStockNo" runat="server" Text='<%#Bind("StockNo") %>'></asp:Label>
                                                <asp:Label ID="lblIssueorderId" runat="server" Text='<%#Bind("issueorderid") %>'></asp:Label>
                                                <asp:Label ID="lblIssueDetailid" runat="server" Text='<%#Bind("IssueDetailid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
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
