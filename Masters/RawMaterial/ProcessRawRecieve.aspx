<%@ Page Title="Weaver Raw Receive" Language="C#" AutoEventWireup="true" CodeFile="ProcessRawRecieve.aspx.cs"
    EnableEventValidation="false" MasterPageFile="~/ERPmaster.master" Inherits="Masters_RawMaterial_ProcessRawRecieve" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "ProcessRawRecieve.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx');
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
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="height: 750px">
                <table>
                    <tr id="TRempcodescan" runat="server" visible="false">
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Label18" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                        </td>
                        <td>
                            <asp:Label ID="Label16" Text="Enter/Scan StockNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtStockNoScan" CssClass="textb" runat="server" Width="150px" Height="20px"
                                AutoPostBack="true" OnTextChanged="txtStockNoScan_TextChanged" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblCustomerCodeAndOrderNo" Text="" runat="server" CssClass="labelbold"
                                ForeColor="Red" />
                        </td>
                    </tr>
                    <tr id="Tr1" runat="server">
                        <td>
                            <asp:Label ID="lbl" Text=" POrder No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPOrderNo" runat="server" Width="90px" AutoPostBack="True" CssClass="textb"
                                OnTextChanged="TxtPOrderNo_TextChanged"></asp:TextBox>
                        </td>
                        <td id="Td1">
                            <asp:Label ID="Label1" Text="  Company Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddCompName" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddCompName" runat="server" TargetControlID="ddCompName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label33" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td id="Td2">
                            <asp:Label ID="Label2" Text=" Process Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddProcessName" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddProcessName" runat="server" TargetControlID="ddProcessName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDProductionunit" runat="server" visible="false">
                            <asp:Label ID="Label20" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDLoomNo" runat="server" visible="false">
                            <asp:Label ID="Label21" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" Width="150px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDLoomNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDEmpName" runat="server" visible="true">
                            <asp:Label ID="Label3" Text="  Emp Name" runat="server" CssClass="labelbold" />
                            <asp:CheckBox ID="ChKForEdit" runat="server" Text=" Check For Edit" CssClass="checkboxbold"
                                AutoPostBack="true" OnCheckedChanged="ChKForEdit_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="ddempname" runat="server" Width="250px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddempname" runat="server" TargetControlID="ddempname"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Td6">
                            <asp:Label ID="Label4" Text="  POrder No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddPOrderNo" Width="115px" runat="server" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddPOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddchalan" runat="server" TargetControlID="ddPOrderNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="tdChallanNo" runat="server" visible="false">
                            <asp:Label ID="Label5" Text=" Challan No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDChallanNo" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDChallanNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Td5" align="center">
                            <asp:Label ID="Label6" Text="  Recieve Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtdate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="Td3">
                            <asp:Label ID="Label7" Text="  Challan No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtChallanNo" Width="100px" runat="server" AutoPostBack="True" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr id="Tr3" runat="server">
                        <td id="procode" runat="server">
                            <asp:Label ID="Label8" Text=" Prod. Code" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="Txtprodcode" Width="100px" runat="server" OnTextChanged="Txtprodcode_TextChanged"
                                AutoPostBack="True" CssClass="textb"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblcategoryname" runat="server" Text="Catagory Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddCatagory" runat="server" TargetControlID="ddCatagory"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dditemname" runat="server" Width="150px" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchdditemname" runat="server" TargetControlID="dditemname"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="ql" runat="server" visible="false">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dquality" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchdquality" runat="server" TargetControlID="dquality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="dsn" runat="server" visible="false">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchdddesign" runat="server" TargetControlID="dddesign"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="clr" runat="server" visible="false">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown" OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddcolor" runat="server" TargetControlID="ddcolor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="shp" runat="server" visible="false">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="100px" CssClass="dropdown" OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddshape" runat="server" TargetControlID="ddshape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="sz" runat="server" visible="false">
                            <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="100px" CssClass="dropdown" OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddsize" runat="server" TargetControlID="ddsize"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="shd" runat="server" visible="false">
                            <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddlshade" runat="server" TargetControlID="ddlshade"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>

                            <asp:CheckBox ID="ChkForWastedMaterial" runat="server" Text=" Check For Wasted Material" CssClass="checkboxbold"
                                 Visible="false" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" Text="  Godown Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddgodown" runat="server" Width="150px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="ddgodown_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddgodown" runat="server" TargetControlID="ddgodown"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDlotno" runat="server">
                            <asp:Label ID="Label17" Text=" Lot No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDlotno" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="true"
                                OnSelectedIndexChanged="DDlotno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDtagno" runat="server">
                            <asp:Label ID="Label19" Text=" Tag No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDtagno" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="true"
                                OnSelectedIndexChanged="DDtagno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDBinNo" runat="server" visible="false">
                            <asp:Label ID="Label14" Text=" Bin No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDBinNo" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label10" Text="  Unit" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddlunit" runat="server" Width="100px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddlunit" runat="server" TargetControlID="ddlunit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Tdconetype" runat="server">
                            <asp:Label ID="Label22" Text="Cone Type" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDconetype" CssClass="dropdown" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label32" Text="No. Of Cone" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:TextBox ID="txtnoofcone" CssClass="textb" Width="70px" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="Label11" Text=" Iss Qty" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtissue" runat="server" Width="100px" ReadOnly="true" CssClass="textb"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td runat="server" visible="false">
                            <asp:Label ID="Label12" Text="   Lot No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtlot" runat="server" Width="150px" CssClass="textb" Enabled="false"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td runat="server" visible="false">
                            <asp:Label ID="Label15" Text="   Tag No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txttagno" runat="server" Width="150px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>
                        <%-- <td id="TDConsumeQty" runat="server" visible="false">
                            <asp:Label ID="Label22" Text="Consume Qty." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtConsumeQty" runat="server" Width="100px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>
                         <td id="TDAlreadyReceivedQty" runat="server" visible="false">
                            <asp:Label ID="Label24" Text="Already Rec Qty" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtAlreadyReceivedQty" runat="server" Width="100px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>
                         <td id="TDBalanceReceive" runat="server" visible="false">
                            <asp:Label ID="Label23" Text="Bal Receive Qty." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtBalanceReceiveQty" runat="server" Width="100px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>--%>
                        <td>
                            <asp:Label ID="Label13" Text="Rec Qty" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtrecqty" runat="server" Width="100px" OnTextChanged="txtrecqty_TextChanged"
                                AutoPostBack="True" CssClass="textb" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="75%">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblRemark" Text="Remark" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtremark" CssClass="textb" Width="50%" runat="server" TextMode="MultiLine"
                                Height="51px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Label ID="LblError" runat="server" Text="Label" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                        <td colspan="4" align="right">
                            <asp:Button ID="btnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                CssClass="buttonnorm" />
                            &nbsp;<asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click"
                                OnClientClick="return confirm('Do You Want To Save?')" CssClass="buttonnorm" />
                            &nbsp;<asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                                OnClick="btnpreview_Click" />
                            &nbsp;<asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" Width="46px" OnClick="btnclose_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div style="height: 200px; overflow: auto;">
                                <asp:GridView ID="DGShowIssDetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="DGShowIssDetail_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:BoundField DataField="category_name" HeaderText="Category Name">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Item_name" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="issueQuantity" HeaderText="Iss Qty">
                                            <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                            </div>
                        </td>
                        <td>
                        </td>
                        <td>
                            <div style="height: 200px; overflow: auto;">
                                <asp:GridView ID="DGMain" runat="server" AutoGenerateColumns="False" DataKeyNames="PrtID"
                                    OnRowDataBound="DGMain_RowDataBound" OnRowDeleting="DGMain_RowDeleting" OnSelectedIndexChanged="DGMain_SelectedIndexChanged">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:BoundField DataField="RecCategoryName" HeaderText="Category Name">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RecItemName" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RecDescription" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RecQty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>

                                         <asp:TemplateField HeaderText="Material Type" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaterialType" Text='<%#Bind("WastedMaterial") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="BtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Delete" OnClientClick="return confirm('Do You Want To Delete?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div style="height: 250px; overflow: auto;">
                                <asp:GridView ID="GDBalanceQty" runat="server" AutoGenerateColumns="False" OnRowDataBound="GDBalanceQty_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Raw Material Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblunit" Text='<%#Bind("UnitName")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Consmp Qty." Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblconsmpqty" Text='<%#Bind("consmpqty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issued Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblalreadyeissued" Text='<%#Bind("issuedQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Return Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReturnQty" Text='<%#Bind("ReturnQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Consumed Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblConsumedQty" Text='<%#Bind("BazaarConsumeQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BalanceQty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceQty" Text='<%#Bind("BalanceQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblifinishedid" Text='<%#Bind("ifinishedid") %>' runat="server" />
                                                <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                                <asp:Label ID="lbliunitid" Text='<%#Bind("iunitid") %>' runat="server" />
                                                <asp:Label ID="lblisizeflag" Text='<%#Bind("isizeflag") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnitemfinishedid" Value="0" runat="server" />
            <asp:HiddenField ID="hngodownid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <style type="text/css">
        #mask
        {
            position: fixed;
            left: 0px;
            top: 0px;
            z-index: 4;
            opacity: 0.4;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
            filter: alpha(opacity=40); /* second!*/
            background-color: Gray;
            display: none;
            width: 100%;
            height: 100%;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ShowPopup() {
            $('#mask').show();
            $('#<%=pnlpopup.ClientID %>').show();
        }
        function HidePopup() {
            $('#mask').hide();
            $('#<%=pnlpopup.ClientID %>').hide();
        }
        $(".btnPwd").live('click', function () {
            HidePopup();
        });
    </script>
    <div id="mask">
    </div>
    <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="175px" Width="300px"
        Style="z-index: 111; background-color: White; position: absolute; left: 35%;
        top: 40%; border: outset 2px gray; padding: 5px; display: none">
        <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
            <tr style="background-color: #8B7B8B; height: 1px">
                <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                    align="center">
                    ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                        class="btnPwd" href="#">X</a>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Enter Password:
                </td>
                <td>
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnCheck" CommandName="Check" runat="server" Text="Check" CssClass="btnPwd"
                        ValidationGroup="m" OnClick="btnCheck_Click" />
                    <input type="button" value="Cancel" class="btnPwd" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
