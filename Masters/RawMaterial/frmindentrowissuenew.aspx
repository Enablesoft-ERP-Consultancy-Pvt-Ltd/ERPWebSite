<%@ Page Title="INDENT RAW ISSUE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmindentrowissuenew.aspx.cs" Inherits="Masters_RawMaterial_frmindentrowissuenew" %>

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
            window.location.href = "frmindentrowissuenew.aspx";
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
                    var selectedindex = $("#<%=ddCompName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=ddProcessName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Process !!\n";
                    }
                    selectedindex = $("#<%=ddempname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Party !!\n";
                    }
                    selectedindex = $("#<%=ddindentno.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Indent No. !!\n";
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
    <asp:UpdatePanel ID="upd1" runat="server">
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
                            <td>
                                <asp:Label ID="lblindent" runat="server" Text="Indent No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="ddindentno" runat="server" Width="150px" TabIndex="3" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddindentno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDChallanNo" runat="server" visible="false">
                                <asp:Label ID="Label7" runat="server" Text="Challan No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDChallanNo" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtdate" runat="server" TabIndex="5" Width="100px" CssClass="textb"
                                    BackColor="Beige"></asp:TextBox>
                                <asp:CalendarExtender ID="caldate" TargetControlID="txtdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label6" Text="Challan No." runat="server" CssClass="labelbold" />
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
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label5" Text="Issue Detail" ForeColor="Red" CssClass="labelbold" runat="server" />
                    </legend>
                    <div>
                        <div id="gride" runat="server" style="overflow: auto; max-height: 500px">
                            <asp:GridView ID="DGIssueDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                EmptyDataText="No Records Found.." OnRowDataBound="DGIssueDetail_RowDataBound">
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
                                            <asp:Label ID="lblissuedescription" runat="server" Text='<%#Bind("ItemDescription") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblunitname" runat="server" Text='<%#Bind("unitname") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltotalqty" runat="server" Text='<%#Bind("IQty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pre Issue">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreissue" runat="server" Text='<%#bind("PreIssue") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pending Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpendingqty" runat="server" Text='<%# Convert.ToDouble(Eval("IQty")) - Convert.ToDouble(Eval("PreIssue"))%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Godown">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DDgodown" CssClass="dropdown" Width="130px" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDgodown_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LotNo./BatchNo.">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DDlotno" CssClass="dropdown" Width="130px" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="DDLotno_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstockqty" runat="server" Width="60px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Issue Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtissQty" runat="server" Width="80px" BackColor="Yellow" AutoPostBack="true"
                                                OnTextChanged="txtiss_TextChanged" onkeypress="return isNumberKey(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtremark" runat="server" Width="150px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblifinishedid" Text='<%#Bind("IFinishedid") %>' runat="server" />
                                            <asp:Label ID="lblorderdetailid" Text='<%#Bind("orderdetailid") %>' runat="server" />
                                            <asp:Label ID="lblofinishedid" Text='<%#Bind("Ofinishedid") %>' runat="server" />
                                            <asp:Label ID="lblunitid" Text='<%#Bind("Iunitid") %>' runat="server" />
                                            <asp:Label ID="lblsizeflag" Text='<%#Bind("Isizeflag") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
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
            <div id="Div1" runat="server" style="overflow: auto; max-height: 300px; width: 100%">
                <asp:GridView ID="DGSavedetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                    EmptyDataText="No Records Found.." OnRowEditing="DGSavedetail_RowEditing" OnRowCancelingEdit="DGSavedetail_RowCancelingEdit"
                    OnRowUpdating="DGSavedetail_RowUpdating" OnRowDeleting="DGSavedetail_RowDeleting">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
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
                                <asp:Label ID="lblissuedescription" runat="server" Text='<%#Bind("ItemDescription") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit">
                            <ItemTemplate>
                                <asp:Label ID="lblunitname" runat="server" Text='<%#Bind("unitname") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Issued Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="txteditissqty" runat="server" Width="70px" BackColor="Yellow" Text='<%#Bind("IssueQuantity") %>'
                                    onkeypress="return isNumberKey(event);" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblissueQty" runat="server" Text='<%#Bind("IssueQuantity") %>'></asp:Label>
                            </ItemTemplate>
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
