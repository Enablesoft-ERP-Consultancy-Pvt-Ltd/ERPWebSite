<%@ Page Title="Show Map Trance Stock no. Detail" Language="C#" AutoEventWireup="true"
    CodeFile="FrmShowMapTraceStockNoDetail.aspx.cs" MasterPageFile="~/ERPmaster.master"
    Inherits="Masters_ReportForms_FrmShowMapTraceStockNoDetail" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(btn) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById(btn).click();
            }
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>                   
                    <td style="width: 6%">
                        <span class="labelbold">Stock No </span>
                    </td>
                    <td style="width: 55%">
                        <asp:TextBox ID="txtStockNo" runat="server" CssClass="textb" Width="99%" AutoPostBack="true"
                            OnTextChanged="txtStockNo_TextChanged" onFocus="this.select()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="BtnShow" runat="server" Text="Show" CssClass="buttonnorm" OnClick="BtnShow_Click" />
                       
                        <asp:Button ID="btnPreview" runat="server" Visible="false" CssClass="buttonnorm"
                            Text="Preview" OnClick="btnPreview_Click" />
                    </td>
                </tr>
                <tr id="trStockRemark" runat="server" visible="false">                   
                    <td>
                        <asp:Button ID="btnpack" CssClass="buttonnorm" Text="StockOut" runat="server"
                            Visible="false" OnClientClick="return confirm('Do you want to stockout this Stock No.?')"
                            OnClick="btnpack_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="false" CssClass="labelbold"
                            ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <div style="height: 350px; width: 100%; background-color: Gray; overflow: auto">
                            <asp:GridView ID="DGStock" runat="server" AutoGenerateColumns="False" DataKeyNames="MAPSTENCILNO"
                                OnRowDataBound="DGStock_RowDataBound" Width="100%">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="MTStockNo" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMSSTOCKNO" Text='<%#Bind("MSSTOCKNO") %>' runat="server" />
                                        </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemname" Text='<%#Bind("Item_Name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quality" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuality" Text='<%#Bind("QualityName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Design" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesign" Text='<%#Bind("Designname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Color" HeaderStyle-HorizontalAlign="Left"> 
                                        <ItemTemplate>
                                            <asp:Label ID="lblColor" Text='<%#Bind("Colorname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shape" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShapename" Text='<%#Bind("shapename") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Size" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSize" Text='<%#Bind("Size") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FolioChallanNo" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFolioChallanNo" Text='<%#Bind("FolioChallanNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="EmpName" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpName" Text='<%#Bind("EmpName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPackStatus" runat="server" Text='<%#Bind("PackStatus") %>'></asp:Label>
                                            <asp:Label ID="lblIssueOrderId" runat="server" Text='<%#Bind("IssueOrderId") %>'></asp:Label>
                                            <asp:Label ID="lblIssueId" runat="server" Text='<%#Bind("IssueId") %>'></asp:Label>
                                            <asp:Label ID="lblIssueDetailId" runat="server" Text='<%#Bind("IssueDetailId") %>'></asp:Label>
                                            <asp:Label ID="lblReceiveId" runat="server" Text='<%#Bind("ReceiveId") %>'></asp:Label>
                                            <asp:Label ID="lblReceiveDetailId" runat="server" Text='<%#Bind("ReceiveDetailId") %>'></asp:Label>
                                            <asp:Label ID="lblIssRecStatus" runat="server" Text='<%#Bind("IssRecStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Font-Size="Small" Text=""
                            runat="server" />
                    </td>
                </tr>
            </table>            
            <asp:HiddenField ID="hngridrowindex" Value="0" runat="server" />
        </ContentTemplate>
        <Triggers>            
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
