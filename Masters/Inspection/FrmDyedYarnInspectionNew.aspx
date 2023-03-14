<%@ Page Title="Dyed Yarn Inspection" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmDyedYarnInspectionNew.aspx.cs" Inherits="Masters_Inspection_FrmDyedYarnInspectionNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "FrmDyedYarnInspectionNew.aspx";
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
        function EmpSelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
        }
       
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkedit" Text="FOR EDIT" CssClass="checkboxbold" AutoPostBack="True"
                            runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                    </td>
                    <td>
                    </td>
                    <td id="TDSupplierSearch" runat="server" visible="false">
                        <asp:TextBox ID="txtsuppliersearch" Placeholder="Type Supplier here to search Doc No."
                            runat="server" Width="235px" CssClass="textb" />
                    </td>
                    <td id="TDshadeno" runat="server" visible="false">
                        <asp:TextBox ID="txtshadesearch" Placeholder="Type Shade No. here to search Doc No."
                            runat="server" Width="235px" CssClass="textb" />
                    </td>
                    <td id="TDLotno" runat="server" visible="false">
                        <asp:TextBox ID="txtlotnosearch" Placeholder="Type Lot No. here to search Doc No."
                            runat="server" Width="150px" CssClass="textb" />
                    </td>
                    <td id="TDTagNo" runat="server" visible="false">
                        <asp:TextBox ID="txtTagNosearch" Placeholder="Type Tag No. here to search Doc No."
                            runat="server" Width="150px" CssClass="textb" />
                    </td>
                    <td>
                        <asp:Button ID="btnsearch" runat="server" Text="Button" OnClick="btnsearch_Click"
                            Style="display: none;" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblcompany" Text="Company Name" CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DDcompanyName" CssClass="dropdown" Width="200px" runat="server"
                            OnSelectedIndexChanged="DDcompanyName_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label10" Text="Branch Name" CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="200px" runat="server"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td id="TDDocno" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label38" Text="Doc No." CssClass="labelbold" runat="server" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDDocNo" CssClass="dropdown" Width="200px" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDDocNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Label ID="Label33" Text="System Gen. Doc  No." CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtdocno" CssClass="textb" runat="server" Enabled="false" BackColor="LightGray" />
                    </td>
                </tr>
            </table>
            <table border="1" cellspacing="2" style="width: 100%">
                <tr>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label ID="Label1" Text="Supplier Name" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="width: 50%; border-style: dotted">
                    <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="txtsuppliername" CssClass="textb" Width="95%" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter SupplierName"
                            ControlToValidate="txtsuppliername" Display="Dynamic" ValidationGroup="s" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <asp:AutoCompleteExtender ID="txtsuppliername_AutoCompleteExtender" runat="server"
                            BehaviorID="SrchAutoComplete1" CompletionInterval="20" Enabled="True" ServiceMethod="GetDyeingVendor"
                            CompletionSetCount="20" OnClientItemSelected="EmpSelected" ServicePath="~/Autocomplete.asmx"
                            TargetControlID="txtsuppliername" UseContextKey="True" ContextKey="0" MinimumPrefixLength="2"
                            DelimiterCharacters="">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td style="width: 5%; border-style: dotted">
                        <asp:Label ID="Label2" Text="Date :" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:TextBox ID="txtdate" CssClass="textb" Width="95%" runat="server" />
                        <asp:CalendarExtender ID="caldate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtdate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="border-style: dotted">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 8%">
                                    <asp:Label ID="Label5" Text="Lot No." runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtlotno" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 8%">
                                    <asp:Label ID="Label6" Text="Tag No." runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="TxtTagNo" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 8%">
                                    <asp:Label ID="Label3" Text="Challan No. &amp; Date" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtchallannodate" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                 <td style="width: 8%">
                                    <asp:Label ID="Label13" Text="Inwards No" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtInwardsNo" CssClass="textb" Width="95%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <tr>
                        <td colspan="4" style="width: 100%; border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 8%">
                                        <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Total Qty" />
                                    </td>
                                    <td style="width: 12%">
                                        <asp:TextBox ID="txttotalbale" runat="server" CssClass="textb" onkeypress="return isNumberKey(event);"
                                            Width="95%" />
                                    </td>
                                     <td style="width: 8%">
                                        <asp:Label ID="Label11" runat="server" CssClass="labelbold" Text="Total Not Ok Qty" />
                                    </td>
                                    <td style="width: 12%">
                                        <asp:TextBox ID="txtTotalNotOkQty" runat="server" CssClass="textb" onkeypress="return isNumberKey(event);"
                                            Width="95%" />
                                    </td>

                                    <td style="width: 8%">
                                        <asp:Label ID="Label8" runat="server" CssClass="labelbold" Text="Sample Plan" />
                                    </td>
                                    <td style="width: 12%">
                                        <asp:TextBox ID="txtsamplesize" runat="server" CssClass="textb" onkeypress="return isNumberKey(event);"
                                            Width="95%" />
                                    </td>

                                     <td style="width: 8%">
                                        <asp:Label ID="Label12" runat="server" CssClass="labelbold" Text="Yarn Type" />
                                    </td>
                                    <td style="width: 12%">
                                        <asp:TextBox ID="txtYarnType" runat="server" CssClass="textb" onkeypress="return isNumberKey(event);"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 8%">
                                        <asp:Label ID="Label9" runat="server" CssClass="labelbold" Text="No. of Hank" />
                                    </td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="txtnoofhank" runat="server" CssClass="textb" onkeypress="return isNumberKey(event);"
                                            Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <div style="width: 100%; overflow: auto; max-height: 200px; max-width:1100px">
                            <asp:GridView ID="DGDetail" CssClass="grid-views" runat="server" EmptyDataText="No. Records found."
                                AutoGenerateColumns="False" OnRowDataBound="DGDetail_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SI No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsrno" Text='<%#Bind("Srno") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shade No.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtshadeno" Text='<%# Eval("Shadeno") %>' runat="server" CssClass="textb"
                                                Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Moisture Content">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtmoisturecontent" Text='<%#Bind("moisturecontent") %>' CssClass="textb"
                                                runat="server" Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Color Fastness To Washing">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtlabtestcolor" Text='<%#Bind("ColorFastnessToWashing") %>' CssClass="textb"
                                                runat="server" Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dry">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtdry" Text='<%#Bind("dry") %>' CssClass="textb" runat="server"
                                                Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wet">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtwet" Text='<%#Bind("Wet") %>' CssClass="textb" runat="server"
                                                Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recd.Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtrecdqty" Text='<%#Bind("recdqty") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shade Variation">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtShadeVariation" Text='<%#Bind("ShadeVariation") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Presence Of Ref Sample">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPresenceOfRefSample" Text='<%#Bind("PresenceOfRefSample") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Result Of PH">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtResultOfPH" Text='<%#Bind("ResultOfPH") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transport Condition And Dam">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTransportConditionandDAM" Text='<%#Bind("TransportConditionandDAM") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Shade(General Apperance)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtShadeGeneralApperance" Text='<%#Bind("ShadeGeneralApperance") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Result">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddresult" runat="server" CssClass="dropdown">
                                                <asp:ListItem Text="Pass" />
                                                <asp:ListItem Text="Fail" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblresult" Text='<%#Bind("RESULT") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 10%">
                        <asp:Label ID="Label4" Text="Comments" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 80%">
                        <asp:TextBox ID="txtcomments" runat="server" CssClass="textb" Width="95%" TextMode="MultiLine"
                            Height="30px" />
                    </td>
                    <td style="width: 10%" valign="top">
                        <table style="width: 100%">
                        </table>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 25%">
                        <asp:Label ID="Label14" Text="Whether accepted goods are put in accepted area" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 20%">
                        <asp:TextBox ID="txtAcceptedArea" runat="server" CssClass="textb" Width="95%" />
                    </td>
                   <td style="width: 25%">
                        <asp:Label ID="Label15" Text="Whether rejected goods are put in rejected area" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 20%">
                        <asp:TextBox ID="txtRejectedArea" runat="server" CssClass="textb" Width="95%" />
                    </td>
                </tr>
            </table>
            <fieldset>
                <legend>
                    <asp:Label Text="Saved Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                </legend>
                <table width="100%">
                    <tr>
                        <td>
                            <div style="width: 100%; overflow: auto; max-height: 300px">
                                <asp:GridView ID="DGSavedetails" CssClass="grid-views" runat="server" EmptyDataText="No. Records found."
                                    AutoGenerateColumns="False" OnRowCancelingEdit="DGSavedetails_RowCancelingEdit"
                                    OnRowEditing="DGSavedetails_RowEditing" OnRowUpdating="DGSavedetails_RowUpdating"
                                    OnRowDataBound="DGSavedetails_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:CommandField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkdel" runat="server" Text="Del" OnClientClick="return confirm('Do you Want to delete this row?')"
                                                    OnClick="lnkdelClick"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SI No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" Text='<%#Bind("Srno") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shade No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblshadeno" Text='<%# Eval("Shadeno") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtshadeno" Text='<%# Eval("Shadeno") %>' runat="server" CssClass="textb"
                                                    Style="text-align: center" />
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Moisture Content">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmoisturecontent" Text='<%#Bind("moisturecontent") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtmoisturecontent" Text='<%#Bind("moisturecontent") %>' CssClass="textb"
                                                    runat="server" Style="text-align: center" />
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color Fastness To Washing">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllabtestcolor" Text='<%#Bind("labtestcolor") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtlabtestcolor" Text='<%#Bind("labtestcolor") %>' CssClass="textb"
                                                    runat="server" Style="text-align: center" />
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dry">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldry" Text='<%#Bind("dry") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtdry" Text='<%#Bind("dry") %>' CssClass="textb" runat="server"
                                                    Style="text-align: center" />
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wet">
                                            <ItemTemplate>
                                                <asp:Label ID="lblwet" Text='<%#Bind("Wet") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtwet" Text='<%#Bind("Wet") %>' CssClass="textb" runat="server"
                                                    Style="text-align: center" />
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recd.Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrecdqty" Text='<%#Bind("recdqty") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtrecdqty" Text='<%#Bind("recdqty") %>' CssClass="textb" runat="server"
                                                    Width="100px" Style="text-align: center" />
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Shade Variation">                                      

                                         <ItemTemplate>
                                                <asp:Label ID="lblShadeVariation" Text='<%#Bind("ShadeVariation") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtShadeVariation" Text='<%#Bind("ShadeVariation") %>' CssClass="textb" runat="server"
                                                    Width="100px" Style="text-align: center" />
                                            </EditItemTemplate>                                           
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Presence Of Ref Sample">
                                       <%-- <ItemTemplate>
                                            <asp:TextBox ID="txtPresenceOfRefSample" Text='<%#Bind("PresenceOfRefSample") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>--%>
                                         <ItemTemplate>
                                                <asp:Label ID="lblPresenceOfRefSample" Text='<%#Bind("PresenceOfRefSample") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPresenceOfRefSample" Text='<%#Bind("PresenceOfRefSample") %>' CssClass="textb" runat="server"
                                                    Width="100px" Style="text-align: center" />
                                            </EditItemTemplate> 
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Result Of PH">
                                       <%-- <ItemTemplate>
                                            <asp:TextBox ID="txtResultOfPH" Text='<%#Bind("ResultOfPH") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>--%>
                                         <ItemTemplate>
                                                <asp:Label ID="lblResultOfPH" Text='<%#Bind("ResultOfPH") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtResultOfPH" Text='<%#Bind("ResultOfPH") %>' CssClass="textb" runat="server"
                                                    Width="100px" Style="text-align: center" />
                                            </EditItemTemplate> 
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transport Condition And Dam">
                                        <%--<ItemTemplate>
                                            <asp:TextBox ID="txtTransportConditionandDAM" Text='<%#Bind("TransportConditionandDAM") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>--%>
                                        <ItemTemplate>
                                                <asp:Label ID="lblTransportConditionandDAM" Text='<%#Bind("TransportCondition") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTransportConditionandDAM" Text='<%#Bind("TransportCondition") %>' CssClass="textb" runat="server"
                                                    Width="100px" Style="text-align: center" />
                                            </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Shade(General Apperance)">
                                        <%--<ItemTemplate>
                                            <asp:TextBox ID="txtShadeGeneralAppereance" Text='<%#Bind("ShadeGeneralAppereance") %>' CssClass="textb" runat="server"
                                                Width="100px" Style="text-align: center" />
                                        </ItemTemplate>--%>
                                         <ItemTemplate>
                                                <asp:Label ID="lblSHADEGENERALAPPERANCE" Text='<%#Bind("SHADEGENERALAPPERANCE") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtSHADEGENERALAPPERANCE" Text='<%#Bind("SHADEGENERALAPPERANCE") %>' CssClass="textb" runat="server"
                                                    Width="100px" Style="text-align: center" />
                                            </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Result">
                                            <ItemTemplate>
                                                <asp:Label ID="lblresultgrid" Text='<%#Bind("Result") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddresult" runat="server" CssClass="dropdown">
                                                    <asp:ListItem Text="Pass" />
                                                    <asp:ListItem Text="Fail" />
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblresult" Text='<%#Bind("RESULT") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldocid" Text='<%#Bind("docid") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div>
           <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label28" Text="Yarn Photo" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:Image ID="lblphotoimage" runat="server" Height="100px" Width="170px" />
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileuploadphoto" ViewStateMode="Enabled" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                        ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="fileuploadphoto"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
            
            </div>

            <table style="width: 100%">
                <tr>
                 <td>
                        
                    </td>
                    <td style="text-align: center">
                        <asp:Button Text="Save" ID="btnsave" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click" />
                        <asp:Button Text="Preview" ID="btnpreview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                        <asp:Button ID="btndelete" Text="Delete" CssClass="buttonnorm" runat="server" OnClientClick="return confirm('Do you want to delete this Doc No.?')"
                            OnClick="btndelete_Click" />
                        <asp:Button Text="Close" ID="btnclose" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                        <asp:Button Text="New" ID="btnnew" CssClass="buttonnorm" runat="server" OnClientClick="return ClickNew();" />
                        <asp:Button Text="Approve" ID="btnApprove" CssClass="buttonnorm" runat="server" Visible="false"
                            OnClick="btnApprove_Click" OnClientClick="return confirm('Do you want to approve Doc No. ?')" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" CssClass="labelbold" runat="server" ForeColor="Red" Font-Size="Small" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hndocid" Value="0" runat="server" />
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
