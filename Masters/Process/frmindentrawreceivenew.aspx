<%@ Page Title="INDENT RECEIVE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmindentrawreceivenew.aspx.cs" Inherits="Masters_Process_frmindentrawreceivenew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
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
            window.location.href = "frmindentrawreceivenew.aspx";
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;

            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;
                    }
                    else {
                        inputlist[i].checked = false;

                    }
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="leg1" Text="Master Detail" ForeColor="Red" CssClass="labelbold" runat="server" />
                    </legend>
                    <table>
                        <tr>
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="chkedit" Text="For Edit" runat="server" AutoPostBack="true" CssClass="checkboxbold"
                                    OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl" Text="Company Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddCompName" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td2" class="tdstyle">
                                <asp:Label ID="Label3" Text=" Process Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddProcessName" runat="server" Width="150px" AutoPostBack="True"
                                    CssClass="dropdown" OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" Text="  Party Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddempname" runat="server" Width="150px" TabIndex="4" AutoPostBack="True"
                                    CssClass="dropdown" OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDChallanNo" runat="server" visible="false">
                                <asp:Label ID="Label7" runat="server" Text="Challan No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDChallanNo" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblindent" runat="server" Text="Indent No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="ddindentno" runat="server" Width="150px" TabIndex="3" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddindentno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Rec. Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtrecdate" runat="server" Width="100px" CssClass="textb" BackColor="Beige"></asp:TextBox>
                                <asp:CalendarExtender ID="caldate" TargetControlID="txtrecdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Gate In No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtgateinno" runat="server" Width="100px" CssClass="textb" BackColor="Beige"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label6" Text="Party Challan No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtchalanno" Width="100px" runat="server" TabIndex="6" CssClass="textb"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="Label2" runat="server" Text="Remark" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtmsterremark" runat="server" TextMode="MultiLine" Width="290px"
                                    CssClass="textb" BackColor="Beige" Height="40px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <fieldset>
                <legend>
                    <asp:Label ID="Label9" Text="Filter By" ForeColor="Red" CssClass="labelbold" runat="server" />
                </legend>
                <table>
                    <tr>
                        <td id="TDCustCode" runat="server" class="tdstyle">
                            <asp:Label ID="Label10" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDOrderNo" runat="server" class="tdstyle">
                            <asp:Label ID="Label11" runat="server" Text=" OrderNo" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDOrderNo" Width="140" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="Tr3" runat="server">
                        <td>
                            <asp:Label ID="lblcategoryname" runat="server" class="tdstyle" Text="Category Name"
                                CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddCatagory" runat="server" Width="130px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dditemname" runat="server" Width="130px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDQuality" runat="server" visible="false">
                            <asp:Label ID="lblqualityname" class="tdstyle" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dquality" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDDesign" runat="server" visible="false" class="style5">
                            <asp:Label ID="lbldesignname" runat="server" class="tdstyle" Text="Design" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDColor" runat="server" visible="false">
                            <asp:Label ID="lblcolorname" runat="server" class="tdstyle" Text="Color" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDShape" runat="server" visible="false">
                            <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="100px" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDSize" runat="server" class="tdstyle" visible="false">
                            <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="100px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td valign="bottom">
                            <asp:Button ID="btngetdata" Text="Get Data" runat="server" CssClass="buttonnorm"
                                OnClick="btngetdata_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>
                    <asp:Label ID="Label8" Text="Receive Detail" ForeColor="Red" CssClass="labelbold"
                        runat="server" />
                </legend>
                <div>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="overflow: auto; max-height: 500px; max-width: 100%">
                                    <asp:GridView ID="DGRecDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No Records Found.." OnRowDataBound="DGRecDetail_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkall" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkitem" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblindentno" runat="server" Text='<%#Bind("IndentNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderno" runat="server" Text='<%#Bind("customerorderno") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderdescription" runat="server" Text='<%#Bind("Orderdescription") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblissuedescription" runat="server" Text='<%#Bind("ItemDescription") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblunitname" runat="server" Text='<%#Bind("unitname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="60px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentQty" runat="server" Text='<%#Bind("IndentQty") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pre Rec.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpreRec" runat="server" Text='<%#bind("PreRec") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pending Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpendingqty" runat="server" Text='<%# Convert.ToDouble(Eval("IndentQty")) - Convert.ToDouble(Eval("PreRec"))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loss%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllossperc" runat="server" Text='<%#Bind("losspercent")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Godown">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDgodown" CssClass="dropdown" Width="130px" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LotNo./BatchNo.">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtlotno" runat="server" Width="100px" Text='<%#Bind("Lotno") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Receive Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrecqty" runat="server" Width="60px" BackColor="Yellow" AutoPostBack="true"
                                                        onkeypress="return isNumberKey(event);" OnTextChanged="txtrecqty_TextChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loss Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtlossQty" runat="server" Width="45px" BackColor="Yellow" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrate" runat="server" Width="45px" BackColor="Yellow" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Remark">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtremark" runat="server" Width="130px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderdetailid" Text='<%#Bind("orderdetailid") %>' runat="server" />
                                                    <asp:Label ID="lblIndentDetailid" Text='<%#Bind("Indentdetailid") %>' runat="server" />
                                                    <asp:Label ID="lblofinishedid" Text='<%#Bind("Ofinishedid") %>' runat="server" />
                                                    <asp:Label ID="lblunitid" Text='<%#Bind("Unitid") %>' runat="server" />
                                                    <asp:Label ID="lblsizeflag" Text='<%#Bind("flagsize") %>' runat="server" />
                                                    <asp:Label ID="lbllosspercentage" Text='<%#Bind("losspercent") %>' runat="server" />
                                                    <asp:Label ID="lblindentid" Text='<%#Bind("indentid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew();" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" ValidationGroup="f1"
                                Width="50px" OnClick="BtnSave_Click" />
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
            <div id="Div1" runat="server" style="overflow: auto; max-height: 300px; width: 100%">
                <asp:GridView ID="DGSavedetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                    EmptyDataText="No Records Found.." OnRowCancelingEdit="DGSavedetail_RowCancelingEdit"
                    OnRowEditing="DGSavedetail_RowEditing" OnRowDeleting="DGSavedetail_RowDeleting"
                    OnRowUpdating="DGSavedetail_RowUpdating">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:TemplateField HeaderText="Indent No.">
                            <ItemTemplate>
                                <asp:Label ID="lblindentno" runat="server" Text='<%#Bind("IndentNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order No.">
                            <ItemTemplate>
                                <asp:Label ID="lblorderno" runat="server" Text='<%#Bind("customerorderno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order Description">
                            <ItemTemplate>
                                <asp:Label ID="lblorderdescription" runat="server" Text='<%#Bind("Orderdescription") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Description">
                            <ItemTemplate>
                                <asp:Label ID="lblRecdescription" runat="server" Text='<%#Bind("ItemDescription") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit">
                            <ItemTemplate>
                                <asp:Label ID="lblunitname" runat="server" Text='<%#Bind("unitname") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Received Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="txteditRecqty" runat="server" Width="70px" BackColor="Yellow" Text='<%#Bind("RecQuantity") %>'
                                    onkeypress="return isNumberKey(event);" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRecQty" runat="server" Text='<%#Bind("RecQuantity") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Loss Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="txteditLossqty" runat="server" Width="70px" BackColor="Yellow" Text='<%#Bind("Lossqty") %>'
                                    onkeypress="return isNumberKey(event);" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLossQty" runat="server" Text='<%#Bind("LossQty") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark">
                            <EditItemTemplate>
                                <asp:TextBox ID="txteditremark" Text='<%#Bind("Remark") %>' BackColor="Yellow" runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblremark" runat="server" Text='<%#Bind("remark") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblprtid" Text='<%#Bind("prtid") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="Del" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
