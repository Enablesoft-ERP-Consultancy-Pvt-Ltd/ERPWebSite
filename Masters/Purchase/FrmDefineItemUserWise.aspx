<%@ Page Title="Define Item User Wise" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmDefineItemUserWise.aspx.cs" Inherits="Masters_Purchase_FrmDefineItemUserWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "FrmDefineItemUserWise.aspx";
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
        function Addsize() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var Shapeid = 0;
                if (document.getElementById('CPH_Form_ddshape')) {
                    var e = document.getElementById("CPH_Form_ddshape");
                    if (e.options.length > 0) {
                        var Shapeid = e.options[e.selectedIndex].value;
                    }
                }
                e = document.getElementById("CPH_Form_DDUnit");
                var VarUnitID = e.options[e.selectedIndex].value;
                window.open('../Carpet/AddSize.aspx?ShapeID=' + Shapeid + '&UnitID=' + VarUnitID + '', '', 'width=1000px,Height=501px');
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

        function CheckBoxClick(objref) {

            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
            }
        }
        function Validation() {
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDCustomerCode.ClientID %>").value <= "0") {
                alert("Pls Select Customer Code");
                document.getElementById("<%=DDCustomerCode.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDOrderNo.ClientID %>").value <= "0") {
                alert("Pls Select Order No");
                document.getElementById("<%=DDOrderNo.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDUserName.ClientID %>").value <= "0") {
                alert("Pls Select User Name");
                document.getElementById("<%=DDUserName.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do you want to save data?')
            }
        }
        
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr id="Tr1" runat="server">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddCompName" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text=" Customer Code" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <br />
                            <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label17" runat="server" Text="Order No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDOrderNo" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="User Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDUserName" runat="server" Width="150px" 
                                CssClass="dropdown" AutoPostBack="True" onselectedindexchanged="DDUserName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div id="gride" runat="server" style="max-height: 400px; overflow: auto">
                                <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    OnRowDataBound="DG_RowDataBound">
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
                                        <asp:TemplateField HeaderText="Item Description">
                                            <ItemTemplate>
                                                <asp:Label ID="LblItemDescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblOrderID" Text='<%#Bind("OrderID") %>' runat="server" />
                                                <asp:Label ID="LblIFinishedID" Text='<%#Bind("IFinishedID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 80%;">
                    <tr>
                        <td>
                            <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                        <td colspan="8" align="right">
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew()"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return Validation();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div id="Div1" runat="server" style="max-height: 400px; overflow: auto">
                                <asp:GridView ID="gvdetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    OnRowDataBound="gvdetail_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Item Description">
                                            <ItemTemplate>
                                                <asp:Label ID="LblItemDescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblOrderID" Text='<%#Bind("OrderID") %>' runat="server" />
                                                <asp:Label ID="LblIFinishedID" Text='<%#Bind("IFinishedID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
