<%@ Page Title="Weaver RawMaterial Hissab" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmWeaverRawMaterialHissabChampa.aspx.cs" Inherits="Masters_Hissab_FrmWeaverRawMaterialHissabChampa" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
     <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <%--<script src="../../Scripts/Fixfocus.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmWeaverRawMaterialHissabChampa.aspx";
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
    <div>
        <asp:UpdatePanel ID="upd2" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td id="TDEdit" runat="server" visible="true">
                                            <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="checkboxbold" runat="server"
                                                AutoPostBack="true" OnCheckedChanged="chkEdit_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Weaver/ContractorName" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDWeaverName" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDWeaverName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="tdCategoryName" runat="server" visible="false">
                                <asp:Label ID="Label4" runat="server" Text="CategoryName" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDCategory" runat="server" CssClass="dropdown" Width="300px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Master QualityName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDItemName" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="ChallanNo" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtChallanNo" Width="70px" BackColor="Yellow" runat="server" Enabled="false" />
                            </td>
                            <td id="TDChallanNo" runat="server" visible="false">
                                <asp:Label ID="Label1" runat="server" Text="ChallanNo" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDChallanNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="End Date" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                                <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label6" runat="server" Text="Item Details" CssClass="labelbold" ForeColor="Red"
                            Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 500px; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found." OnRowDataBound="DG_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <%--<asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Master QualityName" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMasterQualityName" Text='<%#Bind("MasterQuality")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" Text='<%#Bind("Item_Name")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Raw MaterialName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance StockAt Weaver">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceWeaverStock" Text='<%#Bind("Qty")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtBalanceQty" Width="70px" BackColor="Yellow" runat="server" Text='<%#Bind("WeaverBalanceStock")%>'
                                                        OnTextChanged="txtBalanceQty_TextChanged" AutoPostBack="True" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Adjust Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAdjustQty" Width="70px" BackColor="Yellow" runat="server" Enabled="false"
                                                        Text='<%#Bind("AdjustQty")%>' onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate 1">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRate" Width="70px" BackColor="Yellow" runat="server" Text='<%#Bind("Rate")%>'
                                                        onkeypress="return isNumberKey(event);" OnTextChanged="txtRate_TextChanged" AutoPostBack="True" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate 2">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRate2" Width="70px" BackColor="Yellow" runat="server" Text='<%#Bind("Rate2")%>'
                                                        onkeypress="return isNumberKey(event);" OnTextChanged="txtRate2_TextChanged"
                                                        AutoPostBack="True" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAmount" Width="70px" BackColor="Yellow" runat="server" Enabled="false"
                                                        Text='<%#Bind("Amount")%>' onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQualityId" Text='<%#Bind("QualityId") %>' runat="server" />
                                                    <asp:Label ID="lblItemId" Text='<%#Bind("ItemId") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td width="20%">
                                            <asp:Label ID="Label7" Text="Total Wool Yarn:" runat="server" CssClass="labelbold"
                                                ForeColor="Red" />
                                            <asp:Label ID="lblTotalWoolYarn" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                        </td>
                                        <td align="right" width="30%">
                                            <asp:Label ID="Label8" Text="Total Amount" runat="server" CssClass="labelbold" ForeColor="black" />
                                        </td>
                                        <td width="50%" align="left">
                                            <asp:TextBox ID="txtTotalAmount" Width="100px" BackColor="Yellow" runat="server"
                                                Enabled="false" onkeypress="return isNumberKey(event);" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                             <asp:Label ID="Label15" Text="Total Cotton:" runat="server" CssClass="labelbold"
                                                ForeColor="Red" />
                                            <asp:Label ID="lblTotalCotton" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label13" Text="Total Commission BetweenSelectedDate" runat="server"
                                                CssClass="labelbold" ForeColor="black" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCommAmt" Width="100px" BackColor="Yellow" runat="server" OnTextChanged="txtCommAmt_TextChanged"
                                                AutoPostBack="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" Text="Total Tar:" runat="server" CssClass="labelbold"
                                                ForeColor="Red" />
                                            <asp:Label ID="lblTotalTar" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label11" Text="Map+Stencil+Extra" runat="server" CssClass="labelbold"
                                                ForeColor="black" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMapStencilExtra" Width="100px" BackColor="Yellow" runat="server"
                                                OnTextChanged="txtMapStencilExtra_TextChanged" AutoPostBack="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label17" Text="Total Tharri:" runat="server" CssClass="labelbold"
                                                ForeColor="Red" />
                                            <asp:Label ID="lblTotalTharri" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label12" Text="Any Other" runat="server" CssClass="labelbold" ForeColor="black" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAnyOther" Width="100px" BackColor="Yellow" runat="server" OnTextChanged="txtAnyOther_TextChanged"
                                                AutoPostBack="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                           <asp:Label ID="Label18" Text="Total Misc:" runat="server" CssClass="labelbold"
                                                ForeColor="Red" />
                                            <asp:Label ID="lblTotalMisc" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label14" Text="Net Amount" runat="server" CssClass="labelbold" ForeColor="black" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNetAmount" Width="100px" BackColor="Yellow" runat="server" Enabled="false"
                                                onkeypress="return isNumberKey(event);" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hnChallanNo" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
