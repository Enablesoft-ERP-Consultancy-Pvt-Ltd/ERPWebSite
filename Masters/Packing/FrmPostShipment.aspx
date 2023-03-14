<%@ Page Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmPostShipment.aspx.cs" Inherits="Masters_Packing_FrmPostShipment"
    Title="POST SHIPMENT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Validate(id) {
            if (document.getElementById("<%=DDSession.ClientID %>").value == "0") {
                alert("Please Select Session! ");
                document.getElementById("<%=DDSession.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDInvoiceNo.ClientID %>").value == "0") {
                alert("Please Select Invoice No!");
                document.getElementById("<%=DDInvoiceNo.ClientID %>").focus();
                return false;
            }
            else if (id == "1") {
                if (document.getElementById("<%=txtinvRecAmt.ClientID %>").value == "") {
                    alert("Invoice Receive Amt. can not be blank!");
                    document.getElementById("<%=txtinvRecAmt.ClientID %>").focus();
                    return false;
                }
                else {
                    return confirm('Do You Want To Save?')
                }
            }
            else if (id == "2") {
                if (document.getElementById("<%=txtInvDrRecAmt.ClientID %>").value == "") {
                    alert("Invoice drawback Rec. Amt. can not be blank!");
                    document.getElementById("<%=txtInvDrRecAmt.ClientID %>").focus();
                    return false;
                }
                else {
                    return confirm('Do You Want To Save?')
                }
            }
            else {
                return confirm('Do You Want To Save?')
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="90%">
                <tr>
                    <td>
                        <table>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">Session:</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDSession" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDSession_SelectedIndexChanged" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <span class="labelbold">Invoice No.:</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDInvoiceNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDInvoiceNo_SelectedIndexChanged" Width="120px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">Document In</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDDocType" runat="server" CssClass="dropdown" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <span class="labelbold">Open Policy No.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpolicyno" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                                <%--
                                --%></tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">DBK. Amt.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDBKAmt" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">Letter Credit No.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtLetterCreditNo" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">DBK US $/Euro Ex. Rate</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExRate" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">Credit No.Date</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCRNoDate" runat="server" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalExtCRNo" runat="server" Format="dd-MMM-yyyy" TargetControlID="TxtCRNoDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">Shipping Bill No.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtshpBillNo" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">Performa Invoice No.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPerfInvno" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">Shipping Bill Date.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtShpBillDate" runat="server" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalExtShpBillDate" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="TxtShpBillDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <span class="labelbold">Performa Invoice Date.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPerfInvDate" runat="server" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalExPInvDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtPerfInvDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">Vessel Name</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtvesselName" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">Type Of Package</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPkgType" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">BRC US $/Euro Ex. Rate</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBRCExrate" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">Container</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContainer" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">BL/AWB No.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBlNo" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">Container No</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContinerNo" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">BL/AWB Date.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBlDate" runat="server" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtBlDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <span class="labelbold">Seal No</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSealNo" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">Cr Days</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCrdays" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">FIRC No</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFircno" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">FIRC Date</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFIRCDate" runat="server" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtFIRCDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <span class="labelbold">Form "A" No</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFormANo" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">Form "A" Date</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtformADate" runat="server" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtformADate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <span class="labelbold">Agent Commission</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCommission" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">License Commission</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLicenseCommission" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">Bank Submission Date</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbanksubmissionDate" runat="server" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtbanksubmissionDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">Bank Submission Reference No.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBankSubmissionRefNo" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">LEO Date</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLEODate" runat="server" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtLEODate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                             <tr class="RowStyle">
                                <td>
                                    <span class="labelbold">Flight No.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFlightNo" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">Flight Date</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFlightDate" runat="server" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender7" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtFlightDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                            <td>
                             <asp:Label ID="LblErrorMessage" runat="server" Text="ErrorMessage" Visible="false"
                                            ForeColor="Red"></asp:Label>
                            </td>
                           
                            </tr>

                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <table>

                                    <tr>
                                    <td><asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="Total Invoice Amt"></asp:Label><br />
                                    <asp:Label ID="lblTotalInvoiceAmt" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp</td>
                                    </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblInvrecAmt" runat="server" CssClass="labelbold" Text="Invoice Rec. Amt."></asp:Label><br />
                                                <asp:TextBox ID="txtinvRecAmt" runat="server" Width="100px" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRTTNo" runat="server" CssClass="labelbold" Text="RTT No."></asp:Label><br />
                                                <asp:TextBox ID="txtRttno" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;Date<br />
                                                <asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalExt" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtdate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="Div1" runat="server" style="width: 433px; overflow: auto">
                                        <asp:GridView ID="GDVAmtDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="invoiceid"
                                            OnSelectedIndexChanged="GDVAmtDetail_SelectedIndexChanged" OnRowDataBound="GDVAmtDetail_RowDataBound"
                                            Width="413px">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <Columns>
                                                <asp:BoundField DataField="invoiceid" HeaderText="" Visible="false" />
                                                <asp:BoundField DataField="id" HeaderText="ID">
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Rec Amt.">
                                                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtRecAmt" Width="100px" align="right" runat="server" Text='<%#Bind ("amt") %>'
                                                            onkeypress="return isNumber(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rec Date">
                                                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtRecDate" Width="100px" align="right" runat="server" Text='<%#Bind ("recamtdate") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RTT No">
                                                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtRTTNo" Width="100px" align="right" runat="server" Text='<%#Bind ("RTTNo") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                        </asp:GridView>
                                        &nbsp;
                                        <asp:Button ID="BtnSaveReAmt" Text="Save Rec Amt." runat="server" Width="100px" CssClass="buttonnorm"
                                            OnClick="BtnSaveReAmt_Click" OnClientClick="return Validate(1);" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblInvDrRecAmt" runat="server" CssClass="labelbold" Text="Invoice Drawback Rec. Amt."></asp:Label>
                                                &nbsp;
                                                <asp:TextBox ID="txtInvDrRecAmt" runat="server" Width="150px" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp; &nbsp;<asp:Label runat="server" CssClass="labelbold" Text="Date"></asp:Label>&nbsp;&nbsp;
                                                <asp:TextBox ID="txtInvDrRecAmtDate" runat="server" Width="80px" CssClass="textb"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="txtInvDrRecAmtDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="GDVAmtDetail2" runat="server" AutoGenerateColumns="False" DataKeyNames="invoiceid"
                                        OnSelectedIndexChanged="GDVAmtDetail2_SelectedIndexChanged" OnRowDataBound="GDVAmtDetail2_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:BoundField DataField="invoiceid" HeaderText="" Visible="false" />
                                            <asp:BoundField DataField="id" HeaderText="ID">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Rec Amt.">
                                                <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtDBKRecAmt" Width="100px" align="right" runat="server" Text='<%#Bind ("DBKAmt") %>'
                                                        onkeypress="return isNumber(event);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rec Date">
                                                <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtDBKRecDate" Width="100px" align="right" runat="server" Text='<%#Bind ("recamtdate") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RTT No" Visible="false">
                                                <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtDBKRTTNo" Width="100px" align="right" runat="server" Text='<%#Bind ("RTTNo") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                    </asp:GridView>
                                    &nbsp;
                                    <asp:Button ID="BtnSaveDrRecAmt" Text="Save Drawback Rec Amt." Width="200px" runat="server"
                                        CssClass="buttonnorm" OnClick="BtnSaveDrRecAmt_Click" OnClientClick="return Validate(2);" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="BtnSave" Text="Save" Width="80px" runat="server" CssClass="buttonnorm"
                            OnClick="BtnSave_Click" OnClientClick="return Validate(0);" />
                        <asp:Button ID="BtnClose" Text="Close" Width="80px" runat="server" CssClass="buttonnorm"
                            OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
