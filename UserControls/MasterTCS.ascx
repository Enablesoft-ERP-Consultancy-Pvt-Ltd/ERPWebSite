<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MasterTCS.ascx.cs" Inherits="UserControls_MasterTCS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
//        function closeForm() {

//            var objParent = window.opener;
//            if (objParent != null) {
//                if (window.opener.document.getElementById('CPH_Form_refreshDyerColor')) {
//                    window.opener.document.getElementById('CPH_Form_refreshDyerColor').click();
//                    self.close();
//                }

//            }
//            else {
//                window.location.href = "../../main.aspx";
//            }

//        }

        function reloadPage() {
            window.location.href = "FrmTCS.aspx";
        }
        function addpriview() {
            window.open("../../ReportViewer.aspx")
        }        
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 15%; margin-right: 15%">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                        &nbsp;
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="false"></asp:TextBox>
                        </td>
                        <td colspan="2">
                        &nbsp;
                        </td>
                       
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align:left">
                            <asp:Label ID="lblProcessName" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label><br />
                             <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="150px" runat="server"
                              AutoPostBack="true" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>

                             <%--<asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="150px" runat="server"
                              AutoPostBack="true" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>--%>
                        </td>
                         <td class="tdstyle" style="text-align:left">
                            <asp:Label ID="lblCategoryName" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label><br />
                             <asp:DropDownList CssClass="dropdown" ID="DDCategoryName" Width="150px" runat="server"
                              AutoPostBack="true" OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                            
                        </td>
                         <td class="tdstyle" style="text-align:left">
                            <asp:Label ID="lblItemName" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label><br />
                             <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="150px" runat="server"
                              AutoPostBack="true" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                            
                        </td>
                        <td style="text-align:left;">
                         <asp:Label ID="lblSubItemName" runat="server" Text="SubItem Name" CssClass="labelbold"></asp:Label><br />
                           <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="150px" runat="server"
                           AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                            
                        </td>
                    </tr>
                   <%-- <tr>
                    <td colspan="4">&nbsp;</td>
                    </tr>--%>
                    <tr>
                    <td class="tdstyle"  style="text-align:left">
                            <asp:Label ID="lblTCS" runat="server" Text="TCS" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox CssClass="textb" ID="txtTCSRate" Width="150px" runat="server"></asp:TextBox>
                        </td>
                        <%-- <td class="tdstyle" style="text-align:left">
                            <asp:Label ID="lblSGST" runat="server" Text="SGST" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox CssClass="textb" ID="txtSGSTRate" Width="150px" runat="server"></asp:TextBox>
                            
                        </td>
                         <td class="tdstyle" style="text-align:left">
                            <asp:Label ID="lblIGST" runat="server" Text="IGST" CssClass="labelbold"></asp:Label><br />
                             <asp:TextBox CssClass="textb" ID="txtIGSTRate" Width="150px" runat="server"></asp:TextBox>
                            
                        </td>--%>
                        <td style="text-align:left;">
                          <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date" Width="150px" CssClass="labelbold"></asp:Label><br />
                           <asp:TextBox ID="txtEffectiveDate" runat="server"  CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtEffectiveDate">
                            </asp:CalendarExtender>
                            
                        </td>
                    
                    
                    
                    
                    </tr>

               
                   <tr>
                   <td colspan="4">&nbsp;</td>
                   </tr>
                    <tr>
                       <td style="text-align: left; padding-left:250px" colspan="4">
                         
                           <%-- <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" Width="53px"
                                OnClientClick="return confirm('Do you want to save data?')" OnClick="btnSave_Click" />--%>
                                <asp:Button ID="BtnNew" runat="Server" Text="New" CssClass="buttonnorm" OnClientClick="return reloadPage();" />
                                  <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="56px" 
                               OnClientClick="if (!SaveData()) return ;this.disabled=true;this.value = 'wait ...';" CssClass="buttonnorm" />                         
                                <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="closeForm();"
                            UseSubmitBehavior="False" />
                         <%--   <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" Visible="false"
                                onclick="btnpreview_Click" />--%>
                           
                        </td>
                    </tr>
                    <tr>
                    <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                       
                        <td colspan="4">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            <div style="height: 250px; overflow: auto; width: 100%;">
                                <asp:GridView ID="gdTCSRate" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    OnSelectedIndexChanged="gdTCSRate_SelectedIndexChanged" DataKeyNames="Id"
                                    OnRowDataBound="gdTCSRate_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
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
                                        <asp:TemplateField HeaderText="TCS Rate" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCSRate" runat="server" Text='<%#Bind("TCSRate") %>'></asp:Label>
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
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>