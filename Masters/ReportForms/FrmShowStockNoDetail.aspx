<%@ Page Title="Show Stock no. Detail" Language="C#" AutoEventWireup="true" CodeFile="FrmShowStockNoDetail.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_ReportForms_FrmShowStockNoDetail"
    EnableEventValidation="false" %>

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
                    <%-- <td>
                            <asp:Button ID="btnStockNo" runat="server" BackColor="White" 
                                    BorderColor="White" BorderWidth="0px" ForeColor="White" Height="0px" Width="0px" />                               
                            </td>--%>
                    <td style="width: 6%">
                        <span class="labelbold">Stock No </span>
                    </td>
                    <td style="width: 55%">
                        <asp:TextBox ID="txtStockNo" runat="server" CssClass="textb" Width="99%" AutoPostBack="true"
                            OnTextChanged="txtStockNo_TextChanged" onFocus="this.select()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="BtnShow" runat="server" Text="Show" CssClass="buttonnorm" OnClick="BtnShow_Click" />
                        <asp:CheckBox ID="chkForBazarSize" Text="For Bazar Size" runat="server" />
                        <asp:CheckBox ID="ChkForStockRawIssueDetail" Text="For Stock Raw Issue" runat="server" />
                        <asp:Button ID="btnconfirm" runat="server" Visible="false" CssClass="buttonnorm"
                            Text="Confirm" OnClick="btnconfirm_Click" />
                        <asp:Button ID="btnPreview" runat="server" Visible="false" CssClass="buttonnorm"
                            Text="Preview" OnClick="btnPreview_Click" />
                    </td>
                </tr>
                <tr id="trStockRemark" runat="server" visible="false">
                    <td style="width: 6%">
                        <span class="labelbold">Remark </span>
                    </td>
                    <td style="width: 55%">
                        <asp:TextBox ID="TxtStockNoRemark" runat="server" CssClass="textb" Width="99%"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="BtnSaveRemark" runat="server" CssClass="buttonnorm" Text="Confirm"
                            OnClick="BtnSaveRemark_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnpack" CssClass="buttonnorm" Text="Direct Pack" runat="server"
                            Visible="false" OnClientClick="return confirm('Do you want to pack this Stock No.?')"
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
                            <asp:GridView ID="DGStock" runat="server" AutoGenerateColumns="False" DataKeyNames="StockNo"
                                OnRowDataBound="DGStock_RowDataBound" Width="100%">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="TStockNo" HeaderText="StockNo">
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PROCESS_NAME" HeaderText="Process_Name">
                                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmpName" HeaderText="Emp_Name">
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IssueChallanNo" HeaderText="IssueChallanNo">
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OrderDate" HeaderText="IssueDate">
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RecChallanNo" HeaderText="RecChallanNo">
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ReceiveDate" HeaderText="ReceiveDate">
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Left" Width="350px" />
                                        <ItemStyle HorizontalAlign="Left" Width="350px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code"></asp:BoundField>
                                    <asp:BoundField DataField="CustomerorderNo" HeaderText="Order No."></asp:BoundField>
                                    <asp:BoundField DataField="LocalOrder" HeaderText="LocalOrder No."></asp:BoundField>
                                    <asp:BoundField DataField="Penality" HeaderText="Penality" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="PenalityRemark" HeaderText="Penality Remark" Visible="false">
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="linkforseeDetail" runat="server" CommandName="" Text="Raw Detail"
                                                OnClick="lnkbtnName_Click" Font-Size="12px" Width="70px">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProcessId" runat="server" Text='<%#Bind("ToProcessId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinishedId" runat="server" Text='<%#Bind("Item_Finished_id") %>'></asp:Label>
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
            <div>
                <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnModelPopup"
                    TargetControlID="btnModalPopUp" DropShadow="true" BackgroundCssClass="modalBackground"
                    CancelControlID="btnCancel" PopupDragHandleControlID="pnModelPopup" OnOkScript="onOk()">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnModelPopup" runat="server" Style="background-color: White; border: 3px solid #0DA9D0;
                    border-radius: 12px; padding: 0" Height="241px" Width="330px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblAgentName" runat="server" Text="" ForeColor="#cc3300" CssClass="labelbold"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="overflow: auto; width: 320px; height: 100px; margin-top: 10px; height: 180px">
                        <asp:GridView ID="GDLinkedtoCustomer" runat="server" Width="290px" Style="margin-left: 10px"
                            ForeColor="#333333" AutoGenerateColumns="False" CssClass="grid-views">
                            <HeaderStyle CssClass="gvheaders" Height="20px" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                            <PagerStyle CssClass="PagerStyle" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                            <Columns>
                                <asp:BoundField DataField="Item_Name" HeaderText="ItemName" />
                                <asp:BoundField DataField="QualityName" HeaderText="Quality" />
                                <asp:BoundField DataField="ShadeColorName" HeaderText="Color" />
                                <asp:BoundField DataField="LotNo" HeaderText="LotNo/batchNo" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <table width="300px">
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnprintstockrawdetail" Text="Print" runat="server" CssClass="buttonnorm"
                                    OnClick="btnprintstockrawdetail_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonnorm" Width="100px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <asp:HiddenField ID="hngridrowindex" Value="0" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnprintstockrawdetail" />
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
