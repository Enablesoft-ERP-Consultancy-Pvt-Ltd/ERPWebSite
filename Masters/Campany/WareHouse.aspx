<%@ Page Language="C#" AutoEventWireup="true" Title="Warehouse" CodeFile="WareHouse.aspx.cs"
    Inherits="WareHouse" MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function getbacktostepone() {
            window.lcation = "formWareHouseMaster.aspx";
        }
        function onSuccess() {
            setTimeout(okay, 200);
        }
        function onError() {
            setTimeout(getbacktostepone, 200);
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
        }
        function Priview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function AddWareHouseName() {
            var a = document.getElementById('CPH_Form_txtid').value;

            if (a == "" || a == "0") {
                alert('Plz Select or Insert WareHouse Code');
                return false;
            }
            window.open('AddWareHouseName.aspx?a=' + a, '', 'width=500px,Height=400px');
        }    
    </script>
    <div>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <%--<asp:TextBox ID="txtid" Visible="false" runat="server"></asp:TextBox>--%>
                <asp:TextBox ID="txtid" runat="server" CssClass="textb" Style="display: none"></asp:TextBox>
                <table style="width:100%; align:100%">
                    <tr align="left">
                        <td class="tdstyle" width="200px">
                            <asp:Label Text="Customer Code" runat="server" ID="lbl" Font-Bold="true"/><br />
                             <asp:DropDownList ID="cmbCustomerCode" runat="server" Width="154px" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="cmbCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="cmbCustomerCode"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                            
                        </td>
                        <td width="200px">
                             <asp:Label Text="City" runat="server" ID="Label2" Font-Bold="true"/><br />
                          <asp:TextBox ID="txtCity" runat="server" CssClass="textb" Width="150px" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox>
                           
                        </td>
                        <td class="tdstyle" width="200px">
                         <asp:Label Text="Country Final Destination" runat="server" ID="Label10" Font-Bold="true"/><br />

                        <asp:TextBox ID="txtCountryFinalDestination" runat="server" CssClass="textb" Width="150px" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox>
                            
                        </td>                       
                       
                    </tr>
                  
                        <tr align="left">
                        <td class="tdstyle">
                        <asp:Label Text="WareHouse Code" runat="server" ID="Label3" Font-Bold="true"/><br />
                              <asp:TextBox ID="txtWareHouseCode" runat="server" CssClass="textb" Width="150px" >
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtWareHouseCode"
                                ErrorMessage="Please Select WareHouse Code" ValidationGroup="m" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label Text="State" runat="server" ID="Label4" Font-Bold="true"/><br />
                             <asp:TextBox ID="txtState" runat="server" CssClass="textb" Width="150px" onkeydown="return (event.keyCode!=13);">
                                </asp:TextBox>
                          
                        </td>
                        <td class="tdstyle">
                        
                            <asp:Label Text="Port Of Loading" runat="server" ID="Label11" Font-Bold="true"/><br />
                             <asp:DropDownList ID="DDPortOfLoading" runat="server" Width="154px" CssClass="dropdown">
                            </asp:DropDownList>
                             <%--<asp:TextBox ID="txtPortOfLoading" runat="server" CssClass="textb" Width="150px" onkeydown="return (event.keyCode!=13);">
                                </asp:TextBox>--%>
                            
                        </td>
                    </tr>
                    <tr align="left">
                        <td class="tdstyle">
                        <asp:Label Text="WareHouse Name" runat="server" ID="Label5" Font-Bold="true"/><br />
                        <asp:TextBox ID="txtWareHouseName" runat="server" CssClass="textb" Width="150px">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtWareHouseName"
                                ErrorMessage="Please Enter WareHouse Name" ValidationGroup="m" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            
                        </td>
                        <td>
                            <asp:Label Text=" Country" runat="server" ID="Label6" Font-Bold="true"/><br />
                             <asp:TextBox ID="txtCountry" runat="server" CssClass="textb" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                             
                            
                        </td>
                        <td class="tdstyle">
                          <asp:Label Text=" Port Of Discharge" runat="server" ID="Label12" Font-Bold="true"/><br />
                             <asp:TextBox ID="txtPortOfDischarge" runat="server" CssClass="textb" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            
                           
                        </td>
                       
                    </tr>
                    <tr align="left">
                        <td class="tdstyle">
                        <asp:Label Text="  Address" runat="server" ID="Label7" Font-Bold="true"/><br />
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textb" Width="150px" onkeydown="return (event.keyCode!=13);"  TextMode="MultiLine" Height="50px" ></asp:TextBox>
                           
                        </td>
                         <td>
                            <asp:Label Text=" Place Of Delivery" runat="server" ID="Label13" Font-Bold="true"/><br />
                             <asp:TextBox ID="txtPlaceOfDelivery" runat="server" CssClass="textb" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            
                            
                        </td>
                        <td>
                            <asp:Label Text=" Final Destination" runat="server" ID="Label14" Font-Bold="true"/><br />
                             <asp:TextBox ID="txtFinalDestination" runat="server" CssClass="textb" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            
                            
                        </td>
                    </tr>

                     <tr align="left">
                        <td class="tdstyle">
                        <asp:Label Text="Consignee" runat="server" ID="Label8" Font-Bold="true"/><br />
                         <asp:TextBox ID="txtConsignee" runat="server" CssClass="textb" Width="200px" Height="80px" TextMode="MultiLine">
                            </asp:TextBox>   
                            
                        </td>
                        <td>
                             <asp:Label Text=" Buyer (If Other Than Consignee)" runat="server" ID="Label9" Font-Bold="true"/><br />
                             <asp:TextBox ID="txtBuyerOtherThanConsignee" runat="server" CssClass="textb" Width="200px" Height="80px" TextMode="MultiLine"></asp:TextBox>
                             
                                                
                        </td>
                        <td class="tdstyle">
                         <asp:Label Text=" Ship To" runat="server" ID="Label15" Font-Bold="true"/><br />
                             <asp:TextBox ID="txtShipTo" runat="server" CssClass="textb" Width="200px" Height="80px" TextMode="MultiLine"></asp:TextBox>
                            
                           
                        </td>
                        
                    </tr>

                    <tr align="left">
                        <td colspan="4">
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="false"
                                ShowSummary="true" Font-Bold="true" Font-Italic="true" Font-Names="Times new Roman"
                                Font-Overline="false" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr align="left">
                        <td colspan="4">
                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdWarehouse" runat="server" DataKeyNames="SrNo" OnRowDataBound="gdWarehouse_RowDataBound"
                                OnSelectedIndexChanged="gdWarehouse_SelectedIndexChanged" Width="350px" AllowPaging="True"
                                OnPageIndexChanging="gdWarehouse_PageIndexChanging" PageSize="50" CssClass="grid-views" AutoGenerateColumns="False"
                                OnRowCreated="gdWarehouse_RowCreated">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />

                                 <Columns>
                                           <%-- <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>--%>                                          
                                           
                                            <asp:TemplateField HeaderText="Customer Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerCode" Text='<%#Bind("CustomerCode")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WareHouse Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWareHouseCode" Text='<%#Bind("Warehousecode")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WareHouse Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWareHouseName" Text='<%#Bind("Warehousename")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddress" Text='<%#Bind("Address")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField HeaderText="City">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCity" Text='<%#Bind("City")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="State">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblState" Text='<%#Bind("State")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Country">
                                                <ItemTemplate>
                                                   <asp:Label ID="lblCountry" Text='<%#Bind("Country")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                                                                     
                                            
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWareHouseId" Text='<%#Bind("SrNo") %>' runat="server" />  
                                                 <asp:Label ID="lblWHConsignee" Text='<%#Bind("WHConsignee") %>' runat="server" />    
                                                    <asp:Label ID="lblWHBuyerOtherConsignee" Text='<%#Bind("WHBuyerOtherConsignee") %>' runat="server" />
                                                    <asp:Label ID="lblWHShipTo" Text='<%#Bind("WHShipTo") %>' runat="server" />
                                                    <asp:Label ID="lblWHCountryFinalDestination" Text='<%#Bind("WHCountryFinalDestination") %>' runat="server" />
                                                    <asp:Label ID="lblWHPortOfLoading" Text='<%#Bind("WHPortOfLoading") %>' runat="server" />
                                                    <asp:Label ID="lblWHPortOfDischarge" Text='<%#Bind("WHPortOfDischarge") %>' runat="server" />
                                                    <asp:Label ID="lblWHPlaceOfDelivery" Text='<%#Bind("WHPlaceOfDelivery") %>' runat="server" />
                                                    <asp:Label ID="lblWHFinalDestination" Text='<%#Bind("WHFinalDestination") %>' runat="server" />                                           
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                            </asp:GridView>
                        </td>
                    </tr>
                    <tr align="left">
                        <td colspan="2">
                        </td>
                        <td colspan="2" align="right">
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonnorm" OnClick="btnCancel_Click"
                                Text="New" />
                            <asp:Button ID="btnSave" runat="server" CssClass="buttonnorm" OnClick="Save_Click"
                                OnClientClick="return confirm('Do you want to save data?')" Text="Save" Width="65px"
                                ValidationGroup="m" />
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Preview" CssClass="buttonnorm" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />

                             &nbsp;<asp:Button ID="BtnAddWareHouseName" runat="server" CssClass="buttonnorm"
                                                        Text="ADD WareHouse Name" OnClientClick="return AddWareHouseName();" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
