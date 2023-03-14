<%@ Page Title="CHALLAN FOR LOCAL CUSTOMER" Language="C#" AutoEventWireup="true"
    CodeFile="frmChallanForLocalCustomer.aspx.cs" Inherits="Masters_Packing_frmChallanForLocalCustomer"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function reloadPage() {
            window.location.href = "frmChallanForLocalCustomer.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function textStockNoValidate() {
            if (document.getElementById("CPH_Form_txtChallanNo").value == "") {
                document.getElementById("CPH_Form_txtChallanNo").focus();
                alert('Challan No. can not be blank');
                return false;
            }
        }
        function validate(btn) {
            var row = btn.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var str = "CPH_Form_GVStockDetail_btnSave_" + rowIndex;
            var txt = document.getElementById("CPH_Form_GVStockDetail_txtPackQty_" + rowIndex + "");

            if (document.getElementById("CPH_Form_txtChallanNo").value == "") {
                alert('Please Enter Challan No...');
                document.getElementById("CPH_Form_txtChallanNo").focus();
                return false;
            }
            if (txt.value == "0" || txt.value == "") {

                alert('Pack Qty can not be Zero or Blank...');
                document.getElementById("CPH_Form_GVStockDetail_txtPackQty_" + rowIndex + "").focus();
                return false;
            }
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
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div style="height: auto">
                <div style="height: auto">
                    <table>
                        <tr>
                            <td>
                                Company Name<br />
                                <asp:DropDownList ID="DDCompanyName" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Customer Code<br />
                                <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" AutoPostBack="true"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Order No.<br />
                                <asp:DropDownList ID="DDOrderNo" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Challan No.<br />
                                <asp:TextBox ID="txtChallanNo" runat="server" Width="100px" CssClass="textb" BackColor="Yellow">
                                </asp:TextBox>
                            </td>
                            <td>
                                Challan Date<br />
                                <asp:TextBox ID="txtChallanDate" runat="server" Width="100px" CssClass="textb">
                                </asp:TextBox>
                                <asp:CalendarExtender ID="cal1ChallanDate" runat="server" TargetControlID="txtChallanDate"
                                    Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblitemDetail" ForeColor="Red" Font-Bold="true" Text="Item Details.."
                                runat="server"></asp:Label></legend>
                        <table>
                            <tr>
                                <td id="TDProdcode" runat="server" visible="false">
                                    Product Code
                                    <asp:TextBox ID="txtProdCode" runat="server" Width="75px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblCategory" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDCategory" runat="server" Width="150px" CssClass="dropdown"
                                        OnSelectedIndexChanged="DDCategory_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblItemName" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDitemName" runat="server" Width="150px" CssClass="dropdown"
                                        OnSelectedIndexChanged="DDitemName_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDQuality" runat="server" visible="false">
                                    <asp:Label ID="lblQuality" runat="server" Text="Quality Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDQuality" runat="server" Width="150px" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDDesign" runat="server" visible="false">
                                    <asp:Label ID="lblDesign" runat="server" Text="Design Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDDesign" runat="server" Width="150px" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td id="TDColor" runat="server" visible="false">
                                    <asp:Label ID="lblColor" runat="server" Text="Color Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDColor" runat="server" Width="150px" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDShape" runat="server" visible="false">
                                    <asp:Label ID="lblShape" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDShape" runat="server" Width="150px" AutoPostBack="true" CssClass="dropdown"
                                        OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDSize" runat="server" visible="false">
                                    <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="DDSizetype" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        Width="90px" OnSelectedIndexChanged="DDSizetype_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:DropDownList ID="DDSize" runat="server" Width="150px" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDShadeColor" runat="server" visible="false">
                                    <asp:Label ID="lblShade" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label>
                                    <asp:DropDownList ID="DDshade" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <br />
                                    <asp:Button ID="btnShowDetail" runat="server" Text="Show Details" CssClass="buttonnorm"
                                        Width="100px" OnClick="btnShowDetail_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div style="width: 700px; height: 25px">
                    <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <div style="width: 800px; margin: 0px auto; height: 220px">
                    <div style="float: left; width: 300px; margin-top: 20px">
                        <table>
                            <tr>
                                <td>
                                    <b>Enter Stock No.</b><br />
                                    <asp:TextBox ID="txtStockNo" runat="server" Width="150px" CssClass="textb" Height="25px"
                                        AutoPostBack="true" onclick="return textStockNoValidate();" OnTextChanged="txtStockNo_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: left; width: 450px; overflow: auto; height: 200px">
                        <asp:GridView ID="GVStockDetail" runat="server" AutoGenerateColumns="False" OnRowCommand="GVStockDetail_RowCommand">
                            <HeaderStyle CssClass="gvheader" Height="20px" />
                            <AlternatingRowStyle CssClass="gvalt" />
                            <RowStyle CssClass="gvrow" />
                            <Columns>
                                <asp:BoundField DataField="Category_Name" HeaderText="CategoryName">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ItemDescription" HeaderText="Item Description">
                                    <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StockQty" HeaderText="Available Stock">
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Enter Pack Qty">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPackQty" runat="server" Width="75px" BackColor="Yellow" Style="text-align: center"
                                            onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblitemFinishedid" runat="server" Text='<%#Bind("Item_Finished_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnSave" runat="server" Text="PACK" CssClass="buttonnorm" Width="60px"
                                            CommandName="Save" OnClientClick="return validate(this);" CausesValidation="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div style="width: 800px; height: 270px">
                    <table style="width: 750px;">
                        <tr>
                            <td colspan="5">
                                <div style="width: 500px; float: left; height: 250px; overflow: auto">
                                    <asp:GridView ID="GvChallanDetail" Width="500px" runat="server" AutoGenerateColumns="False"
                                        DataKeyNames="ChallanDetailId">
                                        <HeaderStyle CssClass="gvheader" Height="20px" />
                                        <AlternatingRowStyle CssClass="gvalt" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:BoundField DataField="Category_Name" HeaderText="CategoryName">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ItemDescription" HeaderText="Item Description">
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TStockNo" HeaderText="Stock No.">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="100px" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanId" runat="server" Text='<%#Bind("ChallanId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanDetailId" runat="server" Text='<%#Bind("ChallanDetailId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                            <td>
                                <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="buttonnorm" Width="50px"
                                    OnClientClick="return CloseForm();" />
                            </td>
                            <td>
                                <asp:Button ID="btnNew" Text="New" runat="server" CssClass="buttonnorm" Width="50px"
                                    OnClientClick="return reloadPage();" />
                            </td>
                            <td>
                                <asp:Button ID="btnPreview" Text="Preview" runat="server" CssClass="buttonnorm" Width="70px"
                                    Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table>
                        <tr>
                            <td>
                                <b>Total Packed Qty.</b><asp:TextBox ID="txttotalPackQty" runat="server" CssClass="textb"
                                    Enabled="false" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:HiddenField ID="hnchallanId" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
