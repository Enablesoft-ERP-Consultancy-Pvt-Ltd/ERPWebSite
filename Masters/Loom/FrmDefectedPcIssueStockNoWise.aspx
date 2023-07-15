<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmDefectedPcIssueStockNoWise.aspx.cs" Inherits="Masters_Loom_FrmDefectedPcIssueStockNoWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmDefectedPcIssueStockNoWise.aspx";
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

        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {
                        inputlist[i].checked = true;
                    }
                    else {
                        inputlist[i].checked = false;
                    }
                }
            }
        }
    </script>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table style="width: 100%">
                        <tr>
                            <td id="TDchkedit" runat="server" visible="false">
                                <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                            <td>
                                <asp:TextBox ID="TxtUserType" runat="server" Style="display: none"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 80%" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label35" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label1" runat="server" Text="ProcessName" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="150" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="Rec Date" CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="txtRecdate" CssClass="textb" Width="95px" runat="server" />
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtRecdate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="TxtIssueNo" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="txtIssuedate" CssClass="textb" Width="95px" runat="server" />
                                            <asp:CalendarExtender ID="cal1" TargetControlID="txtIssuedate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnShowData" Text="ShowData" CssClass="buttonnorm" runat="server"
                                                OnClick="ShowData_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 20%" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label4" Text="Product Description" runat="server" CssClass="labelbold"
                            ForeColor="Red" />
                    </legend>
                    <table width="80%">
                        <tr>
                            <td>
                                <div id="Div2" runat="server" style="max-height: 300px; width: 100%; overflow: auto">
                                    <asp:GridView ID="DGIssueDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found." OnRowDataBound="DGIssueDetail_RowDataBound"
                                        Width="100%">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkheader" Text="" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkitem" Text="" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sr No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltstockno" Text='<%#Bind("TStockno") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Parameter Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParaName" Text='<%#Bind("ParaName") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdesc" Text='<%#Bind("itemdesc") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Chk Reject">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDReject" runat="server" CssClass="dropdown">
                                                        <asp:ListItem Value="1">Re-Work</asp:ListItem>
                                                        <asp:ListItem Value="2">Rejected</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStockNo" Text='<%#Bind("StockNo") %>' runat="server" />
                                                    <asp:Label ID="lblprocessrecid" Text='<%#Bind("Process_Rec_Id") %>' runat="server" />
                                                    <asp:Label ID="lblprocessrecdetailid" Text='<%#Bind("Process_Rec_Detail_Id") %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    UseSubmitBehavior="false" OnClientClick="if (!confirm('Do you want to save Data?')) return; this.disabled=true;this.value = 'wait ...';" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" Visible="false" CssClass="buttonnorm"
                                    OnClick="btnPreview_Click" />
                                <%--<asp:Button ID="btnReturnGatePass" runat="server" Text="Return GatePass" CssClass="buttonnorm"
                                    OnClick="btnReturnGatePass_Click" />--%>
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 80%" valign="top">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="background-color: #faf9f7">
                                            <asp:Label Text="Received Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <div id="Div1" runat="server" style="max-height: 300px; width: 100%; overflow: auto">
                                                            <asp:GridView ID="DGRecDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                                EmptyDataText="No. Records found." OnRowDataBound="DGRecDetail_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle CssClass="gvheaders" />
                                                                <AlternatingRowStyle CssClass="gvalts" />
                                                                <RowStyle CssClass="gvrow" />
                                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Item Description">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rec Qty.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblrecqty" Text='<%#Bind("Recqty") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Weight (Kg.)">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtweight" Text='<%#Bind("Weight") %>' runat="server" Width="80px" />
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblweight" Text='<%#Bind("Weight") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtrategrid" Width="70px" Text='<%#Bind("Rate") %>' runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comm.Rate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcommrate" Text='<%#Bind("Comm") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtcommrategrid" Width="70px" Text='<%#Bind("comm") %>' runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Penality">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpenalitygrid" Text='<%#Bind("penality") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtpenalitygrid" Width="70px" Text='<%#Bind("penality") %>' runat="server"
                                                                                onkeypress="return isNumberKey(event);" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Penality Remark">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpenalityremarkgrid" Text='<%#Bind("Premarks") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtpenalityremarkgrid" Width="250px" Text='<%#Bind("Premarks") %>'
                                                                                runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Stock No.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblstockno" Text='<%#Bind("StockNo") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Grade">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCarpetGradeName" runat="server" Text='<%#Bind("CarpetGradeName") %>'></asp:Label>
                                                                            <asp:Label ID="lblCarpetGradeId" runat="server" Text='<%#Bind("CarpetGrade") %>'
                                                                                Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="QA NAME">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblqanamegrid" Text='<%#Bind("Qaname") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtqanamegrid" Width="250px" Text='<%#Bind("Qaname") %>' runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprocessrecid" Text='<%#Bind("Process_Rec_Id") %>' runat="server" />
                                                                            <asp:Label ID="lblprocessrecdetailid" Text='<%#Bind("Process_Rec_Detail_Id") %>'
                                                                                runat="server" />
                                                                            <asp:Label ID="lblHrate" Text='<%#Bind("Rate") %>' runat="server" />
                                                                            <asp:Label ID="lblHcommrate" Text='<%#Bind("comm") %>' runat="server" />
                                                                            <asp:Label ID="lblDefectStatus" Text='<%#Bind("DefectStatus") %>' runat="server" />
                                                                            <asp:Label ID="lblQualityId" Text='<%#Bind("QualityId") %>' runat="server" />
                                                                            <asp:Label ID="lblCalType" Text='<%#Bind("CalType") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Actual Width">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblactualwidth" Text='<%#Bind("Actualwidth") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtawidthgrid" Text='<%#Bind("Actualwidth") %>' Width="80px" runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Actual Length">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblactuallength" Text='<%#Bind("ActualLength") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtalengthgrid" Text='<%#Bind("ActualLength") %>' Width="80px" runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--<asp:TemplateField HeaderText="QC CHECK">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkQccheck" runat="server" CausesValidation="False" Text="QCCHECK"
                                                                                OnClientClick="return confirm('Do you want to check QC?')" OnClick="lnkqcparameter_Click"></asp:LinkButton>
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
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkRemoveQccheck" runat="server" CausesValidation="False" Text="REMOVE QC"
                                                                                OnClientClick="return confirm('Do you want to remove QC?')" OnClick="lnkRemoveQccheck_Click"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Add Penality" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkAddPenality" runat="server" Text="Add Penality" OnClick="lnkAddPenality_Click">
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div>
                    <asp:HiddenField ID="hnissueorderid" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
