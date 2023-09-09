<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPNMRowIssueDetail.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_Rawmaterial_FrmPNMRowIssueDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmPNMRowIssueDetail.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function validate() {
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Plz select Comapny Name");
                return false;
            }

            if (document.getElementById('CPH_Form_ddempname') != null) {
                if (document.getElementById('CPH_Form_ddempname').options.length == 0) {
                    alert("PartyName must have a value....!");
                    document.getElementById("CPH_Form_ddempname").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddempname').options[document.getElementById('CPH_Form_ddempname').selectedIndex].value == 0) {
                    alert("Please select PartyName....!");
                    document.getElementById("CPH_Form_ddempname").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddindentno') != null) {
                if (document.getElementById('CPH_Form_ddindentno').options.length == 0) {
                    alert("Indent No.  must have a value....!");
                    document.getElementById("CPH_Form_ddindentno").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddindentno').options[document.getElementById('CPH_Form_ddindentno').selectedIndex].value == 0) {
                    alert("Please select Indent No....!");
                    document.getElementById("CPH_Form_ddindentno").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=DDChallanNo.ClientID %>").value <= "0") {
                alert("Plz select Challan No.");
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label Text="Master Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <asp:Panel ID="pnl1" runat="server">
                        <table>
                        </table>
                        <table>
                            <tr id="Tr1" runat="server">
                                <td id="Td1" class="tdstyle">
                                    <asp:Label ID="Label3" Text="Company Name" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:DropDownList ID="ddCompName" runat="server" Width="150px" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td class="tdstyle">
                                    <asp:Label ID="Label33" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td id="Td2" class="tdstyle">
                                    <asp:Label ID="Label4" Text="Process Name" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:DropDownList ID="ddProcessName" runat="server" Width="150px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="Td3" align="center" class="tdstyle">
                                    <asp:Label ID="Label6" Text="Champo Indent No" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:DropDownList ID="ddChampoIndentNo" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddChampoIndentNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="Td6" class="tdstyle">
                                    <asp:Label ID="Label8" Text="Champo Challan No" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:DropDownList ID="DDChallanNo" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="Td4" align="center" class="tdstyle">
                                    <asp:Label ID="Label5" Text="  Issue No" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtIssueNo" runat="server" CssClass="textb" Width="100px" AutoPostBack="True"></asp:TextBox>
                                </td>
                                <td id="Td5" align="center" class="tdstyle">
                                    <asp:Label ID="Label7" Text="  Issue Date" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtdate" runat="server" CssClass="textb" Width="100px" AutoPostBack="True"></asp:TextBox>
                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                        ErrorMessage="please Enter Date" ControlToValidate="txtdate" ValidationGroup="f1"
                                        ForeColor="Red">*</asp:RequiredFieldValidator>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtdate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <br />
                                    <asp:TextBox ID="TxtProdCode" CssClass="textb" runat="server" Visible="false" OnTextChanged="TxtProdCode_TextChanged"
                                        AutoPostBack="True" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label28" Text="Item Details" CssClass="labelbold" ForeColor="Red"
                            runat="server" /></legend>
                    <table>
                        <tr id="Tr3" runat="server" class="tdstyle">
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" Text="Ctagory Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged"
                                    AutoPostBack="True" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dditemname" runat="server" Width="150px" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                                    AutoPostBack="True" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="ql" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dquality" runat="server" Width="150px" AutoPostBack="True"
                                    CssClass="dropdown" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="dsn" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="clr" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="shp" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="sz" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="shd" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddlshade" runat="server" Width="150px" AutoPostBack="true"
                                    CssClass="dropdown" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label17" Text="  Unit" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddlunit" runat="server" AutoPostBack="true" CssClass="dropdown"
                                    Width="88px">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label9" Text=" Godown Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddgodown" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddgodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDLotNo" runat="server" class="tdstyle">
                                <asp:Label ID="Label13" Text="  LotNo." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddlotno" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddlotno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDTagNo" runat="server" visible="false">
                                <asp:Label ID="Label19" Text="Tag No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDTagNo" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBinNo" runat="server" visible="false">
                                <asp:Label ID="Label20" Text="Bin No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDBinNo" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" Text="Stock Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtStockQty" runat="server" Enabled="false" CssClass="textb" Width="80px"
                                    Style="background-color: #F5F5DC"></asp:TextBox>
                            </td>
                            <td id="TDtxtpendingqty" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label12" Text=" Pending Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtpendingqty" runat="server" Enabled="false" CssClass="textb" Width="80px"
                                    Style="background-color: #F5F5DC"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label15" Text="  Iss Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtissqty" runat="server" CssClass="textb" Width="80px" Style="background-color: #F5F5DC"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table width="100%">
                    <tr>
                        <td style="width: 60%" id="TDtxtremarks" runat="server">
                            <asp:Label ID="Label18" Text=" Remarks" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtremarks" runat="server" Width="90%" CssClass="textb"></asp:TextBox>
                        </td>
                        <td style="width: 40%; text-align: center">
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return validate(); "
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnpriview" runat="server" Text="Preview" CssClass="buttonnorm"
                                OnClick="btnpriview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                CssClass="buttonnorm" OnClick="btnclose_Click" />
                            <%-- <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" Visible="false"
                                Enabled="False" />--%>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="Label1" runat="server" Text="Label" CssClass="labelbold" Font-Size="Small"
                                ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <div style="width: 95%; max-height: 250px; overflow: auto">
                                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                    DataKeyNames="prtid" CssClass="grid-views" OnRowDeleting="gvdetail_RowDeleting"
                                    Width="100%">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="PRTid" HeaderText="Sr.No." Visible="False" />
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Label21" Text='<%#Bind("ITEM_NAME") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="Label23" Text='<%#Bind("DESCRIPTION") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Godown Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Label22" Text='<%#Bind("GodownName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lot No.">
                                            <ItemTemplate>
                                                <asp:Label ID="Label24" Text='<%#Bind("Lotno") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tag No.">
                                            <ItemTemplate>
                                                <asp:Label ID="Label25" Text='<%#Bind("TagNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bin No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbinnogrid" Text='<%#Bind("BinNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="Label26" Text='<%#Bind("Qty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do You Want To Deleted Data ?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        </asp:TemplateField>--%>
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
