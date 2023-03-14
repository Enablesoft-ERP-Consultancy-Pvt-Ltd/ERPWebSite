<%@ Page Title="Material issue stock no wise" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmmaterialissuestocknowise.aspx.cs" Inherits="Masters_RawMaterial_frmmaterialissuestocknowise" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmmaterialissuestocknowise.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnStockNo.ClientID %>').click();
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <fieldset>
                <legend>
                    <asp:Label ID="Label1" Text=".." CssClass="labelbold" Font-Size="Small" runat="server" />
                </legend>
                <table>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                        <td id="Tdedit" runat="server" visible="false">
                            <asp:CheckBox ID="chkedit" Font-Size="Small" Text="For Edit" CssClass="checkboxbold"
                                AutoPostBack="true" runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Enter Stock No./Scan" CssClass="labelbold" Font-Size="Small" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtstockno" CssClass="textb" Height="40px" Width="250px" runat="server"
                                onKeypress="KeyDownHandler(event);" />
                            <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="txtstockno_TextChanged" />
                        </td>
                        <td id="Tdissueno" runat="server" visible="false">
                            <asp:Label Text="Issue No." CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDissueno" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblmsg" Text="" CssClass="labelbold" Font-Size="Small" runat="server"
                                ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>
                    <asp:Label ID="Label6" runat="server" Text="Raw Material Details" CssClass="labelbold"
                        ForeColor="Red" Font-Bold="true"></asp:Label></legend>
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
                                        <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Raw Material Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblunit" Text='<%#Bind("UnitName")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Godown">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="150px" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDgodown_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BinNo">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DDBinNo" CssClass="dropdown" Width="150px" runat="server" OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lot No.">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DDLotNo" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="DDLotno_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tag No.">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DDTagNo" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="DDTagno_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstockqty" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Consmp Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblconsmpqty" Text='<%#Bind("consmpqty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Already Issued">
                                            <ItemTemplate>
                                                <asp:Label ID="lblalreadyeissued" Text='<%#Bind("issuedQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BalanceQty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceQty" Text='<%# System.Math.Round(Convert.ToDouble(Eval("consmpqty")) -Convert.ToDouble(Eval("issuedQty")),3) %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issue Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtissueqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="No of Cone">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtnoofcone" Width="50px" BackColor="Yellow" runat="server" 
                                                    onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblifinishedid" Text='<%#Bind("ifinishedid") %>' runat="server" />
                                                <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                                <asp:Label ID="lbliunitid" Text='<%#Bind("iunitid") %>' runat="server" />
                                                <asp:Label ID="lblisizeflag" Text='<%#Bind("isizeflag") %>' runat="server" />
                                                <asp:Label ID="lblstockno" Text='<%#Bind("stockno") %>' runat="server" />
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
                        <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click" />
                        <asp:Button ID="btnpreview" Text="Preview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                        <asp:Button ID="btnnew" Text="New" CssClass="buttonnorm" runat="server" OnClientClick="return NewForm();" />
                        <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
            <fieldset>
                <legend><span class="labelbold" style="color: Red">Issued Details</span> </legend>
                <div style="max-height: 300px; overflow: auto;">
                    <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No records found.."
                        OnRowDeleting="gvdetail_RowDeleting">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <Columns>
                            <asp:TemplateField HeaderText="Item Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LotNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblLotNo" Text='<%#Bind("LotNo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TagNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblTagNo" Text='<%#Bind("TagNo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtqty" Text='<%#Bind("IssueQuantity") %>' Width="70px" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblissueqty" Text='<%#Bind("IssueQuantity") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblhqty" Text='<%#Bind("IssueQuantity") %>' runat="server" />
                                    <asp:Label ID="lblprmid" Text='<%#Bind("prmid") %>' runat="server" />
                                    <asp:Label ID="lblprtid" Text='<%#Bind("prtid") %>' runat="server" />
                                    <asp:Label ID="lblprorderid" Text='<%#Bind("prorderid") %>' runat="server" />
                                    <asp:Label ID="lblprocessid" Text='<%#Bind("processid") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </fieldset>
            <asp:HiddenField ID="hncompanyid" runat="server" Value="0" />
            <asp:HiddenField ID="hngodownid" runat="server" Value="0" />
            <asp:HiddenField ID="hnissueid" runat="server" Value="0" />
            <asp:HiddenField ID="hnstockno" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
