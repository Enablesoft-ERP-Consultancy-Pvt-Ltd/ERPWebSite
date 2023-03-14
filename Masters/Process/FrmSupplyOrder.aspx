<%@ Page Title="Supply Order" Language="C#" AutoEventWireup="true" CodeFile="FrmSupplyOrder.aspx.cs" MasterPageFile="~/ERPmaster.master"
    Inherits="Masters_ProcessIssue_FrmSupplyOrder" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "FrmSupplyOrder.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate_RequiredDate() {
            var required_date = document.getElementById("<%=TxtRequiredDate.ClientID %>").Value;
            var assign_date = document.getElementById("<%=TxtAssignDate.ClientID %>").value;
            if (assign_date < required_date) {
                alert("Required Date Must Be Greater Then Assign Date");
            }
        }
        function AddAddEmp() {
            window.open('../Campany/frmWeaver.aspx?ABC=1', '', 'Height=600px,width=1000px');
        } 
    </script>
    <table width="100%" border="1">
        <tr>
            <td align="left" height="inherit" valign="top" class="style1" colspan="2">
                <div id="maindiv">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="80%">
                                <tr>
                                    <td id="TDLblSupplyOrderNo" runat="server" class="tdstyle">
                                    <span class=labelbold>Supply Order No</span>
                                        
                                    </td>
                                    <td class="tdstyle">
                                    <span class=labelbold> Company Name</span>
                                       
                                    </td>
                                    <td class="tdstyle">
                                     <span class=labelbold> Cust Code</span>
                                       &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="ChkForEdit" runat="server" Text="For Edit" AutoPostBack="True"
                                            OnCheckedChanged="ChkForEdit_CheckedChanged" ForeColor="Red" CssClass="checkboxnormal" TabIndex="1" />
                                    </td>
                                    <td class="tdstyle">
                                     <span class=labelbold>Customer Order No.</span>
                                        
                                    </td>
                                    <td class="tdstyle">
                                     <span class=labelbold> Process Name</span>
                                       
                                    </td>
                                    <td>
                                        <asp:Button ID="refreshEmp2" runat="server" Visible="true" Text="" BorderWidth="0px"
                                            Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                            ForeColor="White" OnClick="refreshEmp_Click" />
                                        <asp:Label ID="LblEmpName" class="tdstyle" runat="server" Text="Vendor Name"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="BtnAddEmp" runat="server" Text="ADD"  CssClass="buttonnorm" Width="40px"
                                            OnClientClick="return AddAddEmp()" />
                                    </td>
                                    <td id="TDLblDDSupplyOrderNo" runat="server" class="tdstyle">
                                     <span class=labelbold>Supply Order No</span>
                                        
                                    </td>
                                    <td class="tdstyle">
                                     <span class=labelbold>Supply No</span>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDSupplyOrderNoEdit" runat="server">
                                        <asp:TextBox ID="TxtsupplyOrderNoEdit" runat="server" Width="120px"  CssClass="textb"
                                            AutoPostBack="True" OnTextChanged="TxtsupplyOrderNoEdit_TextChanged" TabIndex="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList  CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" TabIndex="3">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList  CssClass="dropdown" ID="DDCustomerCode" Width="160px" runat="server"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" TabIndex="4">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCustomerCode"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList  CssClass="dropdown" ID="DDCustomerOrderNumber" Width="150px"
                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDCustomerOrderNumber_SelectedIndexChanged" TabIndex="5">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCustomerOrderNumber"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList  CssClass="dropdown" ID="DDProcessName" runat="server" AutoPostBack="True"
                                            Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged1" TabIndex="6" >
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDProcessName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList  CssClass="dropdown" ID="DDEmployeeName" Width="150px" runat="server"
                                            OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged" AutoPostBack="True" TabIndex="7">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDEmployeeName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDDDSupplyOrderNo" runat="server">
                                        <asp:DropDownList  CssClass="dropdown" ID="DDSupplyOrderNo" Width="125px" runat="server"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDSupplyOrderNo_SelectedIndexChanged" TabIndex="8">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDSupplyOrderNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtIssueNo" runat="server" Width="90px" Enabled="false" 
                                             CssClass="textb"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="TRDETAIL">

                                    <td class="tdstyle">
                                     <span class=labelbold> Freight</span>
                                       
                                        <br />
                                        <asp:DropDownList ID="DDFreight" runat="server" AutoPostBack="True"  CssClass="dropdown"
                                            Width="125px" TabIndex="9" >
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDFreight"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                     <span class=labelbold>Insurance</span>
                                        
                                        <br />
                                        <asp:DropDownList ID="DDInsurance" runat="server" AutoPostBack="True"  CssClass="dropdown"
                                            Width="150px" TabIndex="10">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDInsurance"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                     <span class=labelbold> Payment At</span>
                                       
                                        <br />
                                        <asp:DropDownList ID="DDPaymentAt" runat="server" AutoPostBack="True"  CssClass="dropdown"
                                            Width="150px" TabIndex="11">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDPaymentAt"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td width="150px" class="tdstyle">
                                     <span class=labelbold>Supply</span>
                                        
                                        <br />
                                        <asp:TextBox ID="TxtDestination" runat="server" AutoPostBack="True" Width="150px"
                                             CssClass="textb" TabIndex="12"></asp:TextBox>
                                    </td>
                                    <td width="150px" class="tdstyle">
                                     <span class=labelbold> Liasoning</span>
                                       
                                        <br />
                                        <asp:TextBox ID="TxtLiasoning" runat="server" AutoPostBack="True" Width="150px"  CssClass="textb" TabIndex="13"></asp:TextBox>
                                    </td>
                                    <td width="150px" class="tdstyle">
                                     <span class=labelbold> Inspection</span>
                                       
                                        <br />
                                        <asp:TextBox ID="TxtInspection" runat="server" AutoPostBack="True" Width="150px"
                                             CssClass="textb" TabIndex="14"></asp:TextBox>
                                    </td>
                                    <td width="150px" class="tdstyle">
                                     <span class=labelbold> Sample Number</span>
                                       
                                        <br />
                                        <asp:TextBox ID="TxtSampleNumber" runat="server" AutoPostBack="True" Width="125px"
                                             CssClass="textb" TabIndex="15"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                     <span class=labelbold>Assign Date</span>
                                        
                                    </td>
                                    <td class="tdstyle">
                                     <span class=labelbold>Required Date</span>
                                        
                                    </td>
                                    <td class="tdstyle">
                                     <span class=labelbold> Unit</span>
                                       
                                    </td>
                                    <td class="tdstyle" id="calnaame" runat="server">
                                     <span class=labelbold>Cal Type</span>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="TxtAssignDate" runat="server" Width="100px"  CssClass="textb" TabIndex="16"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtAssignDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtRequiredDate" runat="server" AutoPostBack="true"  CssClass="textb"
                                            OnTextChanged="TxtRequiredDate_TextChanged" Width="100px" TabIndex="17"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtRequiredDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDunit" runat="server"  CssClass="dropdown" Width="100px"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDunit_SelectedIndexChanged" TabIndex="18">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDunit"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDcaltype" runat="server"  CssClass="dropdown" Width="100px" TabIndex="19">
                                            <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                            <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                            <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                            <asp:ListItem Value="3">W-2</asp:ListItem>
                                            <asp:ListItem Value="4">L-2</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2" id="TDCONSMP" runat="server">
                                        <asp:CheckBox ID="ChkForConsumption" runat="server" Text="For Consumption" AutoPostBack="True"
                                            CssClass="checkboxbold" ForeColor="Blue" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="right" colspan="7">
                                        <asp:Button ID="BtnNew" runat="Server" OnClick="BtnNew_Click" OnClientClick="return reloadPage();"
                                            Text="New"  CssClass="buttonnorm"  />
                                        &nbsp;<asp:Button ID="BtnSave" runat="server" OnClick="BtnSave_Click"  CssClass="buttonnorm"
                                            OnClientClick="return confirm('Do you want to save data?')" Text="Save" TabIndex="22" />
                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" Text="Preview" Visible="true" OnClick="BtnPreview_Click"
                                             CssClass="buttonnorm" TabIndex="23"/>
                                        &nbsp;<asp:Button ID="BtnClose" runat="server"  CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                            Text="Close" TabIndex="26"/>
                                        &nbsp;<asp:Button ID="Btnstatus" runat="server"  CssClass="buttonnorm" Width="120px" Visible="false"
                                            Text="Complete Order" TabIndex="26" onclick="Btnstatus_Click"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td runat="server" id="tdinst">
                                     <span class=labelbold>Instructions</span>
                                        
                                    </td>
                                    <td colspan="7">
                                     <span class=labelbold> Remarks</span>
                                       
                                        <asp:TextBox ID="TxtRemarks" runat="server" Width="90%"  CssClass="textb" TabIndex="20"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7" runat="server" id="tdtxtinst">
                                        <asp:TextBox ID="TxtInstructions" runat="server" Height="50px" Width="100%"  CssClass="textb" TabIndex="21"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" CssClass="labelbold"
                                            Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                <td colspan="4" align="center">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" ForeColor="RED" CssClass="labelbold"
                                            Text="Description" Visible="false"></asp:Label>
                                </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <div style="width: 100%; height: 300px; overflow: scroll">
                                            <asp:GridView ID="DGDetail" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                BorderStyle="Solid" CssClass="grid-views"
                                                OnRowDataBound="DGDetail_RowDataBound" >
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="Chkbox" runat="server"  />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Item" HeaderText="">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Quality" HeaderText="">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtDGDesign" runat="server" Width="100px" Text='<%# Bind("Design") %>'
                                                                AutoPostBack="true"  CssClass="textb" TabIndex="27"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtDGColor" runat="server" Width="100px" Text='<%# Bind("Color") %>'
                                                                AutoPostBack="true"  CssClass="textb" TabIndex="28"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Shape" HeaderText="">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="70px"  />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Size" HeaderText="">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OQty" HeaderText="OQty">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SQty" HeaderText="SQty">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Area" HeaderText="Area">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="IssQty">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtDGIssQty" runat="server" Width="50px" Text='<%# Bind("IssQty") %>'
                                                                AutoPostBack="true"  CssClass="textb" TabIndex="29"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="40px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Rate">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtDGRate" runat="server" Width="40px" Text='<%# Bind("Rate") %>'
                                                                AutoPostBack="true"  CssClass="textb" TabIndex="30"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="40px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ArticalNo">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtDGArticalNo" runat="server" Width="120px" Text='<%# Bind("ArticalNo") %>'
                                                                AutoPostBack="true"  CssClass="textb" TabIndex="31"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="SubQuality" HeaderText="SubQuality">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="360px" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                               
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
