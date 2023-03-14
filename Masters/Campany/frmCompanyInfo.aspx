<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmCompanyInfo.aspx.cs" Inherits="Masters_Campany_frmCompanyInfo"
    EnableEventValidation="false" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <style type="text/css">
        #newPreview
        {
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);
        }
        .textbox
        {
        }
    </style>
    <script language="javascript" type="text/javascript">
        function PreviewImg(imgFile) {
            var newPreview = document.getElementById("newPreview");
            document.getElementById("newPreview").value = "";
            newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
            newPreview.style.width = "111px";
            newPreview.style.height = "66px";
            var control = document.getElementById("newPreview1");
            control.style.visibility = "hidden";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmCompanyInfo.aspx";
        }
        function report() {
            window.open('../../ReportViewer.aspx', '');
            // window.open('AddItemt.aspx','popup Form', 'Width=550px, Height=400px') 
        }

        function AddBank() {
            //            var answer = confirm("Do you want to ADD?")
            //            if (answer) {
            window.open('AddBank.aspx', '', 'width=950px,Height=500px');
            //            }
        }
        function Addlegalinformation() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var a = document.getElementById('CPH_Form_txtid').value;
                if (a == "" || a == "0") {
                    alert('Plz Select company');
                    return false;
                }
                window.open('AddLegalinformation.aspx?a=' + a, '', 'width=1150px,Height=600px');
            }
        }
        function Addsign() {
            //            var answer = confirm("Do you want to ADD?")
            //            if (answer) {
            var left = (screen.width / 2) - (500 / 2);
            var top = (screen.height / 2) - (300 / 2);

            //window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');

            window.open('frmSignature.aspx', '', 'width=500px,Height=300px,top=' + top + ',left=' + left);
            //            }
        }
        
    </script>
    <%--Page Design--%>
    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table width="100%" border="1">
                <tr>
                    <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                        <div id="1" style="height: auto" align="left">
                            <table>
                                <caption>
                                    <tr>
                                        <td class="tdstyle" colspan="2">
                                            <asp:Label ID="lbl1" runat="server" CssClass="label" Text="Company Name" />
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtCompName" runat="server" CssClass="textb" ValidationGroup="s"
                                                Width="80%"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCompName"
                                                ErrorMessage="please Enter Companyname" ForeColor="Red" ValidationGroup="s">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle" colspan="2">
                                            <asp:Label ID="Label1" runat="server" CssClass="label" Text="Address1" />
                                        </td>
                                        <td class="style6">
                                            <asp:TextBox ID="txtAddress1" runat="server" CssClass="textboxres" TextMode="MultiLine"
                                                Width="253px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" CssClass="label" Text="Address2" />
                                            <asp:Button ID="B" runat="server" BackColor="White" BorderWidth="0px" ForeColor="White"
                                                OnClick="B_Click" Width="0px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAddress2" runat="server" CssClass="textboxres" TextMode="MultiLine"
                                                Width="253px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" CssClass="label" Text="Address3" />
                                        </td>
                                        <td class="style6">
                                            <asp:TextBox ID="txtAddress3" runat="server" CssClass="textboxres" TextMode="MultiLine"
                                                Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </caption>
                            </table>
                            &nbsp<table>
                                <tr>
                                    <td class="tdstyle" colspan="2">
                                        <asp:Label ID="Label4" Text="Company Phone" runat="server" CssClass="label" />
                                    </td>
                                    <td class="style6">
                                        <asp:TextBox CssClass="textb" ID="txtPhone" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" Operator="DataTypeCheck"
                                            Type="Integer" ControlToValidate="txtPhone" Text="Must be a number." ForeColor="Red"
                                            SetFocusOnError="true" />--%>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label5" Text="Tin No" runat="server" CssClass="label" />
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="txtUPTTNo" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label6" Text="EDP Code" runat="server" CssClass="label" />
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="txtEDPCode" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                </tr>
                </tr>
                <tr>
                    <td class="tdstyle" colspan="2">
                        <asp:Label ID="Label7" Text="Bank Name" runat="server" CssClass="label" />
                    </td>
                    <td class="style6">
                        <asp:DropDownList CssClass="dropdown" ID="dropListBank" runat="server" Width="125px">
                        </asp:DropDownList>
                        <asp:Button CssClass="buttonsmalls" ID="cmdBankMaster" runat="server" Text="&#43;"
                            OnClientClick="return AddBank()" Width="35px" />
                        <asp:Button ID="x" runat="server" Text="Button" BackColor="White" BorderColor="White"
                            BorderWidth="0px" ForeColor="White" Height="1px" Width="1px" OnClick="x_Click" />
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label8" Text="IE Code" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtIEcode" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        <asp:Button ID="Button2" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px"
                            ForeColor="White" Height="1px" OnClick="Button2_Click" Width="1px" />
                    </td>
                    <td>
                        <br />
                        <asp:TextBox CssClass="textb" ID="txtid" runat="server" Style="display: none"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle" colspan="2">
                        <asp:Label ID="Label9" Text="Roll Mark Head" runat="server" CssClass="label" />
                    </td>
                    <td class="style6">
                        <asp:TextBox CssClass="textb" ID="txtRollMhead" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label10" Text="Exporter Ref" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtExpref" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>&nbsp;
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label11" Text="Signatory" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:DropDownList CssClass="dropdown" ID="CmbSignatory" runat="server" Width="130px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="CmbSignatory"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                        <asp:Button CssClass="buttonsmalls" ID="btnAdd" runat="server" Text="&#43;" Height="22px"
                            Style="margin-left: 0px; margin-top: 0px" Width="35px" OnClientClick="return Addsign();" />
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle" colspan="2">
                        <asp:Label ID="Label12" Text="DBK1 Bank" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:DropDownList CssClass="dropdown" ID="dropDBK1Bank" runat="server" Width="130px"
                            TabIndex="13">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label13" Text="Company Fax" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtCompFax" runat="server" Width="125px" Height="16px"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label14" Text="CST No" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtCSTNo" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle" colspan="2">
                        <asp:Label ID="Label15" Text="DBK1 A/C No." runat="server" CssClass="label" />
                    </td>
                    <td class="style6">
                        <asp:TextBox CssClass="textb" ID="txtDBK1Ac" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label16" Text="Current A/C No" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtCACNo" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label26" Text="GST No" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtGSTNo" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle" colspan="2">
                        <asp:Label ID="Label17" Text="DBK2 Bank" runat="server" CssClass="label" />
                    </td>
                    <td class="style6">
                        <asp:DropDownList CssClass="dropdown" ID="dropDBK2Bank" runat="server" Width="125px"
                            TabIndex="18">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label18" Text="Pan No." runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtPAN" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle" colspan="2">
                        <asp:Label ID="Label19" Text="DBK2 A/C No." runat="server" CssClass="label" />
                    </td>
                    <td class="style6">
                        <asp:TextBox CssClass="textb" ID="txtDBK2Ac" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label20" Text="RBI Code" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtRBICode" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label21" Text="CoSynonyms" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtCoSynonyms" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle" colspan="2">
                        <asp:Label ID="Label22" Text="Email" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtEmail" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle" rowspan="2" colspan="2">
                        <asp:Label ID="Label23" Text="Instructions" runat="server" CssClass="label" />
                    </td>
                    <td class="style6" rowspan="2">
                        <asp:TextBox CssClass="textb" ID="multitext" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label24" Text="Declaration1" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtdecloration1" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox><br />
                        <br />
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label27" Text="WebSite Name" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtWebSiteName" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox><br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label25" Text="Declaration2" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtdecloration2" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label28" Text="Factory Address" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtFactoryAddress" runat="server" Width="250px"
                            TextMode="MultiLine"></asp:TextBox><br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">                     
                        <asp:Label ID="Label31" Text="LUT-ARNNo." runat="server" CssClass="label" />
                        <br />
                    </td>
                    <td class="tdstyle">
                        <asp:TextBox CssClass="textb" ID="txtlutarnno" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>

                     <td >
                        
                         <asp:Label ID="Label32" Text="LUT-IssueDate" runat="server" CssClass="label" />
                        <br />
                    </td>
                    <td >
                         <asp:TextBox ID="txtLUTIssueDate" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtLUTIssueDate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                    </td>

                     <td >                        
                         <asp:Label ID="Label33" Text="LUT-ExpiryDate" runat="server" CssClass="label" />
                        <br />
                    </td>
                    <td>
                         <asp:TextBox ID="txtLUTExpiryDate" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtLUTExpiryDate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                    </td>
                </tr>
                <tr id="TRMobileState" runat="server" visible="false">
                    <td class="tdstyle" colspan="2">
                        <asp:Label ID="Label29" Text="Mobile No" runat="server" CssClass="label" />
                    </td>
                    <td class="style6">
                        <asp:TextBox CssClass="textb" ID="txtMobileNo" runat="server" Width="125px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label30" Text="State" runat="server" CssClass="label" />
                    </td>
                    <td>
                        <asp:DropDownList CssClass="dropdown" ID="DDState" runat="server" Width="125px">
                        </asp:DropDownList>
                        <br />
                        <br />
                    </td>
                    <td class="tdstyle">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="newPreview" runat="server">
                            <asp:Image ID="newPreview1" runat="server" Height="66px" Width="111px" />
                        </div>
                    </td>
                    <td colspan="4">
                        <asp:FileUpload ID="compneyImage" CssClass="fileuploads" onchange="PreviewImg(this)"
                            runat="server" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                            ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="compneyImage"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Bold="true" Font-Italic="true"
                            Font-Names="Times new Roman" Font-Overline="false" ForeColor="Red" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="s" />
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:Label ID="lblErr1" runat="server" CssClass="errormsg" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblerr" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="text-align: right" colspan="5">
                        <asp:Button ID="btnnew0" runat="server" CssClass="buttonnorm" OnClientClick="NewForm();"
                            TabIndex="31" Text="New" Width="70px" />
                        <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CssClass="buttonnorm"
                            ValidationGroup="s" OnClientClick="return confirm('Do you want to save data?')"
                            Width="70px" />
                        <asp:Button CssClass="buttonnorm" ID="rpt0" runat="server" Text="Preview" TabIndex="32"
                            OnClick="rpt0_Click" Width="70px" />
                        <asp:Button ID="Button1" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();"
                            TabIndex="33" Text="Close" Width="70px" />
                        <asp:Button ID="addlegalinformation" runat="server" CssClass="buttonnorm" Text="ADD Legal information"
                            OnClientClick="Addlegalinformation();" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <div style="width: 1000px; height: 200px; overflow: auto">
                            <asp:GridView ID="dgComapny" runat="server" Width="864px" CellPadding="4" PageSize="5"
                                ForeColor="#333333" DataKeyNames="CompanyId" AllowPaging="true" OnPageIndexChanging="dgComapny_PageIndexChanging"
                                OnRowDataBound="dgComapny_RowDataBound" OnSelectedIndexChanged="dgComapny_SelectedIndexChanged"
                                TabIndex="33" AutoGenerateColumns="False" OnRowCreated="dgComapny_RowCreated"
                                OnRowDeleting="dgComapny_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="CompanyId" HeaderText="Sr.No." Visible="false" />
                                    <asp:BoundField DataField="CompanyName" HeaderText="CompName" />
                                    <asp:BoundField DataField="CompAddr1" HeaderText="CompAddr1" />
                                    <asp:BoundField DataField="CompAddr2" HeaderText="CompAddr2" />
                                    <asp:BoundField DataField="CompAddr3" HeaderText="CompAddr3" />
                                    <asp:BoundField DataField="CompFax" HeaderText="CompFax" />
                                    <asp:BoundField DataField="CompTel" HeaderText="CompTel" />
                                    <asp:BoundField DataField="RBICode" HeaderText="RBICode" />
                                    <asp:BoundField DataField="IECode" HeaderText="IECode" />
                                    <asp:BoundField DataField="PANNr" HeaderText="PANNr" />
                                    <asp:BoundField DataField="EDPNo" HeaderText="EDPNo" />
                                    <asp:BoundField DataField="CurAcctNo" HeaderText="CurAcctNo" />
                                    <asp:BoundField DataField="RollMarkHead" HeaderText="RollMarkHead" />
                                    <asp:BoundField DataField="Dbk1ACNo" HeaderText="Dbk1ACNo" />
                                    <asp:BoundField DataField="Dbk2ACNo" HeaderText="Dbk2ACNo" />
                                    <asp:BoundField DataField="UPTTNo" HeaderText="UPTTNo" />
                                    <asp:BoundField DataField="CSTNo" HeaderText="CSTNo" />
                                    <asp:BoundField DataField="RCMCNo" HeaderText="RCMCNo" />
                                    <asp:BoundField DataField="GSPNo" HeaderText="GSPNo" />
                                    <asp:BoundField DataField="CoSyn" HeaderText="CoSyn" />
                                    <asp:BoundField DataField="ExpRef" HeaderText="ExpRef" />
                                    <asp:BoundField DataField="OpeningBalance" HeaderText="OpeningBalance" />
                                    <asp:BoundField DataField="Instruction" HeaderText="Instruction" />
                                    <asp:BoundField DataField="TinNo" HeaderText="TinNo" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="Declaration1" HeaderText="Declaration1" />
                                    <asp:BoundField DataField="Declaration2" HeaderText="Declaration2" />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            </div> </td> </tr> </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
