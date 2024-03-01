<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmItemWiseSPIWKForMWS.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Title="ItemWise SPI WK" Inherits="Masters_Order_FrmItemWiseSPIWKForMWS"
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
                        <asp:DropDownList ID="DDLInCompanyName" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLInCompanyName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label3" Text="Date" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtDate" runat="server" Format="dd-MMM-yyyy" Width="100px" CssClass="textb"></asp:TextBox>
                    </td>
                </tr>
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
            <table>
                <tr>
                    <td colspan="3" align="left">
                        <div style="width: 100%; max-height: 400px; overflow: auto">
                            <asp:GridView ID="DGItemDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="Item_Finished_ID"
                                CssClass="grid-views" OnRowDataBound="DGItemDetail_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
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
                                    <asp:TemplateField HeaderText="Item">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemname" Text='<%#Bind("Item_Name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="QualityName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesc" Text='<%#Bind("Description") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Text='<%#Bind("Status") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SPI WK Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSPIWKQty" Text='<%#Bind("SPIWKQty") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStatus" runat="server" align="right" Width="75px" Text='<%# Bind("Status") %>'
                                                onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SPI WK Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSPIWKQty" runat="server" align="right" Width="75px" Text='<%# Bind("SPIWKQty") %>'
                                                onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle VerticalAlign="Middle" />
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
