<%@ Page Title="RapierOrderMaster" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="RapierOrderMaster.aspx.cs" Inherits="Masters_Repier_RapierOrderMaster"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"> </script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "RapierOrderMaster.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate_RequiredDate() {
            var required_date = document.getElementById('TxtRequiredDate').Value;
            var assign_date = document.getElementById('TxtAssignDate').value;
            if (assign_date < required_date) {
                alert("Required Date must Be greater than assign Date");
            }
        }
        function ValidateSave() {
            if (document.getElementById('CPH_Form_DDCompanyName') != null) {
                if (document.getElementById('CPH_Form_DDCompanyName').options.length == 0) {
                    alert("Company name must have a value....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                    alert("Please select company name ....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDProcessName') != null) {
                if (document.getElementById('CPH_Form_DDProcessName').options.length == 0) {
                    alert("Process name must have a value....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name ....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDEmployerName') != null) {
                if (document.getElementById('CPH_Form_DDEmployerName').options.length == 0) {
                    alert("Employee name must have a value....!");
                    document.getElementById("CPH_Form_DDEmployerName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDEmployerName').options[document.getElementById('CPH_Form_DDEmployerName').selectedIndex].value == 0) {
                    alert("Please select employee name ....!");
                    document.getElementById("CPH_Form_DDEmployerName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDDescription') != null) {
                if (document.getElementById('CPH_Form_DDDescription').options.length == 0) {
                    alert("Description must have a value....!");
                    document.getElementById("CPH_Form_DDDescription").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDDescription').options[document.getElementById('CPH_Form_DDDescription').selectedIndex].value == 0) {
                    alert("Please select description ....!");
                    document.getElementById("CPH_Form_DDDescription").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TxtQtyRequired').value == "" || document.getElementById('CPH_Form_TxtQtyRequired').value == "0") {
                alert("Pls fill issue qty....!");
                document.getElementById('CPH_Form_TxtQtyRequired').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtRate').value == "" || document.getElementById('CPH_Form_TxtRate').value == "0") {
                alert("Pls fill rate ....!");
                document.getElementById('CPH_Form_TxtRate').focus();
                return false;
            }

            if (document.getElementById('CPH_Form_ChkForEdit').checked == true) {
                if (document.getElementById('CPH_Form_DDChallanNo') != null) {
                    if (document.getElementById('CPH_Form_DDChallanNo').options.length == 0) {
                        alert("Challan no must have a value....!");
                        document.getElementById("CPH_Form_DDChallanNo").focus();
                        return false;
                    }
                    else if (document.getElementById('CPH_Form_DDChallanNo').options[document.getElementById('CPH_Form_DDChallanNo').selectedIndex].value == 0) {
                        alert("Please select challan no ....!");
                        document.getElementById("CPH_Form_DDChallanNo").focus();
                        return false;
                    }
                }
            }
            return confirm('Do you want to save ?')
        }
    </script>
    <div id="maindiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbl" Text=" Company Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label3" Text="Process Name" runat="server" CssClass="labelbold" />
                            <asp:CheckBox ID="ChkForEdit" Font-Bold="True" runat="server" Text=" For Edit" AutoPostBack="True"
                                CssClass="checkboxbold" OnCheckedChanged="ChkForEdit_CheckedChanged" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" Text=" Vendor Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDEmployeeName" Width="150px" runat="server"
                                OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" id="TDDDChallanNo" runat="server" visible="false">
                            <asp:Label ID="Label1" Text=" Challan No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDChallanNo" Width="150px" runat="server"
                                OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" Text=" Challan No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtChallanNo" runat="server" Width="90px" CssClass="textb" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label6" Text=" Assign Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtAssignDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtAssignDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label7" Text=" Required Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRequiredDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtRequiredDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label8" Text="Unit" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDunit" runat="server" Width="100px">
                            </asp:DropDownList>
                        </td>
                        <%--<td>
                            <asp:Label ID="Label9" Text=" Cal Type" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px">
                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                <asp:ListItem Value="4">L-2</asp:ListItem>
                            </asp:DropDownList>
                        </td>--%>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblcategoryname" class="tdstyle" runat="server" Text="Category Name"
                                CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCategoryName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label10" Text="  Description" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDDescription" runat="server" Width="400px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label19" Text="IssueQty" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtQtyRequired" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text="Rate" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <b>
                                <asp:Button ID="BtnNew" runat="Server" Text="New" OnClientClick="return reloadPage();"
                                    CssClass="buttonnorm" />
                                <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return ValidateSave();"
                                    CssClass="buttonnorm" />
                                <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                    Visible="true" CssClass="buttonnorm preview_width" />
                                <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                    CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="7">
                            <div style="width: 100%; max-height: 250px; overflow: auto">
                                <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="DetailID"
                                    OnRowDataBound="DGOrderdetail_RowDataBound" OnRowDeleting="DGOrderdetail_RowDeleting"
                                    CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="Category" HeaderText="Category">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Item" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Rate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label21" Text="Remarks" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRemarks" runat="server" Width="500px" TextMode="MultiLine" Height="30px"
                                CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
