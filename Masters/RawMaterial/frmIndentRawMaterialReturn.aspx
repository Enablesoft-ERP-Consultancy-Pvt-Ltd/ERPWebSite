<%@ Page Title="Indent Raw Return" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmIndentRawMaterialReturn.aspx.cs" Inherits="Masters_RawMaterial_frmIndentRawMaterialReturn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function NewForm() {
            window.location.href = "frmIndentRawMaterialReturn.aspx"
        }

        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('Please Enter numeric value only');
                return false;
            }
            else {
                return true;
            }
        }
        function ValidationSave(e, txt) {

            if (document.getElementById('CPH_Form_DDCompany') != null) {
                if (document.getElementById('CPH_Form_DDCompany').options.length == 0) {
                    alert("Company name must have a value....!");
                    document.getElementById("CPH_Form_DDCompany").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDCompany').options[document.getElementById('CPH_Form_DDCompany').selectedIndex].value == 0) {
                    alert("Please select company name ....!");
                    document.getElementById("CPH_Form_DDCompany").focus();
                    return false;
                }
            }

            if (document.getElementById('CPH_Form_DDPartyName') != null) {
                if (document.getElementById('CPH_Form_DDPartyName').options.length == 0) {
                    alert("Party name must have a value....!");
                    document.getElementById("CPH_Form_DDPartyName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDPartyName').options[document.getElementById('CPH_Form_DDPartyName').selectedIndex].value == 0) {
                    alert("Please select Party name ....!");
                    document.getElementById("CPH_Form_DDPartyName").focus();
                    return false;
                }
            }

            if (document.getElementById('CPH_Form_DDChallanNo') != null) {
                if (document.getElementById('CPH_Form_DDChallanNo').options.length == 0) {
                    alert("ChallanNo name must have a value....!");
                    document.getElementById("CPH_Form_DDChallanNo").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDChallanNo').options[document.getElementById('CPH_Form_DDChallanNo').selectedIndex].value == 0) {
                    alert("Please select Challan No ....!");
                    document.getElementById("CPH_Form_DDChallanNo").focus();
                    return false;
                }
                var gvcheck = document.getElementById('CPH_Form_DGItemDetail');
                var rowindex = txt.offsetParent.parentNode.rowIndex;
                var inputs = gvcheck.rows[rowindex].cells[7].children[0].value;
                if (inputs == "0") {
                    alert("Return Quantity can not be zero !");
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDGatePass') != null) {
                if (document.getElementById('CPH_Form_DDGatePass').options.length == 0) {
                    alert("Gate Pass No. must have a value....!");
                    document.getElementById("CPH_Form_DDGatePass").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDGatePass').options[document.getElementById('CPH_Form_DDGatePass').selectedIndex].value == 0) {
                    alert("Please select Gate Pass No. ....!");
                    document.getElementById("CPH_Form_DDGatePass").focus();
                    return false;
                }
            }


            return confirm('Do You Want To Save?')
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div id="main" runat="server" style="height: 400px; width: 800px">
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="CHKEdit" runat="server" Text=" Edit" AutoPostBack="true" OnCheckedChanged="CHKEdit_CheckedChanged"
                                CssClass="checkboxbold" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbl" Text="CompanyName" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDCompany" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompany"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="Process" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddProcessName" runat="server" Width="170px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddProcessName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" Text="PartyName" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDPartyName" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label3" Text="ChallanNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDChallanNo" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" runat="server" id="tdGatepassDD" visible="false">
                            <asp:Label ID="Label4" Text="GatePassNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDGatePass" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDGatePass_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" runat="server" id="tdGTPassTxt">
                            <asp:Label ID="Label5" Text="GatePassNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtGatepassno" ReadOnly="true" runat="server" CssClass="textb" Width="75px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label6" Text=" Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtDate" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                            <asp:CalendarExtender ID="extender1" runat="server" TargetControlID="TxtDate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="8">
                            <div style="max-height: 250px; overflow: auto; width: 750">
                                <asp:GridView ID="DGItemDetail" runat="server" Width="100%" AutoGenerateColumns="False"
                                    DataKeyNames="DetailID" CssClass="grid-views" OnRowCommand="DGItemDetail_RowCommand"
                                    OnRowDataBound="DGItemDetail_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="Item_Name" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemDescription" HeaderText="Description" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GodownName" HeaderText="GodownName" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LotNo" HeaderText="LotNo" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                         <asp:TemplateField HeaderText="TagNo" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltagno" Text='<%#Bind("TagNo") %>' runat="server" />
                                                </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RecQuantity" HeaderText="RecQty" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BalQty" HeaderText="Bal.Qty" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="IndentNo" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblIndentID" runat="server" Text='<%# Bind("IndentID") %>' Width="80px"></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ReturnQty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtReturnQty" runat="server" Text='<%# Bind("ReturnQty") %>' Width="70px"
                                                    onkeypress="return isNumber(event);"></asp:TextBox>
                                                <asp:Label ID="lblPRMID" runat="server" Text='<%# Bind("PRMID") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblPRTID" runat="server" Text='<%# Bind("PRTID") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblFinishedid" runat="server" Text='<%# Bind("Finishedid") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblGodownid" runat="server" Text='<%# Bind("Godownid") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblPartyId" runat="server" Text='<%# Bind("EmpId") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblUnitid" runat="server" Text='<%# Bind("Unitid") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txtremark" runat="server" Text='<%# Bind("Remark") %>' Width="250px"></asp:TextBox></ItemTemplate>
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Button ID="btnSave" CssClass="buttonnorm" Width="50px" Text="Save" runat="server"
                                                    CommandName="save" OnClientClick="return ValidationSave(event,this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="BtnPreview" runat="server" Text="Return Gatepass" CssClass="buttonnorm "
                                OnClick="BtnPreview_Click" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>

                    <tr id="TRGVIndentRawReturn" runat="server" visible="false">
                    
                    <td style="width: 50%" valign="top">
                        <div style="max-height: 300px; overflow: auto;">
                            <asp:GridView ID="GVIndentReturnDetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="GVIndentReturnDetail_RowDataBound"
                                DataKeyNames="DetailID"  OnRowDeleting="GVIndentReturnDetail_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>                                   
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Label20" Text='<%#Bind("Item_name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="Label21" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Godown Name.">
                                        <ItemTemplate>
                                            <asp:Label ID="Label23" Text='<%#Bind("GodownName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Lot No.">
                                        <ItemTemplate>
                                            <asp:Label ID="Label25" Text='<%#Bind("Lotno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag No.">
                                        <ItemTemplate>
                                            <asp:Label ID="Label24" Text='<%#Bind("Tagno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Return Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="Label22" Text='<%#Bind("ReturnQty") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>     
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="Label26" Text='<%#Bind("Remarks") %>' runat="server" />
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
                    </td>
                    
                    
                    </tr>

                </table>
            </div>
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

