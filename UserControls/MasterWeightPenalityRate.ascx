<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MasterWeightPenalityRate.ascx.cs" Inherits="UserControls_MasterWeightPenalityRate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closeForm() {

            var objParent = window.opener;
            if (objParent != null) {
                if (window.opener.document.getElementById('CPH_Form_refreshDyerColor')) {
                    window.opener.document.getElementById('CPH_Form_refreshDyerColor').click();
                    self.close();
                }

            }
            else {
                window.location.href = "../../main.aspx";
            }



        }

//        function CloseForm() {

//            var objParent = window.opener;
//            if (objParent != null) {
//                if (window.opener.document.getElementById('CPH_Form_refreshdyercolorrate')) {
//                    window.opener.document.getElementById('CPH_Form_refreshdyercolorrate').click();
//                    self.close();
//                }
//                else if (window.opener.document.getElementById('refreshdyercolorrate')) {
//                    window.opener.document.getElementById('refreshdyercolorrate').click();
//                    self.close();
//                }

//            }
//            else {
//                window.location.href = "../../main.aspx";
//            }
//        }

        function addpriview() {
            window.open("../../ReportViewer.aspx")
        }        
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; float: left; background-color: #E3E3E3">
                <table>
                    <tr>
                       
                        <td colspan="5">
                            <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                                <asp:Label ID="Label9" Text="Quality Type" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:DropDownList ID="DDItemName" AutoPostBack="true" runat="server" Width="150px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                                </asp:DropDownList>                                
                            </td>
                       <td class="tdstyle">
                                <asp:Label ID="Label4" Text="From (%)" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:TextBox CssClass="textb" ID="txtFromPercentage" runat="server" Width="150px"></asp:TextBox>                               
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label5" Text="To (%)" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:TextBox CssClass="textb" ID="txtToPercentage" runat="server" Width="150px"></asp:TextBox>                               
                            </td>
                             <td class="tdstyle">
                                <asp:Label ID="Label6" Text="Rate" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:TextBox CssClass="textb" ID="txtRate" runat="server" Width="150px"></asp:TextBox>                               
                            </td>
                             <td class="tdstyle">
                                <asp:Label ID="Label7" Text="Effective Date" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:TextBox ID="txtEffectiveDate" runat="server"  CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtEffectiveDate">
                            </asp:CalendarExtender>                              
                            </td>
                    </tr>
                    <tr>
                    <td colspan="5">&nbsp;</td>
                    </tr>                   
                 
                    <tr>
                       <td style="text-align: left; padding-left:250px" colspan="5">
                         
                           <%-- <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" Width="53px"
                                OnClientClick="return confirm('Do you want to save data?')" OnClick="btnSave_Click" />--%>
                                  <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="56px" 
                               OnClientClick="if (!SaveData()) return ;this.disabled=true;this.value = 'wait ...';" CssClass="buttonnorm" />                         
                                <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="closeForm();"
                            UseSubmitBehavior="False" />
                         <%--   <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" Visible="false"
                                onclick="btnpreview_Click" />--%>
                           
                        </td>
                    </tr>
                    <tr>
                    <td colspan="5">&nbsp;</td>
                    </tr>
                    <tr>
                       
                        <td colspan="5">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            <div style="height: 250px; overflow: auto; width: 100%;">
                                <asp:GridView ID="GVWeightPenalityRate" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    OnSelectedIndexChanged="GVWeightPenalityRate_SelectedIndexChanged" DataKeyNames="Id"
                                    OnRowDataBound="GVWeightPenalityRate_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("Item_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="300px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Perc">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromPerc" runat="server" Text='<%#Bind("FromPerc") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Perc">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToPerc" runat="server" Text='<%#Bind("ToPerc") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%#Bind("Rate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>                                                                                
                                        <asp:TemplateField HeaderText="Effective Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#Bind("EffectiveDateFrom","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>                                      
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>' Visible="false"></asp:Label>
                                                 <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("ItemId") %>' Visible="false"></asp:Label>                                                 
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