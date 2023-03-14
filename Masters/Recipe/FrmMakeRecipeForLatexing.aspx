<%@ Page Title="Make Recipe For Latexing" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmMakeRecipeForLatexing.aspx.cs" Inherits="Masters_Recipe_FrmMakeRecipeForLatexing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/Fixfocus.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmMakeRecipeForLatexing.aspx";
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
        function CheckBoxClick(objref) {

            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
            }
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;
                        row.style.backgroundColor = "Orange";
                    }
                    else {
                        inputlist[i].checked = false;
                        row.style.backgroundColor = "White";

                    }
                }
            }
        }
    </script>
    <div>
        <asp:UpdatePanel ID="upd2" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDCompanyName" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Slip No." CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDSlipNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDSlipNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Recipe Name" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDRecipeName" runat="server" CssClass="dropdown" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDRecipeName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDIssueNo" runat="server" visible="false">
                            <asp:Label ID="Label7" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDissueno" runat="server" CssClass="dropdown" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtissueno" CssClass="textb" Width="61px" runat="server" Enabled="false" />
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtissuedate" CssClass="textb" Width="80px" runat="server" />
                            <asp:CalendarExtender ID="cal1" TargetControlID="txtissuedate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr id="Tr3" runat="server">
                        <td colspan="2">
                            <asp:Label ID="LblDescription" runat="server" Text="Description" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDDescription" runat="server" Width="300px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="DDDescription_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="LblUnit" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDUnit" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="LblConsmpQty" runat="server" Text="Consmp Qty" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtConsmpQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                Enabled="false" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="LblRecQty" runat="server" Text="Rec Qty" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtRecQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                Enabled="false" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="LblRecBalQty" runat="server" Text="Rec Bal Qty" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtRecBalQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                Enabled="false" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="LblIssueQty" runat="server" Text="Iss Qty" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtIssueQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                Enabled="false" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="LblBalQty" runat="server" Text="Bal Qty" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtBalQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                Enabled="false" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="LblQty" runat="server" Text="Qty" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" Visible="false" CssClass="buttonnorm"
                                OnClick="btnPreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
                <div style="max-height: 300px; overflow: auto;">
                    <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No records found.."
                        OnRowDeleting="gvdetail_RowDeleting">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <Columns>
                            <asp:TemplateField HeaderText="Item Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblissueqty" Text='<%#Bind("Qty") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hnissueid" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
