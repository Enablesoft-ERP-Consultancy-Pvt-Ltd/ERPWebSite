<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddLegalinformation.aspx.cs"
    Inherits="Masters_Campany_AddLegalinformation" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Legal Information</title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmCompanyInfo.aspx";
        }
        function report() {
            window.open('../../ReportViewer.aspx', '');
            // window.open('AddItemt.aspx','popup Form', 'Width=550px, Height=400px') 
        }

        function AddBank() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddBank.aspx', '', 'width=950px,Height=500px');
            }
        }
        function Addsign() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('frmSignature.aspx', '', 'width=500px,Height=300px');
            }
        }
        function closeAddlegalinformation() {
            self.close();
        }

        function detail() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('Detail.aspx', '', 'width=500px,Height=300px');
            }
        }
        function doConfirm() {
            var r = confirm("Do you want to delete ?");
            document.getElementById("hnforDelete").value = r;
            if (r == true) {

                return true;
            }
            else {

                return false;
            }

        }
        function doConfirmdownload() {
            var r = confirm("Do you want to download ?");
            document.getElementById("hnfordownload").value = r;
            if (r == true) {

                return true;
            }
            else {

                return false;
            }
        }

    </script>
</head>
<body>
    <form id="addlegalinformation" runat="server">
    <div>
        <table>
            <tr id="trLegal" runat="server" visible="false">
                <td colspan="2" class="tdstyle">
                    Legal Name
                </td>
                <td colspan="3">
                    <asp:TextBox CssClass="textb" ID="txtlegalName" runat="server" Width="80%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="please Enter Legalname"
                        ControlToValidate="txtlegalName" ValidationGroup="s" ForeColor="Red">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2" runat="server" visible="false">
                    Register Address
                </td>
                <td runat="server" runat="server" visible="false">
                    <asp:TextBox ID="txtregisteraddress" runat="server" CssClass="textb" TextMode="MultiLine"
                        Width="253px"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    Legal status
                </td>
                <td>
                    <asp:DropDownList ID="DDlegalstatus" runat="server" CssClass="dropdown" Width="125px">
                        <asp:ListItem Value="1">Partner</asp:ListItem>
                        <asp:ListItem Value="2">Proprietor</asp:ListItem>
                        <asp:ListItem Value="3">Co-Operative</asp:ListItem>
                        <asp:ListItem Value="4">Pvt.Ltd</asp:ListItem>
                        <asp:ListItem Value="5">PublicLimited</asp:ListItem>
                        <asp:ListItem Value="5">Society</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="tdstyle">
                    Date of Establishment
                </td>
                <td>
                    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                    </asp:ToolkitScriptManager>
                    <asp:TextBox ID="txtdoe" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                    <asp:CalendarExtender ID="calenderExtendertxtdoe" runat="server" TargetControlID="txtdoe"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    Incorporation Number
                </td>
                <td>
                    <asp:TextBox ID="txtin" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    Place of Incorporation
                </td>
                <td>
                    <asp:TextBox ID="txtpoi" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    Tin No
                </td>
                <td>
                    <asp:TextBox ID="txttinno" runat="server" CssClass="texbox" Width="125px">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    Association Name
                </td>
                <td>
                    <asp:TextBox ID="txtassociationname" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                </td>
                <td>
                    Registration No
                </td>
                <td>
                    <asp:TextBox ID="txtregistrationno" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    REG Date
                </td>
                <td>
                    <asp:TextBox ID="txtregdate" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtregdate" runat="server" TargetControlID="txtregdate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    REG EXP Date
                </td>
                <td>
                    <asp:TextBox ID="txtregexdate" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtregexdate" runat="server" TargetControlID="txtregexdate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    IEC Number
                </td>
                <td>
                    <asp:TextBox ID="txtiecnumber" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    Issuing Authority
                </td>
                <td>
                    <asp:TextBox ID="txtissauthority" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    Commissionrate
                </td>
                <td>
                    <asp:TextBox ID="txtcr" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                </td>
                <td>
                    Circle
                </td>
                <td>
                    <asp:TextBox ID="txtcircle" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                </td>
                <td>
                    Bank Name
                </td>
                <td>
                    <asp:DropDownList ID="DDbankname" runat="server" CssClass="dropdown" Width="125px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    PAN/GIR No
                </td>
                <td>
                    <asp:TextBox ID="txtpanno" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                </td>
                <td>
                    Date of Issue
                </td>
                <td>
                    <asp:TextBox ID="txtpandate" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtpandate" runat="server" TargetControlID="txtpandate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Issuing Authority
                </td>
                <td>
                    <asp:TextBox ID="txtissuingauth" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    SSI Number
                </td>
                <td>
                    <asp:TextBox ID="txtssino" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    Date of Issue
                </td>
                <td>
                    <asp:TextBox ID="txtssidate" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtssidate" runat="server" TargetControlID="txtssidate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Issue Authority
                </td>
                <td>
                    <asp:TextBox ID="txtissueauth" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    Industrial Licence/Iem
                </td>
                <td>
                    <asp:TextBox ID="txtil" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    Date of Issue
                </td>
                <td>
                    <asp:TextBox ID="txtindustrialdate" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtindustrialdate" runat="server" TargetControlID="txtindustrialdate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Issuing Authority
                </td>
                <td>
                    <asp:TextBox ID="txtissuingauthority" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    CST No
                </td>
                <td>
                    <asp:TextBox ID="txtcstno" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                </td>
                <td>
                    Date of issue
                </td>
                <td>
                    <asp:TextBox ID="txtcstdate" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtcstdate" runat="server" TargetControlID="txtcstdate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Service Tax Reg No
                </td>
                <td>
                    <asp:TextBox ID="txtservicetaxregno" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    Service Tax No
                </td>
                <td>
                    <asp:TextBox ID="txtstno" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                </td>
                <td>
                    Date of Issue
                </td>
                <td>
                    <asp:TextBox ID="txtstdate" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendearExtendertxtstdate" runat="server" TargetControlID="txtstdate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Service Tax Reg Date
                </td>
                <td>
                    <asp:TextBox ID="txtservicetrd" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtservicetrd" runat="server" TargetControlID="txtservicetrd"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    GSP Registration No
                </td>
                <td>
                    <asp:TextBox ID="txtgsprno" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    Date of Issue
                </td>
                <td>
                    <asp:TextBox ID="txtgspdate" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtgspdate" runat="server" TargetControlID="txtgspdate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Place
                </td>
                <td>
                    <asp:TextBox ID="txtplace" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    GSP Account No
                </td>
                <td>
                    <asp:TextBox ID="txtgspaccountno" runat="server" CssClass="textb" Width="130px"></asp:TextBox>
                </td>
                <td>
                    Date of Issue
                </td>
                <td>
                    <asp:TextBox ID="txtgspacdate" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtgspacdate" runat="server" TargetControlID="txtgspacdate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Place
                </td>
                <td>
                    <asp:TextBox ID="txtplac" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle" colspan="2">
                    Type of Certificate
                </td>
                <td>
                    <asp:TextBox ID="txttypeoc" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    Issue Date
                </td>
                <td>
                    <asp:TextBox ID="txtcertificatedate" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtcertificatedate" runat="server" TargetControlID="txtcertificatedate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Certificate No
                </td>
                <td>
                    <asp:TextBox ID="txtcertificateno" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    Validity Date
                </td>
                <td>
                    <asp:TextBox ID="txtcertificatevaldate" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtendertxtcertificatevaldate" runat="server" TargetControlID="txtcertificatevaldate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr id="TRRex" runat="server" visible="false">
                <td class="tdstyle" colspan="2">
                    RexNo
                </td>
                <td>
                    <asp:TextBox ID="txtRexNo" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                </td>
                <td>
                    Issue Date
                </td>
                <td>
                    <asp:TextBox ID="txtRexIssueDate" runat="server" CssClass="textb" Width="125px">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtRexIssueDate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Expiry Date
                </td>
                <td>
                    <asp:TextBox ID="txtRexExpiryDate" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtRexExpiryDate"
                        Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Issue BodyNo
                </td>
                <td>
                    <asp:TextBox ID="txtIssueBodyNo" runat="server" CssClass="textb" Width="125px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="fileupload" />
                    <asp:Button runat="server" ID="UploadButton" Text="Upload" CssClass="fileupload"
                        OnClick="UploadButton_Click" />
                    <asp:Button ID="btndownload" runat="server" Text="Download" Visible="true" />
                    <br />
                    <br />
                    <asp:Label runat="server" ID="StatusLabel" CssClass="labelbold" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td class="style6">
                </td>
                <td style="text-align: right" colspan="5">
                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnSave_Click" />
                    <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return closeAddlegalinformation();" />
                    <asp:Button ID="btndetail" runat="server" CssClass="buttonnorm" Text="DETAIL" OnClientClick="return detail();" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblErr" runat="server" CssClass="labelbold" ForeColor="Red" />
                </td>
            </tr>
        </table>
    </div>
    <table>
        <tr>
            <td>
                <div style="width: 900px; height: 200px; overflow: scroll">
                    <asp:GridView ID="Dglegal" runat="server" Width="864px" EmptyDataText="No Data Found!"
                        AutoGenerateColumns="False" CssClass="grid-view" OnRowCreated="Dglegal_RowCreated"
                        OnRowDataBound="Dglegal_RowDataBound" OnSelectedIndexChanged="Dglegal_SelectedIndexChanged"
                        OnRowDeleting="Dglegal_RowDeleting">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <asp:BoundField DataField="Legalname" HeaderText="Legal_Name" Visible="false" />
                            <asp:BoundField DataField="RegisterAddress" HeaderText="Register_Address" Visible="false" />
                            <asp:BoundField DataField="Legalstatus" HeaderText="Legal_status" />
                            <asp:BoundField DataField="Dateofestablishment" HeaderText="Date_of_Esta." />
                            <asp:BoundField DataField="IncorporationNumber" HeaderText="Incorporation_No." />
                            <asp:BoundField DataField="PlaceofIncorporation" HeaderText="Place_of_Incor." />
                            <asp:BoundField DataField="TinNo" HeaderText="Tin_No" />
                            <asp:BoundField DataField="AssociationName" HeaderText="Association_Name" />
                            <asp:BoundField DataField="Registrationno" HeaderText="Registration_No." />
                            <asp:BoundField DataField="Regdate" HeaderText="REG_Date" />
                            <asp:BoundField DataField="Regexpdate" HeaderText="REG_EXP_Date" />
                            <asp:BoundField DataField="IecNumber" HeaderText="IEC_Number" />
                            <asp:BoundField DataField="Issuingauth_iec" HeaderText="Issuing_Authority" />
                            <asp:BoundField DataField="CommissionRate" HeaderText="Commission_Rate" />
                            <asp:BoundField DataField="Circle" HeaderText="Circle" />
                            <asp:TemplateField HeaderText="BankId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBankId" runat="server" Text='<%#Bind("BankId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="companyid" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblcompanyid" runat="server" Text='<%#Bind("companyid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BankName" HeaderText="Bank_Name" />
                            <asp:BoundField DataField="panGirno" HeaderText="PAN/GIR_No" />
                            <asp:BoundField DataField="DateofIss_pan" HeaderText="Date_of_Issue" />
                            <asp:BoundField DataField="IssuingAuth_pan" HeaderText="Issuing_Authority" />
                            <asp:BoundField DataField="SsiNumber" HeaderText="SSI_Number" />
                            <asp:BoundField DataField="DateofIss_ssi" HeaderText="Date_of_Issue" />
                            <asp:BoundField DataField="IssueAuth_ssi" HeaderText="Issue_Authority" />
                            <asp:BoundField DataField="IndustrialLicenceIem" HeaderText="Industrial_Licence/Iem" />
                            <asp:BoundField DataField="DateofIss_ind" HeaderText="Date_of_Issue" />
                            <asp:BoundField DataField="IssuingAuth_ind" HeaderText="Issuing_Authority" />
                            <asp:BoundField DataField="CstNo" HeaderText="CST_No." />
                            <asp:BoundField DataField="Dateofiss_cst" HeaderText="Date_of_Issue" />
                            <asp:BoundField DataField="ServiceTaxRegno" HeaderText="Service_Tax_Reg_No" />
                            <asp:BoundField DataField="StNo" HeaderText="Service_Tax_No" />
                            <asp:BoundField DataField="DateofIss_st" HeaderText="Date_of_Issue" />
                            <asp:BoundField DataField="ServiceTaxRegDate" HeaderText="Service_Tax_Reg_Date" />
                            <asp:BoundField DataField="GspRegistrationNo" HeaderText="GSP_Registration_No" />
                            <asp:BoundField DataField="DateofIss_gsp" HeaderText="Date_of_Issue" />
                            <asp:BoundField DataField="Place_gsp" HeaderText="Place" />
                            <asp:BoundField DataField="GspAccountNo" HeaderText="GSP_Account_No" />
                            <asp:BoundField DataField="DateofIss_gspac" HeaderText="Date_of_Issue" />
                            <asp:BoundField DataField="Place" HeaderText="Place" />
                            <asp:BoundField DataField="TypeofCertificate" HeaderText="Type_of_Certificate" />
                            <asp:BoundField DataField="IssueDate" HeaderText="Date_of_Issue" />
                            <asp:BoundField DataField="CertificateNo" HeaderText="Certificate_No" />
                            <asp:BoundField DataField="ValidityDate" HeaderText="Validity_Date" />
                            <asp:TemplateField HeaderText="RexNo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRexNo" runat="server" Text='<%#Bind("RexNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rex IssueDate" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRexIssueDate" runat="server" Text='<%#Bind("RexIssueDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rex ExpiryDate" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRexExpiryDate" runat="server" Text='<%#Bind("RexExpiryDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issue BodyNo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueBodyNo" runat="server" Text='<%#Bind("IssueBodyNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="DEL" OnClientClick="return doConfirm();"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="linkforseeDetail" runat="server" CommandName="" Text="Downloads Certificates"
                                        OnClick="lnkbtnName_Click" Font-Size="12px" Width="70px">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </td>
            <td>
                <asp:HiddenField ID="hnforDelete" runat="server" />
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
            <div style="overflow: scroll; width: 320px; height: 100px; margin-top: 10px; height: 180px">
                <asp:GridView ID="GDLinkedtoCustomer" runat="server" Width="200px" Style="margin-left: 10px"
                    ForeColor="#333333" AutoGenerateColumns="False" CssClass="grid-view">
                    <HeaderStyle CssClass="gvheader" Height="20px" />
                    <AlternatingRowStyle CssClass="gvalt" />
                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                    <PagerStyle CssClass="PagerStyle" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblcertificateid" runat="server" Text='<%#Bind("certificateid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FileName">
                            <ItemTemplate>
                                <asp:Label ID="lblfilename" runat="server" Text='<%#Bind("filename") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="linkfordownload" runat="server" CommandName="" Text="Download"
                                    OnClick="lnkbtnName1_Click" Font-Size="12px" Width="70px">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <table width="300px">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonnorm" Width="100px" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
