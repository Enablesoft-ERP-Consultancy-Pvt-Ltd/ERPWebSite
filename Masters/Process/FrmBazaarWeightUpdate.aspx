<%@ Page Title="Bazaar Weight Update" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmBazaarWeightUpdate.aspx.cs" Inherits="Masters_Process_FrmBazaarWeightUpdate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/Fixfocus.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmBazaarWeightUpdate.aspx";
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
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkEdit_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td> 
                             
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtBazaarDate" CssClass="textb" Width="80px" runat="server" OnTextChanged="txtBazaarDate_TextChanged" AutoPostBack="true" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtBazaarDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>

                          <%--  <td id="TD2" runat="server" visible="true">
                                <asp:Label ID="Label7" runat="server" Text="Emp Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDEmpName" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDEmpName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>

                           <td id="TD1" runat="server" visible="true">
                                <asp:Label ID="Label1" runat="server" Text="Receive ChallanNo." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDReceiveChallanNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDReceiveChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>--%>
                           
                        </tr>
                       
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label6" runat="server" Text="Bazaar Details" CssClass="labelbold" ForeColor="Red"
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
                                            <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="Raw Material Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                             <asp:TemplateField HeaderText="Challan No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceiveChallanNo" Text='<%#Bind("CHALLANNO")%>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="EmpName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpName" Text='<%#Bind("EmpName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Receive Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceiveDate" Text='<%#Bind("ReceiveDate")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="QualityName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Design">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesignName" Text='<%#Bind("DesignName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblColorName" Text='<%#Bind("ColorName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shape">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShapeName" Text='<%#Bind("ShapeName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField HeaderText="Size">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" Text='<%#Bind("Size")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnitName" Text='<%#Bind("UnitName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Total Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalRecQty" Text='<%#Bind("RecQty")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>   
                                             <asp:TemplateField HeaderText="Total Wt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTotalWeight" Width="70px" Text='<%#Bind("TotalWeight")%>' BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Chk Pcs">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtChkPcs" Width="70px" Text='<%#Bind("CheckPcs")%>' BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Check Wt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtChkWeight" Width="70px" Text='<%#Bind("CheckWeight")%>' BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Dry Wt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDryWeight" Width="70px" Text='<%#Bind("DryWeight")%>' BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                        
                                         
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                   <asp:Label ID="lblPROCESS_REC_ID" Text='<%#Bind("PROCESS_REC_ID") %>' runat="server" />    
                                                   <asp:Label ID="lblQualityId" Text='<%#Bind("QualityId") %>' runat="server" />                                          
                                                  
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>

                 <%--<div>
                    <table width="70%">
                        <tr>
                            <td>
                                 <asp:Label ID="Label2" runat="server" Text="Total Weight" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtTotalWeight" CssClass="textb" Width="150px" runat="server" />
                            </td>
                             <td>
                                 <asp:Label ID="Label4" runat="server" Text="Chk Pcs" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtChkPcs" CssClass="textb" Width="150px" runat="server" />
                            </td>
                             <td>
                                 <asp:Label ID="Label5" runat="server" Text="Chk Weight" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtChkWeight" CssClass="textb" Width="150px" runat="server" />
                            </td>
                             <td id="TDDryWeight" runat="server" visible="true">
                                 <asp:Label ID="Label8" runat="server" Text="Dry Weight" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtDryWeight" CssClass="textb" Width="150px" runat="server" />
                            </td>
                            <td colspan="2">&nbsp;</td>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>
                </div>--%>

                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />     
                                 <asp:Button ID="BtnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="BtnPreview_Click" />                          
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                                <asp:Button ID="btnFinalChallan" runat="server" Text="Update Final Consumption" CssClass="buttonnorm" OnClick="btnFinalChallan_Click" />
                                
                            </td>
                        </tr>
                    </table>
                </div>   
                 <div>
                    <table width="23%">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="From:" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtFromDate" CssClass="textb" Width="80px" runat="server"/>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                             <td>
                                <asp:Label ID="Label2" runat="server" Text="To:" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtToDate" CssClass="textb" Width="80px" runat="server"/>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td align="left">  
                             <asp:Label ID="Label4" runat="server" Text="" CssClass="labelbold"></asp:Label><br />                             
                                 <asp:Button ID="BtnPreviewNew" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="BtnPreviewNew_Click" /> 
                                
                            </td>
                        </tr>
                    </table>
                </div>                  
               <%-- <asp:HiddenField ID="hnissueid" runat="server" Value="0" />--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
