<%@ Page Title="Material Issue" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmweftIssue.aspx.cs" Inherits="Masters_Loom_frmweftIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/Fixfocus.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmweftIssue.aspx";
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
                            <td id="TRempcodescan" runat="server" visible="false">
                                <asp:Label ID="Label18" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                    Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                            </td>
                            <td id="TDForCompleteStatus" runat="server" visible="false">
                                <span style="text-align: right">
                                    <asp:CheckBox ID="ChkForCompleteStatus" Text="For Complete Order" CssClass="labelbold"
                                        runat="server" AutoPostBack="true" OnCheckedChanged="ChkForCompleteStatus_CheckedChanged" /></span>
                            </td>
                            <td id="TDFolioNo" runat="server" visible="false">
                                <asp:Label ID="Label12" Text="Enter Folio No" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtFolioNo" CssClass="textb" runat="server" Width="150px" Height="20px"
                                    AutoPostBack="true" OnTextChanged="txtFolioNo_TextChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label33" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDprocess" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDprocess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDProductionunit" runat="server" visible="false">
                                <asp:Label ID="Label1" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDLoomNo" runat="server" visible="false">
                                <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDLoomNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDFoliono" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDFoliono_SelectedIndexChanged">
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
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="EWay Bill No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtEWayBillNo" CssClass="textb" Width="150px" runat="server" Enabled="true" />
                            </td>
                            <td id="TDFolioIssueDate" runat="server" visible="false">
                                <asp:Label ID="Label11" runat="server" Text="Folio IssueDate" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtFolioIssueDate" CssClass="textb" Width="100px" runat="server"
                                    Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td id="TDTxtTotalPcs" runat="server" visible="false">
                                <asp:Label ID="Label10" runat="server" Text="Total Pcs" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtTotalPcs" CssClass="textb" Width="150px" runat="server" />
                            </td>
                            <td id="TDIssueNo" runat="server" visible="false">
                                <asp:Label ID="Label7" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDissueno" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label6" runat="server" Text="Raw Material Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 500px; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found." OnRowDataBound="DG_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Raw Material Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblunit" Text='<%#Bind("UnitName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Godown">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="DDgodown_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BinNo">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDBinNo" CssClass="dropdown" Width="150px" runat="server" OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lot No.">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDLotNo" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="DDLotno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tag No.">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDTagNo" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="DDTagno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstockqty" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Consmp 1 Pcs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOnePcsconsmpqty" Text='<%#Bind("OnePcdconsmpqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Consmp Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblconsmpqty" Text='<%#Bind("consmpqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Already Issued">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblalreadyeissued" Text='<%#Bind("issuedQty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BalanceQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceQty" Text='<%#Bind("BalanceQty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issue Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtissueqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--  <asp:TemplateField HeaderText="No of Cone">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtnoofcone" Width="50px" BackColor="Yellow" runat="server" 
                                                    onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblifinishedid" Text='<%#Bind("ifinishedid") %>' runat="server" />
                                                    <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                                    <asp:Label ID="lbliunitid" Text='<%#Bind("iunitid") %>' runat="server" />
                                                    <asp:Label ID="lblisizeflag" Text='<%#Bind("isizeflag") %>' runat="server" />
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
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="BtnUpdateStockNoQty" runat="server" Text="Update Stock No" Visible="false"
                                    CssClass="buttonnorm" OnClick="BtnUpdateStockNoQty_Click" />
                                <asp:Button ID="btnPreviewStockNo" runat="server" Text="Stock No Detail" Visible="false"
                                    CssClass="buttonnorm" OnClick="btnPreviewStockNo_Click" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="max-height: 300px; overflow: auto;">
                    <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="gvdetail_RowCancelingEdit"
                        EmptyDataText="No records found.." OnRowEditing="gvdetail_RowEditing" OnRowUpdating="gvdetail_RowUpdating"
                        OnRowDeleting="gvdetail_RowDeleting">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <Columns>
                            <asp:TemplateField HeaderText="Item Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LotNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblLotNo" Text='<%#Bind("LotNo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TagNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblTagNo" Text='<%#Bind("TagNo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtqty" Text='<%#Bind("IssueQuantity") %>' Width="70px" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblissueqty" Text='<%#Bind("IssueQuantity") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblhqty" Text='<%#Bind("IssueQuantity") %>' runat="server" />
                                    <asp:Label ID="lblprmid" Text='<%#Bind("prmid") %>' runat="server" />
                                    <asp:Label ID="lblprtid" Text='<%#Bind("prtid") %>' runat="server" />
                                    <asp:Label ID="lblprorderid" Text='<%#Bind("prorderid") %>' runat="server" />
                                    <asp:Label ID="lblprocessid" Text='<%#Bind("processid") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                        </Columns>
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hnissueid" runat="server" />
                <asp:HiddenField ID="hngodownid" runat="server" Value="0" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPreviewStockNo" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <style type="text/css">
        #mask
        {
            position: fixed;
            left: 0px;
            top: 0px;
            z-index: 4;
            opacity: 0.4;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
            filter: alpha(opacity=40); /* second!*/
            background-color: Gray;
            display: none;
            width: 100%;
            height: 100%;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ShowPopup() {
            $('#mask').show();
            $('#<%=pnlpopup.ClientID %>').show();
        }
        function HidePopup() {
            $('#mask').hide();
            $('#<%=pnlpopup.ClientID %>').hide();
        }
        $(".btnPwd").live('click', function () {
            HidePopup();
        });
    </script>
    <div id="mask">
    </div>
    <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="175px" Width="300px"
        Style="z-index: 111; background-color: White; position: absolute; left: 35%;
        top: 40%; border: outset 2px gray; padding: 5px; display: none">
        <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
            <tr style="background-color: #8B7B8B; height: 1px">
                <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                    align="center">
                    ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                        class="btnPwd" href="#">X</a>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Enter Password:
                </td>
                <td>
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnCheck" CommandName="Check" runat="server" Text="Check" CssClass="btnPwd"
                        ValidationGroup="m" OnClick="btnCheck_Click" />
                    <input type="button" value="Cancel" class="btnPwd" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
