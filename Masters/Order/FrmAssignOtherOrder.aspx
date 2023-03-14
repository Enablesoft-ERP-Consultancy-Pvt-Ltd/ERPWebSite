<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmAssignOtherOrder.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Title="Order Attach Another Order" Inherits="Masters_Order_FrmAssignOtherOrder"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
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
        function ontextchanged() {
            var intFlag = 0;
            var strErrMsg = "";
            var dtDate = document.getElementById("CPH_Form_TxtOrderDate").value
            var dtDate1 = document.getElementById("CPH_Form_TxtReqDate").value

            var DatedtDate = new Date(dtDate);
            var DatedtDate1 = new Date(dtDate1);

            if (DatedtDate > DatedtDate1) {
                strErrMsg = strErrMsg + "Require Date Cann't Be Shorter Than Order Date \n";
                document.getElementById("CPH_Form_TxtReqDate").value = document.getElementById("CPH_Form_TxtOrderDate").value;
                intFlag++;
            }
            // Display error message if a field is not completed
            if (intFlag != 0) {
                alert(strErrMsg);

                return false;
            }
            else {
                return true;
            }
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function Validate() {
            if (document.getElementById('CPH_Form_DDLOrderNo').options[document.getElementById('CPH_Form_DDLOrderNo').selectedIndex].value == 0) {
                alert("Please select order no....!");
                document.getElementById("CPH_Form_DDLOrderNo").focus();
                return false;
            }
            if (document.getElementById("CPH_Form_TxtStockNo").value == "") {
                alert("Please enter stock no....!");
                document.getElementById("CPH_Form_TxtStockNo").focus();
                return false;
            }
            return confirm('Do You Want To delete these stock nos..?');
        }
    </script>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="lfffd" Text=" Company Name" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                        <br />
                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label1" Text="Customer Code" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>&nbsp;&nbsp;
                        <asp:CheckBox ID="ChkForEdit" runat="server" Text=" For Edit" CssClass="checkboxbold"
                            OnCheckedChanged="ChkForEdit_CheckedChanged" AutoPostBack="True" />
                        <br />
                        <asp:DropDownList ID="DDNewCustomerCode" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDNewCustomerCode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label2" Text="Order No." runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                        <br />
                        <asp:DropDownList ID="DDNewOrderNo" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDNewOrderNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label3" Text="Description" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                        <br />
                        <asp:DropDownList ID="DDDescription" runat="server" Width="400px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDDescription_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label4" Text="Old Customer Code" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                        <br />
                        <asp:DropDownList ID="DDOldCustomerCode" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDOldCustomerCode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label5" Text="Old Order No" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                        <br />
                        <asp:DropDownList ID="DDOldOrderNo" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="true" OnSelectedIndexChanged="DDOldOrderNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="tdstyle">
                        <div style="height: 250px; width: 80%; overflow: scroll">
                            <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                EmptyDataText="No. Records found." OnRowDataBound="DG_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkboxitem" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="StockNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTStockNo" Text='<%#Bind("TStockNo") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="400px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStockNo" Text='<%#Bind("StockNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table width="95%">
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" UseSubmitBehavior="false"
                            OnClientClick="if (!confirm('Do you want to save Data?')) return; this.disabled=true;this.value = 'wait ...';"
                            CssClass="buttonnorm" Width="70px" />
                        <asp:Button ID="Btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            CssClass="buttonnorm" Width="70px" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
