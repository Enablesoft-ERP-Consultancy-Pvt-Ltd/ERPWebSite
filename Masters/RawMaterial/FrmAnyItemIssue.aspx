<%@ Page Title="Any Item Issue" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmAnyItemIssue.aspx.cs" Inherits="Masters_RawMaterial_FrmAnyItemIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmAnyItemIssue.aspx";
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
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    var Dept = document.getElementById('<%=DDDepartmentName.ClientID %>');
                    if (Dept != null) {
                        selectedindex = $("#<%=DDDepartmentName.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Department Name!!\n";
                        }
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
    <asp:UpdatePanel ID="R" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chkedit" Text="For Edit" CssClass="checkboxbold" AutoPostBack="true"
                                    runat="server" OnCheckedChanged="Chkedit_CheckedChanged" />
                            </td>
                            <td id="TRempcodescan" runat="server">
                                <asp:Label ID="Label18" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                    Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Department Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDepartmentName" CssClass="dropdown" runat="server" Width="200px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDDepartmentName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblEmployeeName" runat="server" Text="Employee Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDEmployeeName" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIssuedNo" runat="server" visible="false">
                                <asp:Label ID="Label14" runat="server" Text="Issued No." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDIssuedNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDIssuedNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtIssueNo" CssClass="textb" Width="100px" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtIssueDate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="TxtIssueDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" Text="Enter/Scan Item Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtStockIDScan" CssClass="textb" runat="server" Width="150px" Height="20px"
                                    AutoPostBack="true" OnTextChanged="TxtStockIDScan_TextChanged" />
                            </td>
                            <td>
                                <asp:Label ID="Label4" Text="Remark" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtRemark" CssClass="textb" runat="server" Width="500px" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <div id="gride" runat="server" style="height: 300px; overflow: auto">
                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        OnRowDeleting="DG_RowDeleting">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:TemplateField HeaderText="ItemDescription">
                                <ItemTemplate>
                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Godown">
                                <ItemTemplate>
                                    <asp:Label ID="lblGodown" Text='<%#Bind("GodownName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lot No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblLotno" Text='<%#Bind("Lotno") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tag No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblTagno" Text='<%#Bind("Tagno") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" Text='<%#Bind("Qty") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" Text='<%#Bind("ID") %>' runat="server" />
                                    <asp:Label ID="lbldetailid" Text='<%#Bind("DetailID") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="Del" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
