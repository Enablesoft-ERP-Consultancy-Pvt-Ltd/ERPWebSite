<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmTagging.aspx.cs" MasterPageFile="~/ERPmaster.master"
    Title="Order Tagging" Inherits="Masters_Campany_FrmTagging" EnableEventValidation="false" %>

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
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label1" Text="Customer Code" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>&nbsp;&nbsp;
                        <asp:CheckBox ID="ChkForEdit" runat="server" Text=" For Edit" CssClass="checkboxbold"
                            OnCheckedChanged="ChkForEdit_CheckedChanged" AutoPostBack="True" />
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label2" Text="Order No." runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="DDLInCompanyName" runat="server" Width="300px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLInCompanyName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLCustomerCode" runat="server" Width="250px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLCustomerCode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLOrderNo" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLOrderNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label3" Text="Order Date" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtOrderDate" runat="server" Format="dd-MMM-yyyy" Width="100px"
                            CssClass="textb"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbldeldate" runat="server" Text="Delivery Date" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtDeliveryDate" runat="server" Format="dd-MMM-yyyy" Width="100px"
                            CssClass="textb"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text=" Req Date" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtReqDate" runat="server" Width="100px" Format="dd-MMM-yyyy" align="left"
                            CssClass="textb" BackColor="#7b96bb" onchange="javascript: ontextchanged();"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtReqDate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text=" StockNo" CssClass="labelbold" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="ChkForConsumption" runat="server" Text=" For Supplier Consumption"
                            AutoPostBack="True" CssClass="checkboxnormal" />
                        <br />
                        <asp:TextBox ID="TxtStockNo" runat="server" Width="550px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:Button ID="BtnDelStockNo" CssClass="buttonnorm" Text="Del StockNo" runat="server"
                            OnClientClick="return Validate();" OnClick="BtnDelStockNo_Click" />
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
                        <asp:Button ID="BtnForStockItem" runat="server" Text="For Stock Item" Visible="false"
                            Width="100px" CssClass="buttonnorm" />
                        <asp:Button ID="BtnForProductionItem" runat="server" Enabled="false" Width="150px"
                            Text="For Production Item" OnClick="BtnForProductionItem_Click" CssClass="buttonnorm" />
                        <asp:Button ID="btnproductionsummary" runat="server" Text="Production Summary" CssClass="buttonnorm"
                            Visible="false" OnClick="btnproductionsummary_Click" />
                        <asp:Button ID="btninterprodstockno" CssClass="buttonnorm" Text="Internal Prod. Stock No."
                            runat="server" Visible="false" OnClick="btninterprodstockno_Click" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="3" align="left">
                        <div style="width: 100%; max-height: 400px; overflow: auto">
                            <asp:GridView ID="DGOrderDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="OrderId"
                                CssClass="grid-views" OnRowDataBound="DGOrderDetail_RowDataBound1">
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
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesc" Text='<%#Bind("Description") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="O Qty.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbloqty" Text='<%#Bind("qtyrequired") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S Qty.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsqty" Text='<%#Bind("Avialable_stock") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pre Prod Qty.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreprodassignedqty" Text='<%#Bind("PreProdAssignedQty") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pre Internal Prod Qty.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreinternalprodassignedqty" Text='<%#Bind("preinternalprodassignedqty") %>'
                                                runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty. From Stock">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTagStock" Width="75px" align="right" runat="server" Text='<%# Bind("TagStock") %>'
                                                onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prod Qty Req.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtProd_Qty_Req" runat="server" align="right" Width="75px" Text='<%# Bind("Prod_Qty_Req") %>'
                                                onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prod Weaving Rate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtProd_Weaving_Rate" runat="server" align="right" Width="75px"
                                                Text='<%# Bind("WEAVINGRATEOUTSIDE") %>' onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="D.P. Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSOQTY" runat="server" align="right" Width="75px" Text='<%# Bind("SOQty") %>'
                                                onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Internal Prod Qty Req.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInt_prod_qty_req" runat="server" align="right" Width="75px" Text='<%# Bind("INTERNALPRODASSIGNEDQTY") %>'
                                                onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Int Weaving Rate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInt_Weaving_Rate" runat="server" align="right" Width="75px" Text='<%# Bind("WEAVINGRATEINSIDE") %>'
                                                onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BuyerSrno" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcqid" Text='<%#Bind("CQID") %>' runat="server" />
                                            <asp:Label ID="lbldsrno" Text='<%#Bind("Dsrno") %>' runat="server" />
                                            <asp:Label ID="lblcsrno" Text='<%#Bind("csrno") %>' runat="server" />
                                            <asp:Label ID="lblProdWeavingRate" Text='<%#Bind("WEAVINGRATEOUTSIDE") %>' runat="server" />
                                            <asp:Label ID="lblIntWeavingRate" Text='<%#Bind("WEAVINGRATEINSIDE") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btninterprodstockno" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
