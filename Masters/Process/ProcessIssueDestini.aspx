<%@ Page Title="ProcessIssue" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="ProcessIssueDestini.aspx.cs" Inherits="Masters_Process_ProcessIssueDestini"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "ProcessIssue.aspx";

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
                alert("Required Date Must Be Greater Then Assign Date");
            }
        }
    </script>
    <div id="maindiv" style="height: 490px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            Company Name
                        </td>
                        <td class="tdstyle">
                            Customer Code
                        </td>
                        <td class="tdstyle">
                            Customer Order No.
                        </td>
                        <td class="tdstyle">
                            Process Name
                        </td>
                        <td class="tdstyle">
                            Emp. Name
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerOrderNumber" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerOrderNumber_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDEmployeeName" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Assign Date
                        </td>
                        <td class="tdstyle">
                            Required Date
                        </td>
                        <td class="tdstyle">
                            Unit
                        </td>
                        <td class="tdstyle">
                            Cal Type
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtAssignDate" runat="server" Width="80px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtAssignDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRequiredDate" runat="server" OnTextChanged="TxtRequiredDate_TextChanged"
                                AutoPostBack="true" Width="80px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtRequiredDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDunit" runat="server" Width="100px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px">
                                <asp:ListItem Value="0">Area wise</asp:ListItem>
                                <asp:ListItem Value="1">Unit wise</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblProdCode" class="tdstyle" runat="server" Text="Product Code"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblcategoryname" class="tdstyle" runat="server" Text="Category Name"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            Description
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtProductCode" runat="server" AutoPostBack="True" Width="110px"
                                OnTextChanged="TxtProductCode_TextChanged"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProductCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCategoryName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList CssClass="dropdown" ID="DDDescription" runat="server" Width="90%"
                                AutoPostBack="True" OnSelectedIndexChanged="DDDescription_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <%--<td id="LblArea" runat="server" visible="false">--%>
                        <td class="tdstyle">
                            T.O.Qty
                        </td>
                        <td class="tdstyle">
                            PQty
                        </td>
                        <td class="tdstyle">
                            Width
                        </td>
                        <td class="tdstyle">
                            Length
                        </td>
                        <td id="LblArea" runat="server" visible="false" class="tdstyle">
                            Area
                        </td>
                        <td class="tdstyle">
                            Rate
                        </td>
                        <td class="tdstyle">
                            IssueQty
                        </td>
                    </tr>
                    <tr>
                        <td width="90px">
                            <asp:TextBox ID="TxtTotalQty" runat="server" Width="90px" Enabled="false"></asp:TextBox>
                        </td>
                        <td width="90px">
                            <asp:TextBox ID="TxtPreQuantity" runat="server" Width="90px" Enabled="false"></asp:TextBox>
                        </td>
                        <td width="90px">
                            <asp:TextBox ID="TxtWidth" runat="server" Width="90px" AutoPostBack="True" OnTextChanged="TxtWidth_TextChanged"></asp:TextBox>
                        </td>
                        <td width="90px">
                            <asp:TextBox ID="TxtLength" runat="server" Width="90px" AutoPostBack="True" OnTextChanged="TxtLength_TextChanged"></asp:TextBox>
                        </td>
                        <td id="TdArea" runat="server" visible="false" width="90px">
                            <asp:TextBox ID="TxtArea" runat="server" Width="90px"></asp:TextBox><br />
                        </td>
                        <td width="90px">
                            <asp:TextBox ID="TxtRate" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td width="90px">
                            <asp:TextBox ID="TxtQtyRequired" runat="server" Width="90px" AutoPostBack="True"
                                OnTextChanged="TxtQtyRequired_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="true" ForeColor="RED"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="true" DataKeyNames="Sr_No"
                                OnSelectedIndexChanged="DGOrderdetail_SelectedIndexChanged" OnRowDataBound="DGOrderdetail_RowDataBound"
                                CssClass="grid-view">
                                <HeaderStyle CssClass="gvheader" />
                                <AlternatingRowStyle CssClass="gvalt" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" align="right">
                            <asp:Button ID="BtnNew" runat="Server" Text="New" OnClientClick="return reloadPage();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return confirm('Do you want to save data?')"
                                CssClass="buttonnorm" />
                            <asp:Button ID="BtnUpdate" runat="server" Text="Update" OnClick="BtnUpdate_Click"
                                Visible="False" CssClass="buttonnorm" />
                            <asp:Button ID="BtnPreview" runat="server" Text="Preview" Visible="true" OnClientClick="return Preview();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Instructions
                        </td>
                        <td colspan="7" class="tdstyle">
                            Remarks
                            <asp:TextBox ID="TxtRemarks" runat="server" Width="89.9%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:TextBox ID="TxtInstructions" runat="server" Width="99.5%" Height="50px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
