<%@ Page Title="Material Gate In/Out" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" EnableEventValidation="false" CodeFile="FrmGateInOutMaterial.aspx.cs"
    Inherits="Masters_RawMaterial_FrmGateInOutMaterial" %>

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
        function ClickNew() {
            window.location.href = "FrmGateInOutMaterial.aspx";
        }
        function Validation() {
            if (document.getElementById("<%=txtdate.ClientID %>").value == "") {
                alert("Pls Select Date");
                document.getElementById("<%=txtdate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtPartyName.ClientID %>").value == "") {
                alert("Pls fill party name");
                document.getElementById("<%=txtPartyName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtQty.ClientID %>").value == "") {
                alert("Pls fill rec qty");
                document.getElementById("<%=TxtQty.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtUnit.ClientID %>").value == "") {
                alert("Pls fill unit");
                document.getElementById("<%=txtUnit.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtMaterialDescription.ClientID %>").value == "") {
                alert("Pls fill material description");
                document.getElementById("<%=txtMaterialDescription.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtMobileNo.ClientID %>").value == "") {
                alert("Pls fill mobile no");
                document.getElementById("<%=txtMobileNo.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TDInTime.ClientID %>")) {
                if (document.getElementById('CPH_Form_txtInTime').value == "") {
                    alert("Please fill In-Time");
                    document.getElementById("CPH_Form_txtInTime").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDOutTime.ClientID %>")) {
                if (document.getElementById('CPH_Form_txtOutTime').value == "") {
                    alert("Please fill Out Time");
                    document.getElementById("CPH_Form_txtOutTime").focus();
                    return false;
                }
            }

            else {
                return confirm('Do you want to save data?')
            }
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode != 45 && charCode > 31 && (charCode < 48 || charCode > 57)) {
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
                <table>
                    <tr id="Tr1" runat="server">
                        <td id="Td10" class="tdstyle">
                            <span class="labelbold">GateType</span>
                            <br />
                            <asp:DropDownList ID="ddGateType" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddGateType_SelectedIndexChanged"
                                Enabled="false">
                                <asp:ListItem Value="1">Gate In</asp:ListItem>
                                <asp:ListItem Value="2" Selected="True">Gate Out</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="TDGatePassNo" class="tdstyle" runat="server" visible="false">
                            <span class="labelbold">GatePassNo</span>
                            <br />
                            <asp:DropDownList ID="DDGatePassNo" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDGatePassNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td6" class="tdstyle">
                            <span class="labelbold">Material Type</span>
                            <br />
                            <asp:DropDownList ID="DDMaterialReturnType" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDMaterialReturnType_SelectedIndexChanged">
                                <asp:ListItem Value="1">Return able</asp:ListItem>
                                <asp:ListItem Value="2" Selected="True">Non Return able</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="Td5" class="tdstyle">
                            <span class="labelbold">Rec Date</span>
                            <br />
                            <asp:TextBox ID="txtdate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtdate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="Td1" class="tdstyle">
                            <span class="labelbold">Company Name</span>&nbsp;
                            <asp:CheckBox ID="ChKForEdit" runat="server" Text=" For Edit" CssClass="checkboxbold"
                                OnCheckedChanged="ChKForEdit_CheckedChanged" AutoPostBack="true" />
                            <br />
                            <asp:DropDownList ID="ddCompName" runat="server" Width="180px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="Td9" class="tdstyle">
                            <span class="labelbold">Branch Name</span>&nbsp;
                            <br />
                            <asp:DropDownList ID="DDBranchName" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="Td4" class="tdstyle">
                            <span class="labelbold">Party Name</span>
                            <br />
                            <asp:TextBox ID="txtPartyName" runat="server" Width="150px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Qty</span>
                            <br />
                            <asp:TextBox ID="TxtQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                BackColor="Beige" Width="70px"></asp:TextBox>
                        </td>
                        <td id="Td3" runat="server">
                            <span class="labelbold">Unit</span>
                            <br />
                            <asp:TextBox ID="txtUnit" runat="server" Width="50px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td id="Td2" runat="server">
                            <span class="labelbold">Material Description</span>
                            <br />
                            <asp:TextBox ID="txtMaterialDescription" runat="server" Width="200px" CssClass="textb"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="TdChallanNo" class="tdstyle" runat="server">
                            <span class="labelbold">GatePassIn No </span>
                            <br />
                            <asp:TextBox ID="TxtGatePassInNo" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                Enabled="false"></asp:TextBox>
                        </td>
                        <td id="TdGPNo" class="tdstyle" runat="server" visible="false">
                            <span class="labelbold">GatePassOut No </span>
                            <br />
                            <asp:TextBox ID="txtGatePassOutNo" Width="100px" runat="server" BackColor="Beige"
                                CssClass="textb" onkeydown="return (event.keyCode!=13);" Enabled="false"></asp:TextBox>
                        </td>
                        <td id="TDLVehicleNo" runat="server" class="tdstyle">
                            <span class="labelbold">Vehicle No</span>
                            <br />
                            <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                        </td>
                        <td id="TD7" runat="server" class="tdstyle">
                            <span class="labelbold">Through</span>
                            <br />
                            <asp:TextBox ID="txtThrough" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                        </td>
                        <td id="TD8" runat="server" class="tdstyle">
                            <span class="labelbold">MobileNo</span>
                            <br />
                            <asp:TextBox ID="txtMobileNo" runat="server" CssClass="textb" Width="100px" onkeypress="return isNumber(event);"></asp:TextBox>
                        </td>
                        <td id="TDInTime" runat="server" class="tdstyle">
                            <span class="labelbold">In-Time</span>
                            <br />
                            <asp:TextBox ID="txtInTime" runat="server" CssClass="textb" Width="100px" Enabled="false"></asp:TextBox>
                        </td>
                        <td id="TDOutTime" runat="server" class="tdstyle" visible="false">
                            <span class="labelbold">Out-Time</span>
                            <br />
                            <asp:TextBox ID="txtOutTime" runat="server" CssClass="textb" Width="100px" Enabled="false"></asp:TextBox>
                        </td>
                        <td class="tdstyle" id="TDtxtremarks" runat="server" colspan="3">
                            <span class="labelbold">Remarks</span>
                            <br />
                            <asp:TextBox ID="txtremarks" runat="server" Width="250px" CssClass="textb" TextMode="MultiLine"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="Label" ForeColor="Red" Visible="false"></asp:Label>
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew()"
                                CssClass="buttonnorm" Width="70px" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return Validation();"
                                CssClass="buttonnorm" Width="70px" />
                            <%--  <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click"
                                Width="80px" />--%>
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                CssClass="buttonnorm" Width="70px" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr id="TRMaterialIn" runat="server" visible="false">
                        <td colspan="6" valign="top">
                            <div style="width: 800px; height: 300px; overflow: auto;">
                                <asp:GridView ID="gvMaterialIndetail" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="gvMaterialIndetail_RowDataBound" DataKeyNames="GateInMaterialRegisterID"
                                    CssClass="grid-views" OnRowDeleting="gvMaterialIndetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="GatePassIn No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGatePassInNo" Text='<%#Bind("GatePassInNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MaterialType">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaterialType" Text='<%#Bind("MaterialType") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyName" Text='<%#Bind("CompanyName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PartyName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartyName" Text='<%#Bind("PartyName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" Text='<%#Bind("Qty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnit" Text='<%#Bind("Unit") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VehicleNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehicleNo" Text='<%#Bind("VehicleNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Through">
                                            <ItemTemplate>
                                                <asp:Label ID="lblThrough" Text='<%#Bind("Through") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MobileNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMobileNo" Text='<%#Bind("MobileNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="InTime">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInTime" Text='<%#Bind("InTime") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do You Want To Deleted Data ?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Show">
                                            <ItemTemplate>
                                                <asp:Button ID="BtnPreviewIn" runat="server" Text="Preview" CssClass="dropdown" OnClick="BtnPreviewIn_Click" />
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
                <table>
                    <tr id="TRMaterialOut" runat="server" visible="false">
                        <td colspan="6" valign="top">
                            <div style="width: 800px; height: 300px; overflow: auto;">
                                <asp:GridView ID="GVMaterialOutDetail" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="GVMaterialOutDetail_RowDataBound" DataKeyNames="GateOutMaterialRegisterID"
                                    CssClass="grid-views" OnRowDeleting="GVMaterialOutDetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="GatePassOut No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGatePassOutNo" Text='<%#Bind("GatePassOutNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MaterialType">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaterialType" Text='<%#Bind("MaterialType") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyName" Text='<%#Bind("CompanyName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PartyName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartyName" Text='<%#Bind("PartyName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" Text='<%#Bind("Qty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnit" Text='<%#Bind("Unit") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VehicleNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehicleNo" Text='<%#Bind("VehicleNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Through">
                                            <ItemTemplate>
                                                <asp:Label ID="lblThrough" Text='<%#Bind("Through") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MobileNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMobileNo" Text='<%#Bind("MobileNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OutTime">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOutTime" Text='<%#Bind("OutTime") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do You Want To Deleted Data ?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Show">
                                            <ItemTemplate>
                                                <asp:Button ID="BtnPreviewOut" runat="server" Text="Preview" CssClass="dropdown"
                                                    OnClick="BtnPreviewOut_Click" />
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
</asp:Content>
