<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MasterDyerColorRate.ascx.cs" Inherits="UserControls_MasterDyerColorRate" %>
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
            <div style="margin-left: 15%; margin-right: 15%">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                        &nbsp;
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" width="40%" style="text-align:right">
                            <asp:Label ID="lblcolorname" runat="server" Text="Dyer Name" CssClass="labelbold"></asp:Label>
                             <asp:DropDownList CssClass="dropdown" ID="DDDyerName" Width="150px" runat="server"
                              AutoPostBack="true" OnSelectedIndexChanged="DDDyerName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="40%" style="text-align:left; padding-left:10px">
                         <asp:Label ID="Label1" runat="server" Text="ShadeColor Name" CssClass="labelbold"></asp:Label>
                           <asp:DropDownList CssClass="dropdown" ID="DDShadeColor" Width="150px" runat="server"
                           AutoPostBack="true" OnSelectedIndexChanged="DDShadeColor_SelectedIndexChanged">
                            </asp:DropDownList>
                            
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="tdstyle" width="40%" style="text-align:right">
                            <asp:Label ID="Label2" runat="server" Text="Rate" CssClass="labelbold"></asp:Label>
                             <asp:TextBox CssClass="textb" ID="txtRate" runat="server"></asp:TextBox>
                        </td>
                        <td width="40%" style="text-align:left; padding-left:10px">
                       <asp:Label ID="Label3" runat="server" Text="Effective Date" CssClass="labelbold"></asp:Label>
                           <asp:TextBox ID="txtEffectiveDate" runat="server"  CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtEffectiveDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                   <tr>
                   <td colspan="2">&nbsp;</td>
                   </tr>
                    <tr>
                       <td style="text-align: left; padding-left:250px" colspan="2">
                         
                           <%-- <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" Width="53px"
                                OnClientClick="return confirm('Do you want to save data?')" OnClick="btnSave_Click" />--%>
                                  <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="56px" 
                               OnClientClick="if (!SaveData()) return ;this.disabled=true;this.value = 'wait ...';" CssClass="buttonnorm" />
                          <%--  <asp:Button ID="btnClear" Text="Close" runat="server" CssClass="buttonnorm" Width="53px"
                                OnClientClick="return CloseForm();" />--%>
                                <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="closeForm();"
                            UseSubmitBehavior="False" />
                            <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" Visible="false"
                                onclick="btnpreview_Click" />
                           
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                       
                        <td colspan="2">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            <div style="height: 250px; overflow: auto; width: 100%;">
                                <asp:GridView ID="gdDyerColorRate" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    OnSelectedIndexChanged="gdDyerColorRate_SelectedIndexChanged" DataKeyNames="Id"
                                    OnRowDataBound="gdDyerColorRate_RowDataBound">
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
                                        <asp:TemplateField HeaderText="Dyer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDyerName" runat="server" Text='<%#Bind("EmpName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="300px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shade Color">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShadeColor" runat="server" Text='<%#Bind("ShadeColorName") %>'></asp:Label>
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
                                                <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#Bind("EffectiveDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>                                      
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>' Visible="false"></asp:Label>
                                                 <asp:Label ID="lblDyerId" runat="server" Text='<%# Bind("DyerId") %>' Visible="false"></asp:Label>
                                                  <asp:Label ID="lblDyerShadeColorId" runat="server" Text='<%# Bind("DyerShadeColorId") %>' Visible="false"></asp:Label>
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