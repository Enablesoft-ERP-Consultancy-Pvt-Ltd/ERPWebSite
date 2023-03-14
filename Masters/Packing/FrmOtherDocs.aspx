<%@ Page Title="Other Documents" Language="C#" MasterPageFile="~/ERPmaster.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="FrmOtherDocs.aspx.cs" Inherits="Masters_Packing_FrmOtherDocs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function Validate() {
            if (document.getElementById("<%=DDcmpName.ClientID %>").value == "0") {
                alert("Please Select Company Name!");
                document.getElementById("<%=DDcmpName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDSession.ClientID %>").value == "0") {
                alert("Please Select Session! ");
                document.getElementById("<%=DDSession.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDInvoiceNo.ClientID %>").value == "0") {
                alert("Please Select Invoice No!");
                document.getElementById("<%=DDInvoiceNo.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }

        }
        function Validate1() {
            if (document.getElementById("<%=DDInvoiceNo.ClientID %>").value == "0") {
                alert("Please Select Invoice No!");
                document.getElementById("<%=DDInvoiceNo.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDSession.ClientID %>").value == "0") {
                alert("Please Select Session! ");
                document.getElementById("<%=DDSession.ClientID %>").focus();
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <asp:UpdatePanel ID="Up" runat="server">
        <ContentTemplate>
            <table width="70%">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    In Company Name
                                </td>
                                <td>
                                    Session
                                </td>
                                <td>
                                    Invoice No.
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="DDcmpName" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDSession" runat="server" Width="150px" CssClass="dropdown"
                                        OnSelectedIndexChanged="DDSession_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDInvoiceNo" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDInvoiceNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    Document Type
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDDoctype" runat="server" Width="150px" CssClass="dropdown">
                                        <asp:ListItem Value="1">Collection</asp:ListItem>
                                        <asp:ListItem Value="2">Purchase</asp:ListItem>
                                        <asp:ListItem Value="3">Negotiation</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <div id="Div1" runat="server">
                            <asp:Label ID="LblPcs" runat="server" Text="Pcs        :- " CssClass="labelbold"></asp:Label>
                            <asp:Label ID="LblPcs1" runat="server" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:Label ID="LblRolls" runat="server" Text="Rolls    :-      " CssClass="labelbold"></asp:Label>
                            <asp:Label ID="LblRolls1" runat="server" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:Label ID="LblArea" runat="server" Text="Area  :-        " CssClass="labelbold"></asp:Label>
                            <asp:Label ID="LblArea1" runat="server" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:Label ID="LblAmt" runat="server" Text="Amount:- " CssClass="labelbold"></asp:Label>
                            <asp:Label ID="LblAmt1" runat="server" CssClass="labelbold"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:LinkButton ID="LnkForeignDocBill" runat="server" Text="Foreign Doc Bill" ForeColor="#0000CC"
                Font-Bold="true" OnClick="LnkForeignDocBill_Click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LnkBankLetter" runat="server" Text="Bank Letter" ForeColor="#0000CC"
                Font-Bold="true" OnClick="LnkBankLetter_Click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LnkExpValDec" runat="server" Text="Export Value Declaration"
                ForeColor="#0000CC" Font-Bold="true" OnClick="LnkExpValDec_Click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LnkAnxI" runat="server" Text="Annexure-I" ForeColor="#0000CC"
                Font-Bold="true" OnClick="LnkAnxI_Click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LnkBnkLetter2" runat="server" Text="Bank Letter 2" ForeColor="#0000CC"
                Font-Bold="true" OnClick="LnkBnkLetter2_Click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LnkGR" runat="server" Text="GR Release" ForeColor="#0000CC" Font-Bold="true"
                OnClick="LnkGR_Click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LnkOrigDoc" runat="server" Text="Original Doc" ForeColor="#0000CC"
                Font-Bold="true" OnClick="LnkOrigDoc_Click"></asp:LinkButton>
            <asp:Panel ID="PnlForeignDocBill" runat="server" Visible="true" Width="90%">
                <br />
                <strong>Doc Date:</strong>
                <%--<asp:Label ID="LblDocDate" runat="server" Text="Doc Date" ForeColor="Red" CssClass="labelbold"></asp:Label>--%>
                <asp:TextBox ID="txtDocDate" runat="server" CssClass="textb"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                    TargetControlID="txtDocDate">
                </asp:CalendarExtender>
                <br />
                <table cellpadding="2" cellspacing="3" border="0" width="70%">
                    <tr class="RowStyle">
                        <td style="width: 100px">
                            <strong>Document</strong>
                        </td>
                        <td>
                            Drafts
                        </td>
                        <td>
                            Cust Invoice
                        </td>
                        <td>
                            Cuns. Invoice
                        </td>
                        <td>
                            Cert. Of Origin
                        </td>
                        <td>
                            Weight Note
                        </td>
                        <td>
                            Insp Olley
                        </td>
                        <td>
                            B/L
                        </td>
                        <td>
                            Cert. Analysis
                        </td>
                        <td>
                            Pack List
                        </td>
                        <td>
                            S.C.D
                        </td>
                        <td>
                            Other Document
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td style="width: 300px;">
                            <strong>Original By Courier</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtOrgDraft" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtORGCustinvoice" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtORGCunsInv" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtORGCertofOrigin" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtORGWeight" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtORGInspOlley" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtORGBl" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtORGCertAnalysis" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtORGPackList" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtORGSCD" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtOrgOthDoc" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            <strong>Duplicate By Air Mail</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupDraft" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupCustinvoice" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupCunsInv" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupCertofOrigin" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupWeight" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupInspOlley" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupBL" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupCertAnalysis" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupPackList" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupSCD" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDupOthDoc" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlBankLetter" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="1" border="0" width="75%">
                    <tr class="RowStyle">
                        <td>
                        </td>
                        <td>
                            <strong>No Of Copies</strong>
                        </td>
                        <td>
                        </td>
                        <td>
                            <strong>No Of Copies</strong>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            Invoice no.
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPBLInvno" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            S.Bill No.
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPBLSBillno" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            Doc. Date
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPBLDocDate" runat="server" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalExt1" runat="server" Format="dd-MMM-yyyy" TargetControlID="TxtPBLDocDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            Specification List
                        </td>
                        <td>
                            <asp:TextBox ID="TxtSpecificationList" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            Form SDF Appendix-I
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPBLSdfAppendix" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            Bill Of Exchange
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPBLBillExchange" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            Exchange Control Copy
                        </td>
                        <td>
                            <asp:TextBox ID="txtPBLExchCtrlCopy" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            B/L No.
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPBLBillno" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            Single Country Declaration
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPBLSingContDec" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlExportValDeclaration" runat="server" Visible="false">
                <table>
                    <tr class="RowStyle">
                        <td>
                            <asp:Label ID="LblconsSale" Text="Sale on Consignment Basis" runat="server" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:DropDownList ID="DDConsSale" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="2">No</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp; &nbsp;
                            <asp:Label ID="LblSeller" Text="Whether seller_Buyer are related" runat="server"
                                CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:DropDownList ID="DDSeller" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="2">No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            <asp:Label ID="LblPrice" Text="If Yes, whether relationship has influenced the price"
                                runat="server" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:DropDownList ID="DDPrice" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="2">No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            <asp:Label ID="LblDocDate" Text="Doc. Date" runat="server" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:TextBox ID="PnlETxtdocdate" runat="server" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalExDocDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="PnlETxtdocdate">
                            </asp:CalendarExtender>
                            &nbsp; &nbsp;
                            <asp:Label ID="LblPlace" Text="Place" runat="server" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:TextBox ID="TxtPlace" runat="server" Text="BHADOHI" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlAnnexure1" runat="server" Visible="false">
                <table cellpadding="3" cellspacing="2" border="0">
                    <tr class="RowStyle">
                        <td>
                            Range
                            <br />
                            <asp:TextBox ID="TxtPAnxRange" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            Division
                            <br />
                            <asp:TextBox ID="TxtPAnxDivision" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            Commision Rate
                            <br />
                            <asp:TextBox ID="TxtPANXCommision" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlBnkLetter2" runat="server" Visible="false">
                <table cellpadding="2" cellspacing="3" border="0" width="70%">
                    <tr class="RowStyle">
                        <td>
                            <strong>Document</strong>
                        </td>
                        <td>
                            Drafts
                        </td>
                        <td>
                            Invoice
                        </td>
                        <td>
                            Pack List
                        </td>
                        <td>
                            B/L
                        </td>
                        <td>
                            AWB
                        </td>
                        <td>
                            Cert.Origin GSP
                        </td>
                        <td>
                            Inspol/Cer
                        </td>
                        <td>
                            S.C.D
                        </td>
                        <td>
                            Other Document
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            <strong>Original</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDrafts" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLInv" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLPackList" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLBill" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLAWB" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLCertOrigin" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLInspol" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLSCD" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLOthDoc" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            <strong>Duplicate</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDupDrafts" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDupInv" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDupPackList" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDupBill" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDupAWB" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDupCertOrigin" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDupInspol" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDupSCD" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBLDupOthDoc" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="LblPBLDocDate" runat="server" Text="Doc Date" CssClass="labelbold"></asp:Label>
                <asp:TextBox ID="TxtPBL2DocDate" runat="server" CssClass="textb"></asp:TextBox>
                <asp:CalendarExtender ID="CalEx" runat="server" TargetControlID="TxtPBL2DocDate"
                    Format="dd-MMM-yyyy">
                </asp:CalendarExtender>
            </asp:Panel>
            <asp:Panel ID="PnlGRRelease" runat="server" Visible="false">
                <table cellpadding="2" cellspacing="2" border="0">
                    <tr class="RowStyle">
                        <td>
                            <br />
                            Invoice
                        </td>
                        <td>
                            <strong>No.Of Copies </strong>
                            <br />
                            <asp:TextBox ID="TxtPGRInv" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            RTT Date
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPGRTTDate" runat="server" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtPGRTTDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            AWB
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPGRAWB" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            SDF Form
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPGRSDF" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            Exchange Control
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPGRExchangeControl" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            RTT No
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPGRTTNo" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlOriginalDoc" runat="server" Visible="false">
                <table cellpadding="2" cellspacing="2" border="0">
                    <tr class="RowStyle">
                        <td>
                            <br />
                            Invoice
                        </td>
                        <td>
                            <strong>No. Of Copies</strong>
                            <br />
                            <asp:TextBox ID="TxtOriginalDoc" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            AWB No.
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAWBNo" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            Packing List
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPkgList" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            Date
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPnlOrigDocDate" runat="server" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtPnlOrigDocDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            Courier Co
                        </td>
                        <td>
                            <asp:TextBox ID="TxtCourierCo" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button ID="btnPrintBankLetr" Text="Print Bank Letter" runat="server" CssClass="buttonnorm"
                Width="120px" OnClick="btnPrintBankLetr_Click" OnClientClick="return Validate1();" />
            <asp:Button ID="BtnPrintFBill" Text="Print F.Bill" Width="120px" runat="server" CssClass="buttonnorm"
                OnClick="BtnPrintFBill_Click" OnClientClick="return Validate1();" />
            <asp:Button ID="Btnsave" Text="Save" runat="server" CssClass="buttonnorm" Width="70px"
                OnClientClick="return Validate();" OnClick="Btnsave_Click" />
            <asp:Button ID="BtnClose" Text="Close" runat="server" Width="70px" CssClass="buttonnorm"
                OnClientClick="return CloseForm();" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
