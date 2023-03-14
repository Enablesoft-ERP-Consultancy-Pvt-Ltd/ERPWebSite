<%@ Page Title="INVOICE/PACKING SLIP" Language="C#" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" AutoEventWireup="true" CodeFile="frminvoicenew.aspx.cs"
    Inherits="Masters_Packing_frminvoicenew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Newform() {
            window.location.href = "frminvoicenew.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var txtdestcode = document.getElementById('<%=txtdestcode.ClientID %>');
                    if (txtdestcode.value == "") {
                        Message = Message + "Please Enter Dest Code. !!\n";
                    }
                    //                    var txtecisno = document.getElementById('<%=txtecisno.ClientID %>');
                    //                    if (txtecisno.value == "") {
                    //                        Message = Message + "Please Enter Ecis No. !!\n";
                    //                    }
                    var txtdtstamp = document.getElementById('<%=txtdtstamp.ClientID %>');
                    if (txtdtstamp.value == "") {
                        Message = Message + "Please Enter Dt. !!\n";
                    }
                    var txtDelvwk = document.getElementById('<%=txtDelvwk.ClientID %>');
                    if (txtDelvwk.value == "") {
                        Message = Message + "Please Enter DelvWk. !!\n";
                    }
                    selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Company. !!\n";
                    }
                    var txtinvoiceno = document.getElementById('<%=txtinvoiceno.ClientID %>');
                    if (txtinvoiceno.value == "") {
                        Message = Message + "Please Enter Invoice No. !!\n";
                    }
                    var txtinvoicedate = document.getElementById('<%=txtinvoicedate.ClientID %>');
                    if (txtinvoicedate.value == "") {
                        Message = Message + "Please Enter Invoice Date. !!\n";
                    }
                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>
    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <table border="1px Solid grey">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldestcode" Text="Dest Code" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdestcode" CssClass="textb" runat="server" Width="110px" AutoPostBack="true"
                                            OnTextChanged="txtdestcode_TextChanged" />
                                        <asp:AutoCompleteExtender ID="txtdestcode_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetDestcode" EnableCaching="true"
                                            CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtdestcode"
                                            UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblecis" Text="Ecis No" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtecisno" CssClass="textb" runat="server" Width="110px" AutoPostBack="true"
                                            OnTextChanged="txtecisno_TextChanged" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldtstamp" Text="Dt. Stamp" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdtstamp" CssClass="textb" runat="server" Width="110px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbuyer" Text="Buyer" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtbuyer" CssClass="textb" runat="server" Width="300px" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDelvwk" Text="Delv Wk" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDelvwk" CssClass="textb" runat="server" Width="110px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblconsignee" Text="Consignee" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td colspan="5">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtconsigneeaddress" CssClass="textb" runat="server" Width="250px"
                                                        TextMode="MultiLine" Height="37px" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtconsignee" CssClass="textb" runat="server" Width="240px" TextMode="MultiLine"
                                                        Height="37px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblprecarriageby" Text="Pre-Carriage By" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDPreCarriage" runat="server" Width="175px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblplaceofreceipt" Text="Place of Receipt" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDReceiptAt" runat="server" Width="175px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblvessel_flightno" Text="Vessel/Flight No" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDByAirSea" runat="server" Width="175px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" Text="Port Of Loading" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDPortLoad" runat="server" Width="175px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" Text="Port Of Discharge" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtportofdischarge" CssClass="textb" Width="170px" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label3" Text="Final Destination" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfinaldestination" CssClass="textb" Width="170px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label4" Text="Pack Unit" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td colspan="3">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="DDunit" Width="100px" CssClass="dropdown" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label5" Text="Currency" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DDCurrency" runat="server" Width="130px" CssClass="dropdown"
                                                                    OnSelectedIndexChanged="DDCurrency_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label6" Text="CIF" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DDcif" runat="server" Width="130px" CssClass="dropdown">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label7" Text="Description" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtdescription" CssClass="textb" Width="450px" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="justify" valign="top" style="border: 1px Solid grey">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label33" Text="Company Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="DDcompany" CssClass="dropdown" AutoPostBack="true" Width="200px"
                                            runat="server" OnSelectedIndexChanged="DDcompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="DDSession" CssClass="dropdown" AutoPostBack="true" Width="100px"
                                            runat="server" OnSelectedIndexChanged="DDSession_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblinvoiceno" Text="Invoice No" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtinvoiceno" CssClass="textb" Width="150px" runat="server" OnTextChanged="txtinvoiceno_TextChanged"
                                            AutoPostBack="true" />
                                        <asp:AutoCompleteExtender ID="txtinvoiceno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetInvoiceno" EnableCaching="true"
                                            CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtinvoiceno"
                                            UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" Text="InvoiceDate" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtinvoicedate" CssClass="textb" Width="120px" runat="server" />
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtinvoicedate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" Text="PO No & Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtponodate" CssClass="textb" Width="350px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label10" Text="Exporter's Ref" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtexporterref" CssClass="textb" Width="150px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label11" Text="Other's Ref" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtotherref" CssClass="textb" Width="120px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" Text="Country Of Origin" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcountryoforigin" CssClass="textb" Width="150px" Text="INDIA"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" Text="Country of Final Dest." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcountryoffinaldest" CssClass="textb" Width="120px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label14" Text="Terms_of_Delivery Payment" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDelivery" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label15" Text="Seal No" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsealno" CssClass="textb" Width="120px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label16" Text="BOE Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtboedate" CssClass="textb" Width="150px" runat="server" />
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtboedate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" Text="Ship Bill No" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtshipbillno" CssClass="textb" Width="120px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label18" Text="B L No" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBlno" CssClass="textb" Width="150px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label19" Text="Ship Bill Dt." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtshipbilldate" CssClass="textb" Width="120px" runat="server" />
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtshipbilldate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label20" Text="B L Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtblDate" CssClass="textb" Width="150px" runat="server" />
                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtblDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label21" Text="Shipment ID" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtshipmentid" CssClass="textb" Width="120px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label22" Text="Truck No" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttruckno" CssClass="textb" Width="150px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label23" Text="Dispatch Dt." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdispatchdate" CssClass="textb" Width="120px" runat="server" />
                                        <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtdispatchdate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="TRManualGST" runat="server" visible="true">
                                    <td colspan="4">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblcgst" CssClass="labelbold" Text="CGST (%)" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtcgst" CssClass="textb" Width="70px" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label34" CssClass="labelbold" Text="SGST (%)" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtsgst" CssClass="textb" Width="70px" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label35" CssClass="labelbold" Text="IGST (%)" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIgst" CssClass="textb" Width="70px" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="TRGST" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label59" runat="server" Text="GST Type" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDGSType" runat="server"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDGSType_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Selected="True">---Select----</asp:ListItem>
                                            <asp:ListItem Value="1">CGST/SGST</asp:ListItem>
                                            <asp:ListItem Value="2">IGST</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="labelbold">
                                        LUTARNNO.
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chcklutarnno" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table>
                    <tr>
                        <td style="border: 1px Solig grey">
                            <div style="height: 300px; overflow: scroll">
                                <asp:GridView ID="DG" runat="server" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Roll No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrollno" Text='<%#Bind("rollno") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CARPET NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltstockno" runat="server" Text='<%#Bind("tstockno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QUALITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblqualitydesign" runat="server" Text='<%#Bind("QualityDesign") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ARTICLE NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblarticleno" runat="server" Text='<%#Bind("articleno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="COLOUR">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcolour" runat="server" Text='<%#Bind("Colorname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SIZE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AREA">
                                            <ItemTemplate>
                                                <asp:Label ID="lblarea" runat="server" Text='<%#Bind("Area") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RATE">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblrate" runat="server" Text='<%#Bind("Rate") %>'></asp:Label>--%>
                                                <asp:TextBox ID="txtrate" OnTextChanged="txtrate_Changed" Text='<%#Bind("Rate") %>'
                                                    AutoPostBack="true" runat="server" Width="80px" onkeypress="return isNumberKey(event)" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AMOUNT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblamount" runat="server" Text='<%#Bind("rate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpono" runat="server" Text='<%#Bind("Pono") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemfinishedid" Text='<%#Bind("Item_Finished_id") %>' runat="server" />
                                                <asp:Label ID="lblwidth" Text='<%#Bind("widthmtr") %>' runat="server" />
                                                <asp:Label ID="lbllength" Text='<%#Bind("lengthmtr") %>' runat="server" />
                                                <asp:Label ID="lblstockno" Text='<%#Bind("stockno") %>' runat="server" />
                                                <asp:Label ID="lbldesign" Text='<%#Bind("designName") %>' runat="server" />
                                                <asp:Label ID="lblquality" runat="server" Text='<%#Bind("QualityName") %>'></asp:Label>
                                                <asp:Label ID="lblpodate" runat="server" Text='<%#Bind("podate") %>'></asp:Label>
                                                <asp:Label ID="lblratedate" runat="server" Text='<%#Bind("ratedate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblcontents" Text="Contents" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtcontents" CssClass="textb" Width="300px" runat="server" TextMode="MultiLine"
                                Height="51px" />
                        </td>
                        <td valign="top" align="justify" style="border: 1px Solid Grey">
                            <table border="0" cellpadding="0" cellspacing="1px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblgrwt" Text="Gross Weight" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtgrwt" CssClass="textb" Width="100px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label24" Text="Pcs" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpcs" CssClass="textb" Width="100px" runat="server" Enabled="false" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label25" Text="Total Area" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttotalarea" CssClass="textb" Width="100px" runat="server" Enabled="false" />
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:Label ID="Label26" Text="Gross amount" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:TextBox ID="txtgramount" CssClass="textb" Width="100px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label27" Text="Net Weight" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtnetwt" CssClass="textb" Width="100px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label28" Text="Rolls" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtrolls" CssClass="textb" Width="100px" runat="server" Enabled="false" />
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:Label ID="Label29" Text="Insurance" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:TextBox ID="txtinsurance" CssClass="textb" Width="100px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label30" Text="Volume" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtvol" CssClass="textb" Width="100px" runat="server" />
                                    </td>
                                    <td colspan="2">
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:Label ID="Label31" Text="Freight" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:TextBox ID="txtfreight" CssClass="textb" Width="100px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label32" Text="Inv. Amount" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtinvamt" CssClass="textb" Width="100px" runat="server" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return Newform();" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                OnClientClick="return confirm('Do you want to Save Invoice?');" />
                            <asp:Button ID="btnUpdateGSTType" runat="server" Text="Update GST Type" CssClass="buttonnorm"
                                Visible="false" OnClick="btnUpdateGSTType_Click" OnClientClick="return confirm('Do you want to Update GST Type?');" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to delete this Invoice?');"
                                OnClick="btndelete_Click" Visible="false" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hninvid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
