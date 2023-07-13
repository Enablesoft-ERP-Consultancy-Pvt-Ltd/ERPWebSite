<%@ Page Title="Show Stock no. Detail" Language="C#" AutoEventWireup="true" CodeFile="FrmShowStockNoDetailforAnisa.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_ReportForms_FrmShowStockNoDetailforAnisa"
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
            <table width="85%">
                <tr>
                    <%-- <td>
                            <asp:Button ID="btnStockNo" runat="server" BackColor="White" 
                                    BorderColor="White" BorderWidth="0px" ForeColor="White" Height="0px" Width="0px" />                               
                            </td>--%>
                    <td>
                        <asp:Label ID="lblStockNo" runat="server" Text="Stock No" CssClass="labelbold"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStockNo" runat="server" CssClass="textb" Width="500px" Height="20px"
                            OnTextChanged="txtStockNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:Button ID="BtnShow" runat="server" Text="Show" CssClass="buttonnorm" Visible="false"
                            OnClick="BtnShow_Click" />
                    </td>
                    <td>
                        <asp:Button ID="BtnPreviewGridData" runat="server" Visible="false" CssClass="buttonnorm"
                            Text="Preview" OnClick="BtnPreviewGridData_Click" />
                    </td>
                    <td id="TDPackingBarCode" runat="server" visible="false">
                        <asp:Label ID="Label2" runat="server" Text="Packing BarCode" CssClass="labelbold"></asp:Label>
                    </td>
                    <td id="TDTxtBoxPackingBarCode" runat="server" visible="false">
                        <asp:TextBox ID="txtPackingBarCode" runat="server" CssClass="textb" Width="200px"
                            Height="20px" OnTextChanged="txtPackingBarCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                </tr>
                <tr id="Trpack" runat="server" visible="false">
                    <td>
                        <asp:Label ID="lblremark" Text="Remark" CssClass="labelbold" runat="server" Visible="false" />
                    </td>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtremark" CssClass="textb" Width="500px" runat="server" Visible="false" />
                                </td>
                                <td>
                                    <asp:Button ID="btnpack" CssClass="buttonnorm" Text="Direct Pack" runat="server"
                                        OnClientClick="return confirm('Do you want to pack this Stock No.?')" OnClick="btnpack_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="false" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <div style="height: 350px; width: 90%; background-color: Gray; overflow: auto">
                            <asp:GridView ID="DGStock" runat="server" AutoGenerateColumns="False" DataKeyNames="StockNo"
                                OnRowDataBound="DGStock_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" Height="20px" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                  
                                    <%-- <asp:BoundField DataField="TStockNo" HeaderText="StockNo">
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:BoundField>--%>
                                    <%--<asp:BoundField DataField="PROCESS_NAME" HeaderText="Process_Name">
                                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmpName" HeaderText="Emp_Name">
                                        <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IssueChallanNo" HeaderText="IssueChallanNo">
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OrderDate" HeaderText="IssueDate">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RecChallanNo" HeaderText="RecChallanNo">
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ReceiveDate" HeaderText="ReceiveDate">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Left" Width="350px" />
                                        <ItemStyle HorizontalAlign="Left" Width="350px" />
                                    </asp:BoundField>--%>

                                      <asp:TemplateField HeaderText="Stock No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltstockno" Text='<%#Bind("TStockNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Process_Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPROCESS_NAME" Text='<%#Bind("PROCESS_NAME") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Emp_Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpName" Text='<%#Bind("EmpName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IssueChallanNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIssueChallanNoNew" Text='<%#Bind("FolioChallanNo") %>' runat="server" />

                                            <asp:Label ID="lblIssueChallanNo" Text='<%#Bind("IssueChallanNo") %>' runat="server" Visible="false" />                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="IssueDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderDate" Text='<%#Bind("OrderDate") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="RecChallanNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecChallanNo" Text='<%#Bind("RecChallanNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="ReceiveDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceiveDate" Text='<%#Bind("ReceiveDate") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" Text='<%#Bind("Description") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIssueOrderid" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProcessId" runat="server" Text='<%#Bind("ToProcessId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblunitname" Text='<%#Bind("Unitname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loom No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblloomno" Text='<%#Bind("LoomNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cotton LotNo" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTanaLotNo" Text='<%#Bind("TanaLotNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstockstatus" Text='<%#Bind("stockstatus") %>' runat="server" />
                                            <asp:LinkButton ID="lnkchangestatus" runat="server" CommandName="" Text="Change Status"
                                                Font-Size="12px" Width="100px" Visible="false" OnClick="lnkchangestatus_Click">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="linkforseeDetail" runat="server" CommandName="" Text="Raw Detail"
                                                OnClick="lnkbtnName_Click" Font-Size="12px" Width="70px">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinishedId" runat="server" Text='<%#Bind("Item_Finished_id") %>'></asp:Label>
                                            <asp:Label ID="lbltoprocessid" runat="server" Text='<%#Bind("Toprocessid") %>'></asp:Label>
                                            <asp:Label ID="lblreceivedetailid" runat="server" Text='<%#Bind("ReceiveDetailId") %>'></asp:Label>
                                            <asp:Label ID="lblqualitytype" runat="server" Text='<%#Bind("Qualitytype") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <%-- <asp:BoundField DataField="UserName" HeaderText="UserName">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CustomerCode" HeaderText="Cust Code">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>--%>


                                     <asp:TemplateField HeaderText="UserName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" Text='<%#Bind("UserName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cust Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerCode" Text='<%#Bind("CustomerCode") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="ECIS NO & DEST.CODE" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblECISNoDestCode" Text='<%#Bind("Ecisno") %>' runat="server" /> , <asp:Label ID="lblDestCode" Text='<%#Bind("Destcode") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="JobSequence" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LnkBtnJobSequence" runat="server" CommandName="" Text="Job Sequence"
                                                OnClick="LnkBtnJobSequence_Click" Font-Size="12px" Width="70px">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="right" style="width: 90%">
                        <asp:Button ID="btnstocktrace" CssClass="buttonnorm" Text="Stock No. Trace" runat="server"
                            OnClick="btnstocktrace_Click" />
                    </td>
                    <td style="width: 10%">
                    </td>
                </tr>
            </table>
            <table runat="server" id="TBVendorid" visible="false" style="width: 70%">
                <tr>
                    <td>
                        <asp:Label ID="Label1" Text="Vendor Id" runat="server" CssClass="labelbold" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtvendorid" Width="150px" CssClass="textb" runat="server" />
                    </td>
                    <td>
                        <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblvendorname" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblvendoraddress" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
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
                    border-radius: 12px; padding: 0" Height="241px" Width="410px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblAgentName" runat="server" Text="" ForeColor="#cc3300" CssClass="labelbold"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="overflow: auto; width: 410px; height: 100px; margin-top: 10px; height: 180px">
                        <asp:GridView ID="GDLinkedtoCustomer" runat="server" Width="400px" Style="margin-left: 10px"
                            ForeColor="#333333" AutoGenerateColumns="False" CssClass="grid-views" OnRowDataBound="GDLinkedtoCustomer_RowDataBound">
                            <HeaderStyle CssClass="gvheaders" Height="20px" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                            <PagerStyle CssClass="PagerStyle" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                            <Columns>
                                <%--<asp:BoundField DataField="Item_Name" HeaderText="ItemName" />--%>

                                  <asp:TemplateField HeaderText="ItemName" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" Text='<%#Bind("Item_Name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:BoundField DataField="QualityName" HeaderText="Quality" />
                                <asp:BoundField DataField="ShadeColorName" HeaderText="Color" />
                                <asp:BoundField DataField="LotNo" HeaderText="LotNo/batchNo" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lbltagno" Text='<%#Bind("TagNo") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="StockNo" HeaderText="StockNo" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <table width="410px">
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


            <div>
                <asp:Button runat="server" ID="btnModalPopUp2" Style="display: none" />
                <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="pnModelPopup2"
                    TargetControlID="btnModalPopUp2" DropShadow="true" BackgroundCssClass="modalBackground"
                    CancelControlID="btnCancel2" PopupDragHandleControlID="pnModelPopup2" OnOkScript="onOk()">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnModelPopup2" runat="server" Style="background-color: White; border: 3px solid #0DA9D0;
                    border-radius: 12px; padding: 0" Height="241px" Width="410px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="" ForeColor="#cc3300" CssClass="labelbold"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="overflow: auto; width: 410px; height: 100px; margin-top: 10px; height: 180px">
                        <asp:GridView ID="GridViewJobSequence" runat="server" Width="400px" Style="margin-left: 10px"
                            ForeColor="#333333" AutoGenerateColumns="False" CssClass="grid-views">
                            <HeaderStyle CssClass="gvheaders" Height="20px" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                            <PagerStyle CssClass="PagerStyle" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                            <Columns>

                          <%-- <asp:TemplateField HeaderText="ItemName" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" Text='Amit Kumar' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                <%--<asp:BoundField DataField="Item_Name" HeaderText="ItemName" />--%>

                                 <asp:TemplateField HeaderText="Process Name" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProcessName" Text='<%#Bind("PROCESS_NAME") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                               
                                <asp:TemplateField HeaderText="Seq No" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSeqNo" Text='<%#Bind("SeqNo") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>                               
                            </Columns>
                        </asp:GridView>
                    </div>
                    <table width="410px">
                        <tr>
                            <td align="right">
                               <%-- <asp:Button ID="Button2" Text="Print" runat="server" CssClass="buttonnorm"
                                    OnClick="btnprintstockrawdetail_Click" />--%>
                                <asp:Button ID="btnCancel2" runat="server" Text="Cancel" CssClass="buttonnorm" Width="100px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>


            <style type="text/css">
                #mask
                {
                    position: fixed;
                    left: 0px;
                    top: 0px;
                    z-index: 4;
                    opacity: 0.4;
                    -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
                    filter: alpha(opacity=40); /* second!*/
                    background-color: Gray;
                    display: none;
                    width: 100%;
                    height: 100%;
                }
            </style>
            <script type="text/javascript" language="javascript">
                function ShowPopup() {
                    $('#mask').show();
                    $('#<%=pnlpopup.ClientID %>').show();
                }
                function HidePopup() {
                    $('#mask').hide();
                    $('#<%=pnlpopup.ClientID %>').hide();
                }
                $(".btnPwd").live('click', function () {
                    HidePopup();
                });
            </script>            

            <div id="mask">
            </div>
            <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="175px" Width="300px"
                Style="z-index: 111; background-color: White; position: absolute; left: 35%;
                top: 40%; border: outset 2px gray; padding: 5px; display: none">
                <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
                    <tr style="background-color: #8B7B8B; height: 1px">
                        <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                            align="center">
                            ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                                class="btnPwd" href="#">X</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Enter Password:
                        </td>
                        <td>
                            <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px"
                                OnTextChanged="txtpwd_TextChanged" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="right">
                            <input type="button" value="Cancel" class="btnPwd" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label7" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:HiddenField ID="hngridrowindex" Value="0" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnprintstockrawdetail" />
            <asp:PostBackTrigger ControlID="btnstocktrace" />
            <asp:PostBackTrigger ControlID="BtnPreviewGridData" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
