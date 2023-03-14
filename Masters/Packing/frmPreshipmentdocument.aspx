<%@ Page Title="Pre-Shipment Document" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmPreshipmentdocument.aspx.cs" Inherits="Masters_Packing_frmPreshipmentdocument" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmPreshipmentdocument.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function CheckBoxClick(objref) {

            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
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
                        row.style.backgroundColor = "Orange";
                    }
                    else {
                        inputlist[i].checked = false;
                        row.style.backgroundColor = "White";

                    }
                }
            }

        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";

                    var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Ship From!!\n";
                    }
                    selectedindex = $("#<%=DDPaymentmode.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Payment Mode. !!\n";
                    }
                    selectedindex = $("#<%=DDBuyer.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Ship To. !!\n";
                    }
                    selectedindex = $("#<%=DDOrderNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Order No. !!\n";
                    }
                    if (Message == "") {

                        var grid = document.getElementById("<%=GvDetail.ClientID%>");
                        var inputs = grid.getElementsByTagName("input");
                        for (var i = 0; i < inputs.length; i++) {
                            if (inputs[i].type == "checkbox") {
                                var checkid = inputs[i].id;
                                if (checkid.match(/CPH_Form_GvDetail_Chkboxitem_.*/)) {
                                    var checked = inputs[i].checked;
                                }
                            }
                            if (checked) {
                                if (inputs[i].type == "text") {
                                    var id = inputs[i].id;
                                    if (id.match(/CPH_Form_GvDetail_txtqty_.*/)) {
                                        // do something
                                        if (inputs[i].value == "" || inputs[i].value == "0") {
                                            alert("Qty can not be blank or zero.");
                                            return false;
                                        }
                                    }
                                    if (id.match(/CPH_Form_GvDetail_txtnoofbales_.*/)) {
                                        // do something
                                        if (inputs[i].value == "" || inputs[i].value == "0") {
                                            alert("No of bales can not be blank or zero.");
                                            return false;
                                        }
                                    }

                                }
                            }
                        }
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label Text="Ship From" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="200px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblshipid" Text="Shipment ID" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtshipid" CssClass="textb" runat="server" Enabled="false" Width="100px" />
                        </td>
                        <td>
                            <asp:Label ID="lblshipdate" Text="Shipment Date" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtshipdate" CssClass="textb" runat="server" Width="100px" />
                            <asp:CalendarExtender ID="cal1" runat="server" TargetControlID="txtshipdate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Text="Payment Mode" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDPaymentmode" CssClass="dropdown" Width="200px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text="Bill of Lading" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtbilloflading" CssClass="textb" Width="100px" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="Label3" Text="Total Weight(Kg.)" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txttotalwt" CssClass="textb" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblbuyer" Text="Ship To" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDBuyer" CssClass="dropdown" Width="200px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="DDBuyer_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblorderno" Text="Order No." CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDOrderNo" CssClass="dropdown" Width="150px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <fieldset>
                    <legend>
                        <asp:Label ID="lbldesc" ForeColor="Red" CssClass="labelbold" Text="Product Description"
                            runat="server" />
                    </legend>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <div style="max-height: 400px; overflow: auto">
                                    <asp:GridView ID="GvDetail" runat="server" EmptyDataText="No records Found...." AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sr No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpodate" Text='<%#Bind("orderdate") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblproduct" Text='<%#Bind("ProductDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderqty" Text='<%#Bind("qtyrequired") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shiped Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblshipqty" Text='<%#Bind("shipedqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No of Bales">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtnoofbales" CssClass="textb" Width="70px" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtqty" CssClass="textb" Width="70px" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtamount" CssClass="textb" Width="80px" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
                                                    <asp:Label ID="lblflagsize" Text='<%#Bind("flagsize") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnid" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
