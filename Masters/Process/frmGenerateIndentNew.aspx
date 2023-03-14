<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmGenerateIndentNew.aspx.cs" Inherits="Masters_Process_frmGenerateIndentNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <%-- <script type="text/javascript">

        window.onload = function () {
            debugger;
            var div = document.getElementById('DivBuyerDesc');
            var div_position = document.getElementById('DivBuyerdesc_Position');
            var position = parseInt('<%=Request.Form["div_position"] %>');
            if (isNaN(position)) {
                position = 0;
            }
            div.scrollTop = position;
            div.onscroll = function () {
                div_position.value = div.scrollTop;
            };
        };
</script>--%>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmgenerateindentnew.aspx";
        }        
    </script>
    <script type="text/javascript">
        function CheckBoxClick(objref) {
           
            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
            }

        }
        function AddDyeingRat() {
            var a3 = document.getElementById("<%=hnofinishedid.ClientID %>").value;

            window.open('AddDyeingRat.aspx?' + a3);
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
        function Validate() {
            $(document).ready(function () {
                $("#<%=BtnSave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDProcessName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Process !!\n";
                    }
                    selectedindex = $("#<%=DDPartyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Party !!\n";
                    }

                    if ($("#<%=TDOrderNo.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDOrderNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Order No. !!\n";
                        }
                    }
                    if ($("#<%=TDppno.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDProcessProgramNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select PP No. !!\n";
                        }
                    }
                    if (Message == "") {
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
                Sys.Application.add_load(Validate);
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="leg1" Text="Master Detail" ForeColor="Red" CssClass="labelbold" runat="server" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td id="TDChkEdit" runat="server" visible="false">
                                            <asp:CheckBox ID="Chkedit" Text="For Edit" runat="server" AutoPostBack="true" CssClass="checkboxbold"
                                                OnCheckedChanged="Chkedit_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcomp" Text="CompanyName" CssClass="labelbold" runat="server" />
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label4" runat="server" Text="ProcessName" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="150" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="Vendor Name" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDPartyName" Width="150" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TDCustCode" visible="false" runat="server" class="tdstyle">
                                            <asp:Label ID="Label2" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="TDOrderNo" runat="server" visible="false" class="tdstyle">
                                            <asp:Label ID="Label3" runat="server" Text=" OrderNo" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDOrderNo" Width="140" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="TDppno" runat="server" class="tdstyle">
                                            <asp:Label ID="Label6" runat="server" Text="PPNo" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDProcessProgramNo" Width="140" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDProcessProgramNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td id="TDIndentNo" runat="server" visible="false">
                                            <asp:Label ID="Label1" runat="server" Text=" Indent No." CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDIndentNO" Width="150" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDIndentNO_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text=" Indent No." CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox CssClass="textb" ID="TxtIndentNo" Width="120" runat="server" TabIndex="6"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="Date" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox CssClass="textb" ID="TxtDate" runat="server" Width="100px" TabIndex="7"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text=" ReqDate" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox CssClass="textb" ID="TxtReqDate" runat="server" Width="100px" AutoPostBack="false"
                                                TabIndex="8" BackColor="#7b96bb "></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtReqDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle" id="TDtxtremarks" runat="server" colspan="3">
                                            <asp:Label ID="Label21" runat="server" Text="Remarks" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtremarks" runat="server" Width="330px" CssClass="textb"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="justify">
                                <asp:Label Text="Buyer Description" CssClass="labelbold" ForeColor="Red" runat="server" />
                                <div id="DivBuyerDesc" style="height: 150px; width: 100%; overflow: auto">
                                    <asp:GridView ID="DGBuyerDesc" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                                        Width="400px" >
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAll" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkbox" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                <ControlStyle Height="17px" />
                                                <HeaderStyle Width="150px" />
                                                <ItemStyle HorizontalAlign="left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="customerorderno" HeaderText="BuyerorderNo.">
                                                <ControlStyle Height="17px" />
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QtyRequired" HeaderText="QtyReq.">
                                                <ControlStyle Height="17px" />
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OrderedQty" HeaderText="Ordered Qty.">
                                                <ControlStyle Height="17px" />
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="INDENT QTY">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtindentQty" CssClass="textbox" Width="60px" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderdetailid" Text='<%#Bind("orderdetailid")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                                <asp:Button ID="btngetoutdesc" Text="GetOut Description" runat="server" CssClass="buttonnorm"
                                    OnClick="btngetoutdesc_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label Text="Out Description" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <div id="Div1" runat="server" style="max-height: 500px; overflow: auto">
                        <asp:GridView ID="DGoutdescription" AutoGenerateColumns="False" runat="server" CssClass="grid-views">
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
                                        <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderDescription" Text='<%#Bind("OrderDescription") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Out Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutDescription" Text='<%#Bind("OutDescription") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblunit" Text='<%#Bind("UnitName") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotalqty" Text='<%#Bind("TotalconsmpQty") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PreQty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpreQty" Text='<%#Bind("PreQty") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtoutQty" Width="60px" align="right" runat="server" BackColor="#FFFF66"
                                            Text='<%#Bind("OQty") %>' onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Loss(%)">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtlossperc" Width="70px" align="right" runat="server" BackColor="#FFFF66"
                                            Text='<%#Bind("OLoss") %>' onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRate" Width="60px" align="right" runat="server" BackColor="#FFFF66"
                                            onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtitemremark" Width="150px" align="right" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblppno" Text='<%#Bind("PPno") %>' runat="server" />
                                        <asp:Label ID="lblofinishedid" Text='<%#Bind("ofinishedid") %>' runat="server" />
                                        <asp:Label ID="lblflagsize" Text='<%#Bind("Osizeflag") %>' runat="server" />
                                        <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                        <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                        <asp:Label ID="lblorderdetailid" Text='<%#Bind("orderdetailid") %>' runat="server" />
                                        <asp:Label ID="lblorderqty" Text='<%#Bind("OrderindentQty") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew();" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" ValidationGroup="f1"
                                OnClientClick="return Validation();" Width="50px" OnClick="BtnSave_Click" />
                            <asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" Width="50px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <div id="gride" runat="server" style="max-height: 500px">
                    <asp:GridView ID="DGIndentDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        OnRowDeleting="DGIndentDetail_RowDeleting" OnRowCancelingEdit="DGIndentDetail_RowCancelingEdit"
                        OnRowEditing="DGIndentDetail_RowEditing" OnRowUpdating="DGIndentDetail_RowUpdating">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblindentdetailid" Text='<%#Bind("IndentDetailid") %>' runat="server"
                                />
                                <asp:Label ID="lblindentid" Text='<%#Bind("Indentid") %>' runat="server" />
                                <asp:Label ID="lblHQty" Text='<%#Bind("Quantity") %>' runat="server" />
                                </ItemTemplate> </asp:TemplateField>
                                <asp:TemplateField HeaderText="IndentNo">
                                    <itemtemplate>
                                    <asp:Label ID="lblIndentNo" Text='<%#Bind("IndentNo") %>' runat="server" />
                                </itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OrderNo.">
                                    <itemtemplate>
                                    <asp:Label ID="lblPPNo_OrderNo" Text='<%#Bind("customerOrderNo") %>' runat="server" />
                                </itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OrderDescription">
                                    <itemtemplate>
                                    <asp:Label ID="lblorderDescription" Text='<%#Bind("OrderDescription") %>' runat="server" />
                                </itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OutDescription">
                                    <itemtemplate>
                                    <asp:Label ID="lblOutDescription" Text='<%#Bind("OutDescription") %>' runat="server" />
                                </itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate">
                                    <edititemtemplate>
                                    <asp:TextBox ID="Rate" Width="60px" align="right" runat="server" Text='<%# Bind("rate") %>'
                                        BackColor="#FFFF66" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </edititemtemplate>
                                    <itemtemplate>
                                    <asp:Label ID="lblrate" Text='<%#Bind("rate") %>' runat="server" />
                                </itemtemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OrderQty">
                                    <edititemtemplate>
                                    <asp:TextBox ID="txtorderqty" Width="60px" align="right" runat="server" Text='<%# Bind("orderqty") %>'
                                        BackColor="#FFFF66" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </edititemtemplate>
                                    <itemtemplate>
                                    <asp:Label ID="lblorderqty" Text='<%#Bind("orderqty") %>' runat="server" />
                                </itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IndentQty">
                                    <edititemtemplate>
                                    <asp:TextBox ID="txtindentqty" Width="60px" align="right" runat="server" Text='<%# Bind("indentqty") %>'
                                        BackColor="#FFFF66" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </edititemtemplate>
                                    <itemtemplate>
                                    <asp:Label ID="lblindentQty" Text='<%#Bind("indentqty") %>' runat="server" />
                                </itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <edititemtemplate>
                                    <asp:TextBox ID="Qty" Width="60px" align="right" runat="server" Text='<%# Bind("Quantity") %>'
                                        BackColor="#FFFF66" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </edititemtemplate>
                                    <itemtemplate>
                                    <asp:Label ID="lblQty" Text='<%#Bind("Quantity") %>' runat="server" />
                                </itemtemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" />
                                <asp:TemplateField ShowHeader="False">
                                    <itemtemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="Del" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </itemtemplate>
                                    <headerstyle horizontalalign="Center" width="20px" />
                                    <itemstyle horizontalalign="Center" width="20px" />
                                </asp:TemplateField>
                </Columns> </asp:GridView>
            </div>
            </div>
            <asp:HiddenField ID="hnorderDetailid" runat="server" />
            <asp:HiddenField ID="hnofinishedid" runat="server" />
            <asp:HiddenField ID="DivBuyerdesc_Position" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
