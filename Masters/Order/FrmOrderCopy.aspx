<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmOrderCopy.aspx.cs" Inherits="Masters_Order_FrmOrderCopy"
    Title="Repeat Order" EnableEventValidation="false" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
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
        function ontextchanged() {
            var intFlag = 0;
            var strErrMsg = "";
            var dtDate = document.getElementById("CPH_Form_TxtOrderDate").value
            var dtDate1 = document.getElementById("CPH_Form_TxtDeliveryDate").value
            var orderdate = new Date(dtDate);
            var delivery = new Date(dtDate1);
            if (orderdate > delivery) {
                strErrMsg = strErrMsg + "Delivery Date Cann't Be Shorter Than Order Date \n";
                document.getElementById("CPH_Form_TxtDeliveryDate").value = document.getElementById("CPH_Form_TxtOrderDate").value;
                intFlag++;
            }
            // Display error message if a field is not completed
            if (intFlag != 0) {
                alert(strErrMsg);

                return false;
            }
            else
                return true;
        }
        function checkDate() {
            // define date string to test
            var SDate = new date(document.getElementById("CPH_Form_TxtOrderDate").value).toDateString();
            var EDate = new date(document.getElementById("CPH_Form_TxtDeliveryDate").value).toDateString();
            var alertReason1 = 'Dispatch Date must be greater than or equal to Order Date.'
            var alertReason2 = 'Dispatch Date can not be less than Order Date.';
            var endDate = new Date(EDate);
            var startDate = new Date(SDate);
            if (SDate != '' && EDate != '' && startDate > endDate) {
                alert(alertReason1);
                document.getElementById(DispatchDate).value = "";
                return false;
            }
            else if (SDate == '') {
                alert("Please enter Start Date");
                return false;
            }
            else if (EDate == '') {
                alert("Please enter End Date");
                return false;
            }
        }
        
    </script>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <div id="maindiv">
                <table width="50%" style="border: 1px solid Gray; border-radius: 1px; width: 50%;">
                    <tr>
                        <td align="right" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="ChkEditOrder" runat="server" Text=" EDIT ORDER" AutoPostBack="True"
                                OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            COMPANY NAME*
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="CUSTOMER CODE*"></asp:Label>
                        </td>
                        <td class="tdstyle" colspan="2">
                            CUST ORDER NO*
                        </td>
                        <td class="tdstyle">
                            ORDER TYPE*
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="150px" TabIndex="0"
                                runat="server" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                                Width="150px" TabIndex="2" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TxtCustOrderNo" runat="server" CssClass="textb" AutoPostBack="True"
                                TabIndex="3" OnTextChanged="TxtCustOrderNo_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqUser" runat="server" ControlToValidate="TxtCustOrderNo"
                                CssClass="errormsg" ErrorMessage="Please, Enter Customer Number!">*</asp:RequiredFieldValidator>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustOrderNo" runat="server" AutoPostBack="True"
                                Visible="false" Width="150px" OnSelectedIndexChanged="DDCustOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="ddordertype" runat="server" AutoPostBack="True"
                                Width="150px" TabIndex="4">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trneworder" runat="server">
                        <td class="tdstyle">
                            OUR ORDER NO*
                        </td>
                        <td class="tdstyle">
                            ORDER DATE*
                        </td>
                        <td id="td1" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblcustname" runat="server" Text="Cust.Order DATE*"></asp:Label>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="LblDelvDate" runat="server" Text="DELIVERY DATE*"></asp:Label>
                        </td>
                        <td id="td" runat="server" class="tdstyle">
                            <asp:Label ID="lblduedate" runat="server" Text="FROM ORDER NO"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtLocalOrderNo" runat="server" CssClass="textb" TabIndex="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtOrderDate" runat="server" TabIndex="7" OnTextChanged="TxtOrderDate_TextChanged"
                                AutoPostBack="true"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtOrderDate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="TDTxtcustorderdt" runat="server" visible="false">
                            <asp:TextBox CssClass="textb" ID="Txtcustorderdt" runat="server" AutoPostBack="true"
                                Visible="false" TabIndex="9"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender5" Format="dd-MMM-yyyy" runat="server"
                                TargetControlID="Txtcustorderdt">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtDeliveryDate" runat="server" AutoPostBack="true"
                                TabIndex="8" onchange="javascript: ontextchanged();"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtDeliveryDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDFromOrderNo" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDFromOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4">
                            <asp:CheckBox ID="CHKFORCURRENTCONSUMPTION" Text="  FOR CURRENT CONSUMPTION" runat="server"
                                Visible="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <asp:Label ID="Lblmessage" runat="server" Text="" ForeColor="red" Font-Bold="true"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <asp:Label ID="lblvalidMessage" runat="server" Text="" ForeColor="red" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div style="width: 65%; height: 150px; overflow: scroll">
                    <asp:GridView ID="GDOrderSummary" Width="100%" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="DetailId" CssClass="grid-view" OnRowDataBound="DGOrderDetail_RowDataBound"
                        OnRowCreated="DGOrderDetail_RowCreated" OnRowDeleting="GDOrderSummary_RowDeleting">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <asp:BoundField DataField="ITEM_NAME" HeaderText="ITEM_NAME">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description">
                                <HeaderStyle HorizontalAlign="Left" Width="350px" />
                                <ItemStyle HorizontalAlign="Left" Width="350px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Qty" HeaderText="Qty">
                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Area" HeaderText="Area">
                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate" HeaderText="Rate">
                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Amount" HeaderText="Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="70px" />
                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="Del" OnClientClick="return confirm('Do You Want To Delete ?')"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <div style="width: 80%; height: 250px; overflow: scroll">
                    <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="OrderDetailId" CssClass="grid-view" OnRowDataBound="DGOrderDetail_RowDataBound"
                        OnRowCreated="DGOrderDetail_RowCreated">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk1" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ITEM_NAME" HeaderText="ITEM_NAME">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description">
                                <HeaderStyle HorizontalAlign="Left" Width="350px" />
                                <ItemStyle HorizontalAlign="Left" Width="350px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="Qty" Text='<%# Bind("Qty") %>' runat="server" Width="75px" onkeypress="return isNumberKey(event);"
                                        CssClass="textb" OnTextChanged="TextQtyChanged_Event" AutoPostBack="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Area" HeaderText="Area">
                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Rate">
                                <ItemTemplate>
                                    <asp:TextBox ID="Rate" Text='<%# Bind("Rate") %>' runat="server" Width="75px" onkeypress="return isNumber(event);"
                                        CssClass="textb" OnTextChanged="TextQtyChanged_Event" AutoPostBack="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Amount" HeaderText="Amount">
                                <HeaderStyle HorizontalAlign="Left" Width="70px" />
                                <ItemStyle HorizontalAlign="Left" Width="70px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Remark">
                                <ItemTemplate>
                                    <asp:TextBox ID="Remark" Text='<%# Bind("Remark") %>' runat="server" Width="200px"
                                        CssClass="textb"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="DispatchDate" Text='<%# Bind("DispatchDate") %>' runat="server"
                                        Format="dd-MMM-yyyy" Width="85px" CssClass="textb" OnTextChanged="TxtDispatchDate_Changed"
                                        AutoPostBack="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="DispatchDate">
                                    </asp:CalendarExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Label ID="OrderCaltype" runat="server" Text='<%# Bind("OrderCaltype") %>' Visible="false">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <div style="width: 900px;">
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox CssClass="textb" ID="TxtTotalQtyRequired" Enabled="false" Visible="false"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="textb" ID="TxtOrderArea" Enabled="false" Visible="false" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="textb" ID="TxtTotalAmount" Enabled="false" Visible="false"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: right;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClick="BtnNew_Click" />
                                    <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                                        OnClientClick="return confirm('Do you want to save data?')" />
                                    <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
