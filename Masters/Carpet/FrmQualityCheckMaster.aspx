<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmQualityCheckMaster.aspx.cs"
    EnableEventValidation="false" Inherits="Masters_Carpet_FrmQualityCheckMaster"
    MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<asp:Content ID="CPH" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function preview() {
            document.getElementById('<%=BtnPreview.ClientID %>').click();
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function AddNewParameter() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var varCat = document.getElementById('<%=ddCategoryName.ClientID %>').value;
                var varProc = document.getElementById('<%=ddProcessName.ClientID %>').value;
                window.open('FrmQCParaMeterName.aspx?Category=' + varCat + '&Proc=' + varProc + '', '', 'width=701px,Height=501px');
            }
        }
        function validate() {
            var dd1 = document.getElementById('<%=ddCategoryName.ClientID %>');
            var dd2 = document.getElementById('<%=ddProcessName.ClientID %>');
            var dd3 = document.getElementById('<%=ddItemName.ClientID %>');
            if (dd1.selectedIndex == 0) {
                alert("Pls Select Category Name");
                return false;
            }
            else if (dd2.selectedIndex == 0) {
                alert("Pls Select Process Name");
                return false;
            }
            else if (dd3.selectedIndex == 0) {
                alert("Pls Select Item Name");
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;

                    }
                    else {
                        inputlist[i].checked = false;


                    }
                }
            }

        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%">
                <div style="width: 40%; float: left">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label ID="LblCategory" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddCategoryName" runat="server" OnSelectedIndexChanged="ddcategoryname_SelectedIndexChanged"
                                    Width="150px" AutoPostBack="True" TabIndex="2" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblProcess" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddProcessName" runat="server" Width="150px" AutoPostBack="True"
                                    TabIndex="2" CssClass="dropdown" OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblItemName" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddItemName" runat="server" Width="150px" AutoPostBack="True"
                                    TabIndex="2" CssClass="dropdown" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblQuality" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="dquality" runat="server" Width="150px" TabIndex="2" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button CssClass="buttonnorm" ID="BtnAddParameterReferce" runat="server" Text=""
                                    Style="display: none" OnClick="BtnAddParameterReferce_Click" />
                            </td>
                            <td>
                                <asp:Button ID="BtnAddNewParameter" runat="server" Text="Add New Parameter" CssClass="buttonnorm"
                                    OnClientClick="return AddNewParameter();" Visible="false" Width="150px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 60%; float: right">
                    <table>
                        <tr>
                            <td>
                                <div style="height: 150px; width: 100%; overflow: scroll;">
                                    <asp:GridView ID="DGShowData" runat="server" AutoGenerateColumns="False" DataKeyNames="Sr_No"
                                        CssClass="grid-views">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="Chkall" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkbox" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle Width="30px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr No">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ParameterType" HeaderText="Parameter Type">
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ParameterName" HeaderText="Parameter Name">
                                                <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ParameterName" HeaderText="ParameterName">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ShortName" HeaderText="ShortName">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Specification" HeaderText="Specification">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Method" HeaderText="Method">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="clear: both">
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                OnClientClick="return validate()" TabIndex="62" />
                            <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Visible="false"
                                Text="Preview" OnClientClick="return preview();" />
                            <asp:Button ID="BtncClose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td colspan="3">
                        <div style="height: 200px; overflow: scroll; width: 100%;">
                            <asp:GridView ID="DG" runat="server" AutoGenerateColumns="False" DataKeyNames="Sr_No"
                                OnRowDataBound="DG_RowDataBound" OnRowDeleting="DG_RowDeleting" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr No">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ParameterType" HeaderText="Parameter Type">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ParameterName" HeaderText="ParameterName">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShortName" HeaderText="ShortName">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Specification" HeaderText="Specification">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Method" HeaderText="Method">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="Delete" OnClientClick="return confirm('Do You Want To Delete Data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enable/Disable">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkQcParameter_ED" Text='<%#Bind("Status") %>' runat="server"
                                                OnClick="lnkQcParameter_ED" OnClientClick="return confirm('Do you want to Enable_Disable QC Parameter')" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQcParameterenable_disable" Text='<%#Bind("Enable_Disable") %>'
                                                runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
