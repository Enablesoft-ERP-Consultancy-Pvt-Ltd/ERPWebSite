<%@ Page Title="Dyed Yarn Inspection" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmdyedyarninspection.aspx.cs" Inherits="Masters_Inspection_frmdyedyarninspection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type ="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmdyedyarninspection.aspx";
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
                                 <td id="TDappstatus" runat="server" visible="false">
                                  <asp:CheckBox ID="chkapprove" Text="With Approved" CssClass="checkboxbold" AutoPostBack="True"
                            runat="server"  OnCheckedChanged="chkapprove_CheckedChanged"  />
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
                        <asp:TextBox ID="txtsuppliername" CssClass="textb" Width="95%" runat="server" />
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
                                <td style="width: 10%">
                                    <asp:Label ID="Label5" Text="Lot No." runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtlotno" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 10%">
                                    <asp:Label ID="Label6" Text="Tag No." runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="TxtTagNo" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 10%">
                                    <asp:Label ID="Label3" Text="Challan No. &amp; Date" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtchallannodate" CssClass="textb" Width="95%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <tr>
                        <td colspan="4" style="width: 100%; border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 8%">
                                        <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Total Bale" />
                                    </td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="txttotalbale" runat="server" CssClass="textb" onkeypress="return isNumberKey(event);"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 8%">
                                        <asp:Label ID="Label8" runat="server" CssClass="labelbold" Text="Sample Size" />
                                    </td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="txtsamplesize" runat="server" CssClass="textb" onkeypress="return isNumberKey(event);"
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
                        <div style="width: 100%; overflow: auto; max-height: 200px">
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
                                    <asp:TemplateField HeaderText="Lab Test color">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtlabtestcolor" Text='<%#Bind("labtestcolor") %>' CssClass="textb"
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
                                        <asp:TemplateField HeaderText="Lab Test color">
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
            <table style="width: 100%">
                <tr>
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
    </asp:UpdatePanel>
</asp:Content>
