<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmOrderAddProcess.aspx.cs"
    Inherits="Masters_Order_FrmOrderAddProcess" Title="Order Add Process" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "FrmOrderAddProcess.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <div id="maindiv">
        <asp:UpdatePanel ID="updatepanal" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            COMPANY NAME*
                            <br />
                            <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="250px" TabIndex="0"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            CUSTOMER CODE*
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                                Width="250px" TabIndex="2" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            ORDER NO*
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCustOrderNo" runat="server" AutoPostBack="True"
                                Width="250px" OnSelectedIndexChanged="DDCustOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div style="width: 100%; height: 300px; overflow: scroll">
                                <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                    DataKeyNames="OrderID" OnRowDataBound="DGOrderDetail_RowDataBound" OnRowCreated="DGOrderDetail_RowCreated"
                                    CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" ForeColor="White" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="ITEM_NAME" HeaderText="ITEM NAME">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="QualityName" HeaderText="Quality Name">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DesignName" HeaderText="Design Name">
                                            <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ColorName" HeaderText="Color Name">
                                            <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OQty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="true" HeaderText="AddProcess">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlProcessName" runat="server" NavigateUrl='<%# "../Order/AddProcessName.aspx?OrderID=" + Eval("OrderID") + "&CategoryID=" + Eval("CategoryID") + "&ItemID=" + Eval("ItemID") + "&QualityID=" + Eval("QualityID") + "&DesignID=" + Eval("DesignID") + "&ColorID=" + Eval("ColorID")%>'
                                                    Target="_blank" Text="ADD PROCESS" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <div style="float: right;">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="BtnNew" runat="Server" Text="New" OnClientClick="return reloadPage();"
                                    CssClass="buttonnorm" />
                                <%--<asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return ValidateSave();"
                                    CssClass="buttonnorm" />--%>
                                <%-- <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                    Visible="true" CssClass="buttonnorm preview_width" />--%>
                                <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                    CssClass="buttonnorm" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
