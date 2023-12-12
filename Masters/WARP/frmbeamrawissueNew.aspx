<%@ Page Title="WARPING MATERIAL ISSUE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmbeamrawissueNew.aspx.cs" Inherits="Masters_WARP_frmbeamrawissue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmbeamrawissuenew.aspx";
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
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="Chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="Chkedit_CheckedChanged" />
                            </td>
                             <td id="TDcomplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="labelbold" runat="server" AutoPostBack="true" OnCheckedChanged="chkcomplete_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Department" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDept" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Process" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDProcess" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDProcess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Employee Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDemployee" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="170px" OnSelectedIndexChanged="DDemployee_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIssueNo" runat="server">
                                <asp:Label ID="Label4" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDIssueno" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="130px" OnSelectedIndexChanged="DDIssueno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblarticleroll" Text="Article Roll" runat="server" CssClass="labelbold" /><br />
                                <asp:DropDownList ID="DDArtilceroll" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="370px" OnSelectedIndexChanged="DDArtilceroll_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="No. of Pcs" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtnoofpcs" CssClass="textb" Width="90px" runat="server" Enabled="false"
                                    BackColor="LightGray" />
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="No. of Beam" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtnoofbeam" CssClass="textb" Width="90px" runat="server" Enabled="false"
                                    BackColor="LightGray" />
                            </td>
                            <td id="TDGatepassNo" runat="server" visible="false">
                                <asp:Label ID="Label6" runat="server" Text="Gate Pass No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDGatepassNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="130px" OnSelectedIndexChanged="DDGatepassNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Gate Pass No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtogpno" CssClass="textb" Width="90px" runat="server" Enabled="false"
                                    BackColor="LightGray" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissuedate" CssClass="textb" Width="90px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtissuedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="Remark" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtRemark" CssClass="textb" Width="500px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label5" runat="server" Text="Issue Details" CssClass="labelbold" ForeColor="Red"
                            Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 500px; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        OnRowDataBound="DG_RowDataBound" ShowFooter="true">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <FooterStyle CssClass="gvrow" />
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
                                                    <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="130px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="DDgodown_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lot No.">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDLotNo" CssClass="dropdown" Width="130px" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="DDLotno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tag No.">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDTagNo" CssClass="dropdown" Width="130px" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="DDTagno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bin_No">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDBinNo" CssClass="dropdown" Width="130px" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstockqty" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblftotal" Text="Total" CssClass="labelbold" runat="server" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty to be Issued">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblqtytobeissued" Text='<%#Bind("IssueQty") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblfqtytobeissued" runat="server" CssClass="labelbold" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Already Issued">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblalreadyeissued" Text='<%#Bind("issuedQty") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblfalreadyeissued" runat="server" CssClass="labelbold" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issue Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtissueqty" Width="60px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No of Cone">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtnoofcone" Width="40px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                                    <asp:Label ID="lblifinishedid" Text='<%#Bind("ifinishedid") %>' runat="server" />
                                                    <asp:Label ID="lbliunitid" Text='<%#Bind("iunitid") %>' runat="server" />
                                                    <asp:Label ID="lblisizeflag" Text='<%#Bind("isizeflag") %>' runat="server" />
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
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
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table>
                        <tr>
                            <td>
                                <div id="Div1" runat="server" style="max-height: 500px; overflow: auto">
                                    <asp:GridView ID="DGRawIssueDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        OnRowEditing="DGRawIssueDetail_RowEditing" OnRowCancelingEdit="DGRawIssueDetail_RowCancelingEdit"
                                        OnRowUpdating="DGRawIssueDetail_RowUpdating" OnRowDeleting="DGRawIssueDetail_RowDeleting" OnRowDataBound="DGRawIssueDetail_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
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
                                            <asp:TemplateField HeaderText="Lot No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllotno" Text='<%#Bind("Lotno")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tag No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltagno" Text='<%#Bind("Tagno")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issued Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblissueqty" Text='<%#bind("IssueQty") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtissuedqty" Text='<%#Bind("Issueqty") %>' Width="70px" runat="server"
                                                        onkeypress="return isNumberKey(event);" />
                                                </EditItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No of Cone">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnoofcone" runat="server" Text='<%#Bind("Noofcone") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtnoofcone" Text='<%#Bind("Noofcone") %>' Width="70px" runat="server"
                                                        onkeypress="return isNumberKey(event);" />
                                                </EditItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                                    <asp:Label ID="lblDetailid" Text='<%#Bind("DetailId") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <%-- <asp:TemplateField>
                                                <ItemTemplate>
                                                   
                                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:CommandField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--<div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="Label6" runat="server" Text="Received Details" CssClass="labelbold"
                                ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                        <table>
                            <tr>
                                <td>
                                    <div id="Div1" runat="server" style="max-height: 300px">
                                        <asp:GridView ID="DGReceivedDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                            OnRowDeleting="DGReceivedDetail_RowDeleting">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Receive No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrecno" Text='<%#Bind("ReceiveNo") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ItemDescription">
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
                                                        <asp:Label ID="lblgodown" Text='<%#Bind("GodownName")%>' runat="server" />
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
                                                <asp:TemplateField HeaderText="Rec Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrecqty" Text='<%#Bind("ReceiveQty") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Loss Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbllossqty" Text='<%#Bind("Lossqty") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rec Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRectype" Text='<%#Bind("Rectype") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No of Cone">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblnoofcone" Text='<%#Bind("Noofcone") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cone Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblconetype" Text='<%#Bind("conetype") %>' runat="server" />
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
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblissuemasterid" Text='<%#Bind("issuemasterId") %>' runat="server" />
                                                        <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                                        <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                                        <asp:Label ID="lblissuemasterdetailid" Text='<%#Bind("issuemasterdetailid") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
