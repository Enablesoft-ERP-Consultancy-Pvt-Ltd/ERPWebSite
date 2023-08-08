<%@ Page Title="Master Purchase Receive Detail" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmMasterPurchaseReceiveDetailDiamond.aspx.cs" Inherits="Masters_Purchase_FrmMasterPurchaseReceiveDetailDiamond" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/Fixfocus.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmMasterPurchaseReceiveDetailDiamond.aspx";
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
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkEdit_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDEditIssueNo" runat="server" visible="false">
                                <asp:Label ID="Label10" runat="server" Text="Rec No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtEditIssueNo" CssClass="textb" Width="61px" runat="server" Enabled="true"
                                    AutoPostBack="true" OnTextChanged="txtEditIssueNo_TextChanged" />
                            </td>                           
                           
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Quality Name" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtQualityName" CssClass="textb" Width="150px" runat="server" Enabled="true" />
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Color Name" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtColorName" CssClass="textb" Width="120px" runat="server" Enabled="true" />
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="LotNo" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtLotNo" CssClass="textb" Width="120px" runat="server" Enabled="true" />
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="TagNo" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtTagNo" CssClass="textb" Width="120px" runat="server" Enabled="true" />
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Rec Qty" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtRecQty" CssClass="textb" Width="100px" runat="server" Enabled="true" />
                            </td>
                           
                             <td>
                                <asp:Label ID="Label3" runat="server" Text="Rec Date" CssClass="labelbold" Visible="false"></asp:Label><br />
                                <asp:TextBox ID="txtRecDate" CssClass="textb" Width="80px" runat="server" Visible="false" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtRecDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Rec No" CssClass="labelbold" Visible="false"></asp:Label><br />
                                <asp:TextBox ID="txtRecNo" CssClass="textb" Width="61px" runat="server" Enabled="false" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                        
                            <td id="TDIssueNo" runat="server" visible="false">
                                <asp:Label ID="Label7" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDissueno" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <%--<td colspan="5" runat="server">
                                <asp:Label ID="Label12" runat="server" Text="Remarks" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtRemarks" CssClass="textb" Width="300px" runat="server" />
                            </td>--%>
                        </tr>
                    </table>
                </fieldset>               
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click"
                                    Visible="false" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="max-height: 300px; overflow: auto;">
                    <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No records found.."
                        OnRowDataBound="RowDataBound" OnRowCancelingEdit="gvdetail_RowCancelingEdit"
                        OnRowEditing="gvdetail_RowEditing" OnRowUpdating="gvdetail_RowUpdating" OnRowDeleting="gvdetail_RowDeleting">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <Columns>                            
                            <asp:TemplateField HeaderText="QualityName">
                                <ItemTemplate>
                                    <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName")%>' runat="server" />
                                </ItemTemplate>
                                 <ItemStyle HorizontalAlign="Center" Width="200px" />
                            </asp:TemplateField>                           
                            <asp:TemplateField HeaderText="Color">
                                <ItemTemplate>
                                    <asp:Label ID="lblColorName" Text='<%#Bind("ColorName")%>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LotNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblLotNo" Text='<%#Bind("LotNo")%>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TagNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblTagNo" Text='<%#Bind("TagNo")%>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:TemplateField>                           
                            <asp:TemplateField HeaderText="RecQty">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRecQty" Text='<%#Bind("RecQty") %>' Width="70px" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPerSetQty" Text='<%#Bind("RecQty") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>                                   
                                    <asp:Label ID="lblPRID" Text='<%#Bind("PRID") %>' runat="server" />                                                                   
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField EditText="Edit" ShowEditButton="false" CausesValidation="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                        </Columns>
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hnRecId" runat="server" Value="0" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
