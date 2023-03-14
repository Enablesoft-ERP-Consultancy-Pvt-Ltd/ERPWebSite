<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPurchaseRateMaster.aspx.cs"
    Inherits="Masters_Process_FrmPurchaseRateMaster" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" Title="Purchase Rate Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server" ID="Page">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script type="text/javascript">
        // $(document).ready(function () {
        function jScript() {
            $("#CPH_Form_btnsave").click(function () {
                var Message = "";
                if ($("#CPH_Form_DDCompanyName").val() == "") {
                    Message = Message + "Please,Select Company Name!!!\n";
                }               
                if ($("#CPH_Form_ddJob")) {
                    var selectedIndex = $('#CPH_Form_ddJob').attr('selectedIndex');

                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Job Name !!!\n";
                    }
                }
                if ($("#CPH_Form_DDItemName")) {
                    var selectedIndex = $('#CPH_Form_DDItemName').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Item Name !!!\n";
                    }
                }
                //                if ($("#CPH_Form_DDColor")) {
                //                    var selectedIndex = $('#CPH_Form_DDColor').attr('selectedIndex');
                //                    if (selectedIndex <= 0) {
                //                        Message = Message + "Please,select Colour Name !!!\n";
                //                    }
                //                }
//                if ($("#CPH_Form_DDShape").length) {
//                    if ($("#CPH_Form_DDShape").val() == "") {
//                        Message = Message + "Please,select Shape Name !!!\n";
//                    }
//                }
                //                if ($("#CPH_Form_ddSize").length) {
                //                    var selectedIndex = $('#CPH_Form_ddSize').attr('selectedIndex');
                //                    if (selectedIndex <= 0) {
                //                        Message = Message + "Please,select Size !!!\n";
                //                    }
                //                }
                if ($("#CPH_Form_txtEffectiveDate").val() == "") {
                    Message = Message + "Effective Date can not be blank !!!\n";
                }
                if ($("#CPH_Form_txtrate").val() == "") {
                    Message = Message + "Purchase Price can not be blank and Zero !!!\n";
                }

                if (Message == "") {
                    return true;
                }
                else {
                    alert(Message);
                    return false;
                }
            });
            //now use keypress event for Pincode and Mobile No
            $("#CPH_Form_txtrate").keypress(function (event) {

                if (event.which >= 46 && event.which <= 58) {
                    return true;
                }
                else {
                    return false;
                }

            });
        }
        // });
     
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScript);
            </script>
            <div class="Maindiv">
                <div style="width: 500px; margin: 0px auto;">
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblCompanyName" runat="server" CssClass="labelbold" Text="COMPANY NAME"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="250px" CssClass="dropdown">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Branch NAME"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDBranchName" runat="server" Width="250px" CssClass="dropdown">
                            </asp:DropDownList>
                        </div>
                    </div>
                   
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblJob" runat="server" CssClass="labelbold" Text="JOB NAME"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="ddJob" runat="server" Width="250px" CssClass="dropdown" OnSelectedIndexChanged="ddJob_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>                    
                    <div id="divCategory" runat="server" visible="true">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label1" runat="server" Text="CATEGORY" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDCategory" runat="server" Width="250px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lbluni" runat="server" CssClass="labelbold" Text="ITEM NAME"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDItemName" runat="server" Width="250px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDItemName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divQuality" runat="server" visible="true">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblqualityname" runat="server" Text="QUALITY NAME" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="ddquality" runat="server" Width="250px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="ddquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>  
                   
                    <div id="DivWeavingEmployee" runat="server" visible="true">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label6" runat="server" Text="Vendor Name" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDPurchaseVendor" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblPrice" runat="server" Text="PRICE" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px; margin-top:3px">
                            <asp:TextBox ID="txtrate" runat="server" CssClass="textb"></asp:TextBox>&nbsp&nbsp;
                        </div>
                    </div> 
                   
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label2" runat="server" Text="EFFECTIVE DATE" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px; margin-top:3px">
                             <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="textb"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="txtEffectiveDate">
                        </asp:CalendarExtender>
                        </div>
                    </div>                    

                    <div>
                        <table width="100%">
                            <tr>                                
                                <td align="right">
                                    <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" Width="75px"
                                        OnClick="btnsave_Click" />
                                    <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" Width="75px"
                                        OnClientClick="return CloseForm();" />
                                   
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="lblMessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                Font-Bold="true" />
                        </div>
                    </div>
                </div>
                <div style="width: 650px; margin: 0px auto; overflow: auto; max-height: 400px">
                    <asp:GridView ID="DGRateDetail" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                        Width="600px" OnSelectedIndexChanged="DGRateDetail_SelectedIndexChanged" DataKeyNames="Id" OnRowDataBound="DGRateDetail_RowDataBound">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Process Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProcessName" runat="server" Text='<%#Bind("PROCESS_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategoryName" runat="server" Text='<%#Bind("Category_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("Item_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SubItemName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubItemName" runat="server" Text='<%#Bind("QualityName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="VendorName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVendorName" runat="server" Text='<%#Bind("EmpName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Purchase Rate" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseRate" runat="server" Text='<%#Bind("PurchaseRate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>                                  
                                    
                                    <asp:TemplateField HeaderText="Effective Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#Bind("EffectiveDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblProcessId" runat="server" Text='<%# Bind("ProcessId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblCategoryId" runat="server" Text='<%# Bind("CategoryId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("ItemId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblQualityId" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblEmpId" runat="server" Text='<%# Bind("EmpId") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div> 

            </div>

              <table width="100%">
                    <tr>
                        <td colspan="8">
                            <table>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label17" Text="From Date" runat="server" CssClass="labelbold" /><span
                                            style="color: Red">*</span>
                                        <br />
                                        <asp:TextBox ID="txtFromDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label18" Text="To Date" runat="server" CssClass="labelbold" /><span
                                            style="color: Red">*</span>
                                        <br />
                                        <asp:TextBox ID="txtToDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtToDate" Format="dd-MMM-yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <br />
                                       <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            <asp:HiddenField ID="hnId" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
