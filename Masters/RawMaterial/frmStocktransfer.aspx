<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmStocktransfer.aspx.cs"
    Inherits="Masters_RawMaterial_frmStocktransfer" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" Title="Stock Transfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewClick() {
            window.location.href = "frmStocktransfer.aspx";
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
        function ValidateSave() {
            if (document.getElementById('CPH_Form_DDFCompName').options[document.getElementById('CPH_Form_DDFCompName').selectedIndex].value == 0) {
                alert("Please Select From CompanyName....!");
                document.getElementById("CPH_Form_DDFCompName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDTCompName').options[document.getElementById('CPH_Form_DDTCompName').selectedIndex].value == 0) {
                alert("Please Select To CompanyName....!");
                document.getElementById("CPH_Form_DDTCompName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDChallanNo') != null) {
                if (document.getElementById('CPH_Form_DDChallanNo').options.length == 0) {
                    alert("ChallanNo  must have a value....!");
                    document.getElementById("CPH_Form_DDChallanNo").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDChallanNo').options[document.getElementById('CPH_Form_DDChallanNo').selectedIndex].value == 0) {
                    alert("Please select ChallanNo....!");
                    document.getElementById("CPH_Form_DDChallanNo").focus();
                    return false;
                }
            }
            //            if (document.getElementById("CPH_Form_TxtChallanNo").value == "") {
            //                alert("Please Fill Challan No....");
            //                document.getElementById("CPH_Form_TxtChallanNo").focus();
            //                return false;
            //            }
            if (document.getElementById('CPH_Form_ddCatagory').options[document.getElementById('CPH_Form_ddCatagory').selectedIndex].value == 0) {
                alert("Please Select Category Name....!");
                document.getElementById("CPH_Form_ddCatagory").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_dditemname').options[document.getElementById('CPH_Form_dditemname').selectedIndex].value == 0) {
                alert("Please Select Item Name....!");
                document.getElementById("CPH_Form_dditemname").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_dquality') != null) {
                if (document.getElementById('CPH_Form_dquality').options.length == 0) {
                    alert("Quality name must have a value....!");
                    document.getElementById("CPH_Form_dquality").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_dquality').options[document.getElementById('CPH_Form_dquality').selectedIndex].value == 0) {
                    alert("Please select Quality....!");
                    document.getElementById("CPH_Form_dquality").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_dddesign') != null) {
                if (document.getElementById('CPH_Form_dddesign').options.length == 0) {
                    alert("Design name must have a value....!");
                    document.getElementById("CPH_Form_dddesign").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_dddesign').options[document.getElementById('CPH_Form_dddesign').selectedIndex].value == 0) {
                    alert("Please select Design....!");
                    document.getElementById("CPH_Form_dddesign").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddcolor') != null) {
                if (document.getElementById('CPH_Form_ddcolor').options.length == 0) {
                    alert("Color name must have a value....!");
                    document.getElementById("CPH_Form_ddcolor").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddcolor').options[document.getElementById('CPH_Form_ddcolor').selectedIndex].value == 0) {
                    alert("Please select Color....!");
                    document.getElementById("CPH_Form_ddcolor").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddshape') != null) {
                if (document.getElementById('CPH_Form_ddshape').options.length == 0) {
                    alert("Shape name must have a value....!");
                    document.getElementById("CPH_Form_ddshape").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddshape').options[document.getElementById('CPH_Form_ddshape').selectedIndex].value == 0) {
                    alert("Please select Shape....!");
                    document.getElementById("CPH_Form_ddshape").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddsize') != null) {
                if (document.getElementById('CPH_Form_ddsize').options.length == 0) {
                    alert("Size name must have a value....!");
                    document.getElementById("CPH_Form_ddsize").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddsize').options[document.getElementById('CPH_Form_ddsize').selectedIndex].value == 0) {
                    alert("Please select Size....!");
                    document.getElementById("CPH_Form_ddsize").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDFGodown') != null) {
                if (document.getElementById('CPH_Form_DDFGodown').options.length == 0) {
                    alert("From godown  must have a value....!");
                    document.getElementById("CPH_Form_DDFGodown").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDFGodown').options[document.getElementById('CPH_Form_DDFGodown').selectedIndex].value == 0) {
                    alert("Please select From godown....!");
                    document.getElementById("CPH_Form_DDFGodown").focus();
                    return false;
                }
            }

            if ($("#<%=TDFBinNo.ClientID %>").is(':visible')) {
                var selectedindex = $("#<%=DDFBinNo.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    alert("Please select From Bin No. !");
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDTGodown') != null) {
                if (document.getElementById('CPH_Form_DDTGodown').options.length == 0) {
                    alert("To godown  must have a value....!");
                    document.getElementById("CPH_Form_DDTGodown").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDTGodown').options[document.getElementById('CPH_Form_DDTGodown').selectedIndex].value == 0) {
                    alert("Please select To godown....!");
                    document.getElementById("CPH_Form_DDTGodown").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDTBinNo.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDTBinNo').value <= "0") {
                    alert("Please Select To BinNo....!");
                    document.getElementById("CPH_Form_DDTBinNo").focus();
                    return false;
                }
            }
            if ($("#<%=TDTBinNo.ClientID %>").is(':visible')) {
                var selectedindex = $("#<%=DDTBinNo.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    alert("Please select To Bin No. !");
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddlotno') != null) {
                if (document.getElementById('CPH_Form_ddlotno').options.length == 0) {
                    alert("Lot No  must have a value....!");
                    document.getElementById("CPH_Form_ddlotno").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddlotno').options[document.getElementById('CPH_Form_ddlotno').selectedIndex].value == 0) {
                    alert("Please select Lot No....!");
                    document.getElementById("CPH_Form_ddlotno").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDTagNo') != null) {
                if (document.getElementById('CPH_Form_DDTagNo').options.length == 0) {
                    alert("Tag No  must have a value....!");
                    document.getElementById("CPH_Form_DDTagNo").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDTagNo').options[document.getElementById('CPH_Form_DDTagNo').selectedIndex].value == 0) {
                    alert("Please Select TagNo....!");
                    document.getElementById("CPH_Form_DDTagNo").focus();
                    return false;
                }
            }
            //            if (document.getElementById("<%=TDTagNo.ClientID %>")) {
            //                if (document.getElementById('CPH_Form_DDTagNo').value <= "0") {
            //                    alert("Please Select TagNo3....!");
            //                    document.getElementById("CPH_Form_DDTagNo").focus();
            //                    return false;
            //                }
            //            }

            if (document.getElementById('CPH_Form_DDunit').options[document.getElementById('CPH_Form_DDunit').selectedIndex].value == 0) {
                alert("Please Select Unit....!");
                document.getElementById("CPH_Form_DDunit").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_txtissqty').value == "" || document.getElementById('CPH_Form_txtissqty').value == "0") {
                alert("Pls Fill Transfer Qty....!");
                document.getElementById('CPH_Form_txtissqty').focus();
                return false;
            }
            return confirm('Do You Want To Save?')
        }

    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="height: 750px; width: auto; margin-right: 0px;">
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="ChkEdit" runat="server" Text=" For Edit" Font-Bold="true" OnCheckedChanged="ChkEdit_CheckedChanged"
                                AutoPostBack="true" />
                        </td>
                        <td id="TDCustomerCode" runat="server" visible="false" colspan="2">
                            <asp:Label ID="Label24" Text="Customer Code" runat="server" CssClass="labelbold" />
                            &nbsp;<asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDOrderNo" runat="server" visible="false" colspan="2">
                            <asp:Label ID="Label25" Text="Order No" runat="server" CssClass="labelbold" />
                            &nbsp;<asp:DropDownList ID="DDOrderNo" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl" Text=" From Company" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDFCompName" runat="server" Width="150px" TabIndex="1" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDFCompName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="  To Company" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDTCompName" runat="server" Width="150px" TabIndex="2" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDTCompName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Prodcode" runat="server" class="tdstyle" visible="false">
                            <asp:Label ID="Label3" runat="server" Text="ProdCode" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="TxtProdCode" runat="server" AutoPostBack="True" Width="100px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td id="TDEditchallanNo" runat="server" visible="false">
                            <asp:Label ID="Label2" Text="   Challan No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDChallanNo" runat="server" Width="150px" TabIndex="3" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label18" Text="   Transfer Date" runat="server" CssClass="labelbold" />
                            <b style="color: Red">&nbsp; *</b><br />
                            <asp:TextBox ID="txtdate" runat="server" TabIndex="18" Width="100px" CssClass="textb"
                                BackColor="Beige"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtdate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label4" Text="  Challan No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtChallanNo" runat="server" Width="100px" CssClass="textb" TabIndex="4"
                                Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label5" Text=" CategoryName" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" AutoPostBack="True"
                                TabIndex="5" CssClass="dropdown" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label6" Text=" Item Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="dditemname" runat="server" Width="150px" TabIndex="6" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr id="Tr3" runat="server">
                        <td id="TDQuality" runat="server" visible="false">
                            <asp:Label ID="Label7" Text=" Quality" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="dquality" runat="server" Width="150px" TabIndex="7" CssClass="dropdown"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td id="TDDesign" runat="server" visible="false">
                            <asp:Label ID="Label8" Text="  Design" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="150px" TabIndex="8" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDColor" runat="server" visible="false">
                            <asp:Label ID="Label9" Text=" Color" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="150px" TabIndex="9" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDShape" runat="server" visible="false">
                            <asp:Label ID="Label10" Text="  Shape" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" TabIndex="10"
                                CssClass="dropdown" OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDSize" runat="server" visible="false">
                            <asp:Label ID="Label11" Text=" From" runat="server" CssClass="labelbold" />
                            Size
                            <%--<asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" />Check
                            for Mtr--%>
                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Ft</asp:ListItem>
                                <asp:ListItem Value="1">MTR</asp:ListItem>
                                <asp:ListItem Value="2">Inch</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="150px" TabIndex="11" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDShade" runat="server" visible="false">
                            <asp:Label ID="Label12" Text=" ShadeColor" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                TabIndex="12">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label13" Text="  From Godown" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDFGodown" runat="server" Width="150px" TabIndex="13" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDFGodown_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDFBinNo" runat="server" visible="false">
                            <asp:Label ID="Label20" Text=" From Bin No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDFBinNo" runat="server" Width="150px" TabIndex="14" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDFBinNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label14" Text=" To Godown" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDTGodown" runat="server" Width="150px" TabIndex="14" CssClass="dropdown"
                                OnSelectedIndexChanged="DDTGodown_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td id="TDTBinNo" runat="server" visible="false">
                            <asp:Label ID="Label21" Text=" To Bin No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDTBinNo" runat="server" Width="150px" TabIndex="14" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDLotNo" runat="server">
                            <asp:Label ID="Label15" Text="  From LotNo." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddlotno" runat="server" AutoPostBack="True" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddlotno_SelectedIndexChanged" TabIndex="15">
                            </asp:DropDownList>
                        </td>
                        <td id="TDTagNo" runat="server" visible="false">
                            <asp:Label ID="Label19" Text="  From TagNo." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDTagNo" runat="server" AutoPostBack="True" Width="150px" CssClass="dropdown"
                                TabIndex="16" OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDunit" runat="server">
                            <asp:Label ID="lblunit" Text="Unit." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDunit" runat="server" AutoPostBack="True" Width="80px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <%--</caption>--%>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="TDStock" runat="server">
                            <asp:Label ID="Label16" Text="  Stock" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtstock" runat="server" Enabled="false" CssClass="textb" Width="75px"
                                TabIndex="16"></asp:TextBox>
                        </td>
                        <td id="TDConsmpQty" runat="server">
                            <asp:Label ID="Label26" Text=" Cons Qty" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtConsQty" runat="server" Enabled="false" CssClass="textb" BackColor="Beige"
                                Width="75px" onkeypress="return isNumber(event);" TabIndex="17"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label17" Text=" Transfer Qty" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtissqty" runat="server" CssClass="textb" BackColor="Beige" Width="75px"
                                AutoPostBack="True" onkeypress="return isNumber(event);" TabIndex="17"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label27" Text=" Bell Wt" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtBellWeight" runat="server" CssClass="textb" Width="75px"
                                 onkeypress="return isNumber(event);" TabIndex="17"></asp:TextBox>
                        </td>
                        <td id="TD1" runat="server">
                            <asp:Label ID="Label22" Text="EWay BillNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtEWayBillNo" runat="server" CssClass="textb" Width="130px" TabIndex="19"></asp:TextBox>
                        </td>
                        <td id="TD2" runat="server">
                            <asp:Label ID="Label23" Text="Remarks" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRemarks" runat="server" CssClass="textb" Width="150px" TextMode="MultiLine"
                                Height="50px" TabIndex="19"></asp:TextBox>
                        </td>
                        <td align="right" colspan="6">
                            <br />
                            <asp:Button ID="btnnew" runat="server" CssClass="buttonnorm" OnClientClick="return NewClick();"
                                TabIndex="19" Text="New" Width="75px" />
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                                OnClientClick="return  ValidateSave();" TabIndex="20" Text="Save" Width="75px" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                TabIndex="21" Text="Close" Width="75px" />
                            <asp:Button ID="btnpreview" runat="server" CssClass="buttonnorm" TabIndex="21" Text="Preview"
                                Width="75px" OnClick="btnpreview_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="6" align="center" id="TDDGOrderConsmption" runat="server" visible="false">
                            <div style="width: 100%; height: 200px; overflow: auto;">
                                <asp:GridView ID="DGOrderConsmption" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                    DataKeyNames="OrderID" OnRowDataBound="DGOrderConsmption_RowDataBound" OnSelectedIndexChanged="DGOrderConsmption_SelectedIndexChanged">
                                    <HeaderStyle Height="25px" />
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="true">
                                            <HeaderStyle Width="250px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ConsQty" HeaderText="Cons Qty" ReadOnly="true">
                                            <HeaderStyle Width="80px" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Already TransferQty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAreadyTransferQty" runat="server" Text='<%# Bind("AlreadyTransferQty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bal Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceQty" runat="server" Text='<%# Convert.ToDecimal(Eval("ConsQty")) - Convert.ToDecimal(Eval("AlreadyTransferQty")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Finishedid" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="IFinishedID" runat="server" Width="0px" Visible="false" Text='<%# Bind("IFinishedID") %>'></asp:Label>
                                                <asp:Label ID="CategoryID" runat="server" Width="0px" Visible="false" Text='<%# Bind("CategoryID") %>'></asp:Label>
                                                <asp:Label ID="ItemID" runat="server" Width="0px" Visible="false" Text='<%# Bind("ItemID") %>'></asp:Label>
                                                <asp:Label ID="QualityId" runat="server" Width="0px" Visible="false" Text='<%# Bind("QualityId") %>'></asp:Label>
                                                <asp:Label ID="DesignId" runat="server" Width="0px" Visible="false" Text='<%# Bind("DesignId") %>'></asp:Label>
                                                <asp:Label ID="ColorId" runat="server" Width="0px" Visible="false" Text='<%# Bind("ColorId") %>'></asp:Label>
                                                <asp:Label ID="ShapeId" runat="server" Width="0px" Visible="false" Text='<%# Bind("ShapeId") %>'></asp:Label>
                                                <asp:Label ID="SizeId" runat="server" Width="0px" Visible="false" Text='<%# Bind("SizeId") %>'></asp:Label>
                                                <asp:Label ID="ShadecolorId" runat="server" Width="0px" Visible="false" Text='<%# Bind("ShadecolorId") %>'></asp:Label>
                                                <asp:Label ID="lblConsQty" runat="server" Width="0px" Visible="false" Text='<%# Bind("ConsQty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="0px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                        <td colspan="6" align="center">
                            <div style="width: 100%; height: 200px; overflow: auto;">
                                <asp:GridView ID="DGStockTransfer" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                    DataKeyNames="DetailId" OnRowCancelingEdit="DGStockTransfer_RowCancelingEdit"
                                    OnRowEditing="DGStockTransfer_RowEditing" OnRowUpdating="DGStockTransfer_RowUpdating"
                                    OnRowDeleting="DGStockTransfer_RowDeleting" OnRowDataBound="DGStockTransfer_RowDataBound">
                                    <HeaderStyle Height="25px" />
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory" ReadOnly="true">
                                            <HeaderStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name" ReadOnly="true">
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ReadOnly="true">
                                            <HeaderStyle Width="300px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FLotNo" HeaderText="LotNo" ReadOnly="true">
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FTagNo" HeaderText="TagNo" ReadOnly="true">
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FromGodown" HeaderText="FGodownName" ReadOnly="true">
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ToGodown" HeaderText="TGodownName" ReadOnly="true">
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="Qty" Width="60px" align="right" runat="server" Text='<%# Bind("Qty") %>'
                                                    BackColor="#FFFF66" onkeypress="return isNumber(event);"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Qty">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                            
                                                </asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRemarks" Width="200px" align="right" runat="server" Text='<%# Bind("Remarks") %>'
                                                    BackColor="#FFFF66"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EWayBill No">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEWayBillNo" Width="100px" align="right" runat="server" Text='<%# Bind("EWayBillNo") %>'
                                                    BackColor="#FFFF66"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEWayBillNo" runat="server" Text='<%# Bind("EWayBillNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Finishedid" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Finishedid" runat="server" Width="0px" Visible="false" Text='<%# Bind("Item_Finishedid") %>'></asp:Label>
                                                <asp:Label ID="FGodownId" runat="server" Width="0px" Visible="false" Text='<%# Bind("FGodownId") %>'></asp:Label>
                                                <asp:Label ID="TGodownid" runat="server" Width="0px" Visible="false" Text='<%# Bind("TGodownid") %>'></asp:Label>
                                                <asp:Label ID="FLotNo" runat="server" Width="0px" Visible="false" Text='<%# Bind("FLotNo") %>'></asp:Label>
                                                <asp:Label ID="TransferId" runat="server" Width="0px" Visible="false" Text='<%# Bind("TransferId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="0px" />
                                        </asp:TemplateField>
                                        <asp:CommandField ShowEditButton="True" />
                                        <asp:CommandField ShowDeleteButton="True" />
                                    </Columns>
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
