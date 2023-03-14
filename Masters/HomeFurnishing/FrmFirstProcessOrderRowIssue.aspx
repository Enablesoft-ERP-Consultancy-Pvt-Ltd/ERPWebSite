<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="FrmFirstProcessOrderRowIssue.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_HomeFurnishing_FrmFirstProcessOrderRowIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmFirstProcessOrderRowIssue.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
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
            <div>
                <fieldset>
                    <legend>
                        <asp:Label Text="Master Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table>
                        <tr id="TRempcodescan" runat="server" visible="false">
                            <td colspan="2">
                            </td>
                            <td>
                                <asp:Label ID="Label18" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                    Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="ChKForEdit" runat="server" Text=" Check For Edit" CssClass="checkboxbold"
                                    AutoPostBack="true" OnCheckedChanged="ChKForEdit_CheckedChanged" />
                            </td>
                        </tr>
                        <tr id="Tr1" runat="server">
                            <td>
                                <asp:Label ID="lbl" Text="POrder No" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtPOrderNo" runat="server" Width="90px" AutoPostBack="True" CssClass="textb"
                                    OnTextChanged="TxtPOrderNo_TextChanged"></asp:TextBox>
                            </td>
                            <td id="Td1">
                                <asp:Label ID="Label1" Text=" Company Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddCompName" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td2">
                                <asp:Label ID="Label2" Text="  Process Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddProcessName" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td4">
                                <asp:Label ID="Label3" Text=" Party Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddempname" runat="server" Width="180px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddempname_SelectedIndexChanged" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td3">
                                <asp:Label ID="Label4" Text="  PO No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddOrderNo" runat="server" Width="150px" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddOrderNo_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td id="Td8">
                                <asp:Label ID="Label8" Text="  Rec Challan No" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDRecChallanNo" runat="server" Width="150px" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDRecChallanNo_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td id="Td7" runat="server" visible="false">
                                <asp:Label ID="Label5" Text="  Challan No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDChallanNo" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Td5">
                                <asp:Label ID="Label6" Text="   Issue Date" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtdate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>
                            </td>
                            <td id="Td6">
                                <asp:Label ID="Label7" Text="  Challan No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtchalanno" Width="100px" runat="server" OnTextChanged="txtchalan_ontextchange"
                                    AutoPostBack="True" CssClass="textb"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="height: 200px; overflow: auto;">
                <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                    Width="100%" OnRowDataBound="DG_RowDataBound">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="LblDescription" Text='<%#Bind("Description") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Con Qty">
                            <ItemTemplate>
                                <asp:Label ID="LblConsmpQty" Text='<%#Bind("ConsmpQty") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Iss Qty">
                            <ItemTemplate>
                                <asp:Label ID="LblIssueQty" Text='<%#Bind("IssueQty") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pend Qty">
                            <ItemTemplate>
                                <asp:Label ID="LblPendQty" Text='<%#Bind("PendQty") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Stock Qty">
                            <ItemTemplate>
                                <asp:Label ID="LblStockQty" Text='<%#Bind("StockQty") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Iss Qty">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtQty" Width="80px" runat="server" BackColor="Yellow" onkeypress="return isNumberKey(this);" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LblOrderID" Text='<%#Bind("OrderID") %>' runat="server" />
                                <asp:Label ID="LblOrder_FinishedID" Text='<%#Bind("Order_FinishedID") %>' runat="server" />
                                <asp:Label ID="LblOrderDetailDetail_FinishedID" Text='<%#Bind("OrderDetailDetail_FinishedID") %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <table width="100%">
             <tr id="Tr2">
                    <td colspan="3">
                       <asp:Label ID="Label9" Text="Remarks" CssClass="labelbold" runat="server" />
                       <br />
                       <asp:TextBox ID="txtremarks" CssClass="textb" Width="100%" runat="server" />
                    </td>
                    <</tr>
                <tr id="Tr7">
                    <td colspan="3">
                        <asp:Label ID="LblError" runat="server" Text="Label" CssClass="labelbold" ForeColor="Red"
                            Font-Size="Small" Visible="false"></asp:Label>
                    </td>
                    <td style="width: 45%; text-align: right;">
                        &nbsp;&nbsp;<asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click"
                            OnClientClick="return confirm('Do You Want To Save?')" CssClass="buttonnorm" />
                        <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" />
                        <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                            OnClick="btnpreview_Click" />
                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                            CssClass="buttonnorm" OnClick="btnclose_Click" />
                    </td>
                </tr>
            </table>
            <div style="max-height: 300px; overflow: auto;">
                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                    DataKeyNames="prtid" OnRowDeleting="gvdetail_RowDeleting">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <Columns>
                        <asp:TemplateField HeaderText="Catagory">
                            <ItemTemplate>
                                <asp:Label ID="lblcatgrid" Text='<%#Bind("CATEGORY_NAME") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Name">
                            <ItemTemplate>
                                <asp:Label ID="Label20" Text='<%#Bind("Item_name") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="Label21" Text='<%#Bind("Description") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty">
                            <ItemTemplate>
                                <asp:Label ID="Label22" Text='<%#Bind("Qty") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DEL" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="DEL" OnClientClick="return confirm('Do You Want To Delete?')"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="gvrow" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                </asp:GridView>
            </div>
            <asp:HiddenField ID="hnissampleorder" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
