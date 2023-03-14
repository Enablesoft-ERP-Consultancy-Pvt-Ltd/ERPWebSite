<%@ Page Title="Advance/Loan" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmadvance_Loanmaster.aspx.cs" Inherits="Masters_Payroll_frmadvance_Loanmaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function KeyDownHandlerWeaverIdscan(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnsearchemp.ClientID %>').click();
            }
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmadvance_loanmaster.aspx";
        }
    </script>
    <script type="text/javascript">
        function validatesave() {
            var Message = "";
            var hnempid = document.getElementById('<%=hnempid.ClientID %>');
            if (hnempid.value == "0") {
                Message = Message + "Please Enter Valid Emp. Code!!\n";
            }

            var txtamount = document.getElementById('<%=txtamount.ClientID %>');
            if (txtamount.value == "") {
                Message = Message + "Please Enter Advance/Loan Amount!!\n";
            }
            var txtnoofinstallment = document.getElementById('<%=txtnoofinstallment.ClientID %>');
            if (txtnoofinstallment.value == "") {
                Message = Message + "Please Enter No of Installment!!\n";
            }
            var txtinstallmentamount = document.getElementById('<%=txtinstallmentamount.ClientID %>');
            if (txtinstallmentamount.value == "") {
                Message = Message + "Installment amount can not blank!!\n";
            }

            var txtdate = document.getElementById('<%=txtdate.ClientID %>');
            if (txtdate.value == "") {
                Message = Message + "Please Enter Advance/Loan Date!!\n";
            }

            if (Message == "") {
                return true;
            }
            else {
                alert(Message);
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table style="width: 100%" border="1" cellspacing="2">
                <tr>
                    <td style="width: 20%; border-style: dotted" valign="middle">
                        <asp:Label Text="Select" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </td>
                    <td style="width: 80%; border-style: dotted" valign="top">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 25%">
                                    <asp:Label Text="Advance/Loan No." CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtadvance_LoanNo" CssClass="textboxm" Width="95%" runat="server"
                                        placeholder="Auto generated" Enabled="false" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Label ID="Label1" Text="Enter Emp. Code" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtempcode" CssClass="textboxm" Width="95%" runat="server" onKeypress="KeyDownHandlerWeaverIdscan(event);" />
                                    <asp:Button ID="btnsearchemp" Text="Emp.Search" Style="display: none" runat="server"
                                        OnClick="btnsearchemp_Click" />
                                </td>
                                <td style="width: 55%">
                                    <asp:Label ID="Label2" Text="Emp. Name" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtempname" CssClass="textboxm" Enabled="false" Width="95%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; border-style: dotted" valign="middle">
                        <asp:Label ID="Label3" Text="Advance/Loan" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </td>
                    <td style="width: 80%; border-style: dotted" valign="top">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label ID="Label17" Text="Cr/Dr" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:DropDownList ID="DDcrdr" CssClass="dropdown" Width="95%" runat="server">
                                        <asp:ListItem Text="Cr" Value="1" />
                                        <asp:ListItem Text="Dr" Value="2" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 25%">
                                    <asp:Label ID="Label4" Text="Type" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:DropDownList ID="DDtypeofamount" CssClass="dropdown" Width="95%" runat="server">
                                        <asp:ListItem Text="Advance" Value="1" />
                                        <asp:ListItem Text="Loan" Value="2" />
                                        <asp:ListItem Text="Kharcha" Value="3" />
                                        <asp:ListItem Text="TDS" Value="4" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20%">
                                    <asp:Label ID="Label5" Text="Advance/Loan Amount" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtamount" CssClass="textboxm" Width="95%" runat="server" OnTextChanged="txtamount_TextChanged"
                                        AutoPostBack="true" />
                                </td>
                                <td style="width: 55%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:Label ID="lbldate" Text="Advance/Loan Date" CssClass="labelbold" runat="server" />
                                                <asp:TextBox ID="txtdate" CssClass="textboxm" Width="95%" runat="server" Enabled="false" />
                                                <asp:CalendarExtender ID="calfromdate" TargetControlID="txtdate" Format="dd-MMM-yyyy"
                                                    runat="server" PopupButtonID="imgfromdate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td style="width: 50%">
                                                <br />
                                                <asp:ImageButton ID="imgfromdate" CausesValidation="false" AlternateText="Click here to display calender"
                                                    ImageUrl="~/Images/calendar.png" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; border-style: dotted" valign="middle">
                        <asp:Label ID="Label6" Text="Advance/Loan Return" CssClass="labelbold" ForeColor="Red"
                            runat="server" />
                    </td>
                    <td style="width: 80%; border-style: dotted" valign="top">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label ID="Label7" Text="No. of Installment" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtnoofinstallment" CssClass="textboxm" Width="95%" Text="1" runat="server"
                                        OnTextChanged="txtnoofinstallment_TextChanged" AutoPostBack="true" />
                                </td>
                                <td style="width: 15%">
                                    <asp:Label ID="Label8" Text="Installment Amount" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtinstallmentamount" CssClass="textboxm" Width="95%" runat="server"
                                        Enabled="false" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Label ID="Label9" Text="Deduction Period" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:DropDownList ID="DDDeductionperiod" Width="95%" CssClass="dropdown" runat="server">
                                        <asp:ListItem Text="Monthly" Value="1" />
                                        <asp:ListItem Text="Quarterly" Value="2" />
                                        <asp:ListItem Text="Half-yearly" Value="3" />
                                        <asp:ListItem Text="Yearly" Value="4" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 50%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:Label Text="Deduction From Year" CssClass="labelbold" runat="server" />
                                                <br />
                                                <asp:DropDownList ID="DDdeductionyear" CssClass="dropdown" Width="95%" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:Label ID="Label10" Text="Deduction Month" CssClass="labelbold" runat="server" />
                                                <br />
                                                <asp:DropDownList ID="DDMonth" CssClass="dropdown" Width="95%" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; border-style: dotted" valign="middle">
                        <asp:Label ID="Label11" Text="Advance/Loan Mode" CssClass="labelbold" ForeColor="Red"
                            runat="server" />
                    </td>
                    <td style="width: 80%; border-style: dotted" valign="top">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label ID="Label12" Text="Payment Mode" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:DropDownList ID="DDpaymentmode" CssClass="dropdown" Width="95%" runat="server">
                                        <asp:ListItem Text="Cash" Value="1" />
                                        <asp:ListItem Text="Cheque" Value="2" />
                                        <asp:ListItem Text="Other" Value="3" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%">
                                    <asp:Label ID="Label13" Text="Cheque No." CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtchequeno" MaxLength="10" CssClass="textboxm" Width="95%" runat="server" />
                                </td>
                                <td style="width: 30%">
                                    <asp:Label ID="Label15" Text="Bank Name" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtbankname" CssClass="textboxm" MaxLength="50" Width="95%" runat="server" />
                                </td>
                                <td style="width: 40%">
                                    <asp:Label ID="Label14" Text="Remark (if any)" CssClass="labelbold" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtremark" CssClass="textboxm" MaxLength="50" Width="95%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; border-style: dotted" valign="middle">
                        <asp:Label ID="Label16" Text="Details (if any)" CssClass="labelbold" ForeColor="Red"
                            runat="server" />
                    </td>
                    <td style="width: 80%; border-style: dotted" valign="top">
                        <asp:TextBox ID="txtdetails" CssClass="textboxm" Width="95%" TextMode="MultiLine"
                            runat="server" Height="40px" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; border-style: dotted" valign="middle" colspan="2" align="right">
                        <asp:Button Text="Save" ID="btnsave" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click"
                            OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';"
                            UseSubmitBehavior="false" />
                        <asp:Button Text="Close" ID="btnclose" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm()" />
                        <asp:Button Text="New" ID="btnnew" CssClass="buttonnorm" runat="server" OnClientClick="return NewForm()" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; border-style: dotted" valign="middle" colspan="2">
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                    </td>
                </tr>
            </table>
            <fieldset runat="server" id="FDDelete">
                <legend>
                    <asp:Label Text="Delete Data" CssClass="labelbold" ForeColor="Red" runat="server" />
                </legend>
                <table cellspacing="2" width="100%">
                    <tr>
                        <td style="width: 20%">
                            <asp:Label ID="Label18" Text="Enter Emp. Code" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:TextBox ID="txtempcodeedit" CssClass="textboxm" Width="95%" runat="server" />
                        </td>
                        <td style="width: 10%">
                            <asp:Label ID="Label19" Text="Cr/Dr" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDcrdredit" CssClass="dropdown" Width="95%" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1" />
                                <asp:ListItem Text="Cr" Value="1" />
                                <asp:ListItem Text="Dr" Value="2" />
                            </asp:DropDownList>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="Label20" Text="Type" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDtypeedit" CssClass="dropdown" Width="95%" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1" />
                                <asp:ListItem Text="Advance" Value="1" />
                                <asp:ListItem Text="Loan" Value="2" />
                                <asp:ListItem Text="Kharcha" Value="3" />
                                <asp:ListItem Text="TDS" Value="4" />
                            </asp:DropDownList>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="Label22" Text="Deduction From Year" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDyearedit" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="Label21" Text="Deduction Month" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDDeductionmonthedit" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 20%">
                            <br />
                            <asp:Button Text="Get Data" ID="btngetdata" CssClass="buttonnorm" runat="server"
                                OnClick="btngetdata_Click" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td style="width: 100%">
                            <div style="width: 100%; max-height: 300px; overflow: auto">
                                <asp:GridView ID="DGDetail" CssClass="grid-views" runat="server" Width="100%" AutoGenerateColumns="false"
                                    EmptyDataText="No data fetched...." onrowdatabound="DGDetail_RowDataBound" 
                                    onrowdeleting="DGDetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldate" Text='<%#Bind("Adv_Loandate") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Emp Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblempcode" Text='<%#Bind("empcode") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Emp Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblempname" Text='<%#Bind("empname") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartment" Text='<%#Bind("Departmentname") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cr_Dr">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcrdr" Text='<%#Bind("Cr_Dr") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltype" Text='<%#Bind("Type") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblamount" Text='<%#Bind("Amount") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No of Installment">
                                            <ItemTemplate>
                                                <asp:Label ID="lblnoofinstallment" Text='<%#Bind("NoofInstallment") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Deduction Period">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldeductionperiod" Text='<%#Bind("deductionperiod") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Deduction Year">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldeductionyear" Text='<%#Bind("Deductionyear") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Deduction Month">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldeductionmonth" Text='<%#Bind("Month_name") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Mode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpaymentmode" Text='<%#Bind("paymentmode") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblchequeno" Text='<%#Bind("Chequeno") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bank Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbankname" Text='<%#Bind("bankname") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:Label ID="lblremark" Text='<%#Bind("Remark") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" Text='<%#Bind("id") %>' runat="server" />
                                                <asp:Label ID="lblempid" Text='<%#Bind("empid") %>' runat="server" />
                                                <asp:Label ID="lbldeductionmonthid" Text='<%#Bind("deductionmonthid") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:HiddenField ID="hnempid" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
