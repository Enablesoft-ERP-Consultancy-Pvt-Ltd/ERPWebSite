<%@ Page Title="Warp Convert Description" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmWarpCovertDescription.aspx.cs" Inherits="Masters_WARP_FrmWarpCovertDescription" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmWarpCovertDescription.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function validate() {
            if (document.getElementById("<%=DDcompany.ClientID %>").value <= "0") {
                alert('Plz Select Company Name...');
                document.getElementById("<%=DDcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDDept.ClientID %>").value <= "0") {
                alert('Plz Select Department...');
                document.getElementById("<%=DDDept.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDProcess.ClientID %>").value <= "0") {
                alert('Plz Select Process Name...');
                document.getElementById("<%=DDProcess.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDEmp.ClientID %>").value == "0") {
                alert('Plz Select employee Name');
                document.getElementById("<%=DDEmp.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDChangeOrderDescription.ClientID %>").value == "0") {
                alert('Plz Select Change Order description');
                document.getElementById("<%=DDChangeOrderDescription.ClientID %>").focus();
                return false;
            }

            return confirm('Do you want to save Data');
        }
    </script>
    <asp:UpdatePanel ID="R" runat="server">
        <ContentTemplate>
            <div>
                <fieldset>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Department" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDept" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDDept_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Process" CssClass="labelbold"></asp:Label>
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkedit_CheckedChanged" /><br />
                                <asp:DropDownList ID="DDProcess" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDProcess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Employee" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDEmp" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDEmp_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDcustcode" runat="server">
                                <asp:Label ID="Label12" runat="server" Text="Customer Code" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDcustcode" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDorderNo" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Order No" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDorderNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDorderNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDItemDescription" runat="server">
                                <asp:Label ID="Label13" runat="server" Text="Order Description" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDitemdescription" runat="server" CssClass="dropdown" Width="400px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDBeamDescription" runat="server">
                                <asp:Label ID="Label4" runat="server" Text="Beam Description" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDBeamDescription" runat="server" CssClass="dropdown" Width="400px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="BtnShow" runat="server" Text="Show Data" CssClass="buttonnorm" OnClick="BtnShow_Click" />
                            </td>
                            <td id="TDChangeOrderDescription" runat="server">
                                <asp:Label ID="Label5" runat="server" Text="Change Order Description" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDChangeOrderDescription" runat="server" CssClass="dropdown"
                                    Width="600px" AutoPostBack="true" OnSelectedIndexChanged="DDChangeOrderDescription_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbltotalpcs" Text="Order Pcs" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txttotalpcs" CssClass="textb" Width="100px" runat="server" Enabled="False"
                                    BackColor="LightGray" />
                            </td>
                            <td>
                                <asp:Label ID="Label14" Text="Order Area" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txttotalarea" CssClass="textb" Width="100px" runat="server" Enabled="False"
                                    BackColor="LightGray" />
                            </td>
                            <td id="TDissueNo" runat="server" visible="false">
                                <asp:Label ID="Label10" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDissueNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDissueNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissueno" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissuedate" CssClass="textb" Width="90px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtissuedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label16" runat="server" Text="Beam Details (* Mandatory Fields)" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="Div3" runat="server" style="max-height: 250px; overflow: auto">
                                    <asp:GridView ID="GvBeamDesc" runat="server" CssClass="grid-views" AutoGenerateColumns="false"
                                        EmptyDataText="No Records Found..." OnRowDataBound="GvBeamDesc_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" AutoPostBack="true" OnCheckedChanged="Chkboxitem_CheckChanged" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderNo" Text='<%#Bind("OrderNo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblOrderDescription" Text='<%#Bind("OrderDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Beam No" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBeamNo" Text='<%#Bind("BeamNo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Beam Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblBeamDescription" Text='<%#Bind("BeamDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pcs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPcs" Text='<%#Bind("Pcs") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblID" Text='<%#Bind("ID") %>' runat="server" />
                                                    <asp:Label ID="LblDetailID" Text='<%#Bind("Detailid") %>' runat="server" />
                                                    <asp:Label ID="LblIssueMasterID" Text='<%#Bind("Issuemasterid") %>' runat="server" />
                                                    <asp:Label ID="LblIssueDetailID" Text='<%#Bind("IssueDetailid") %>' runat="server" />
                                                    <asp:Label ID="LblProcessID" Text='<%#Bind("ProcessID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                            <td>
                                <div id="Div1" runat="server" style="max-height: 400px; overflow: auto">
                                    <asp:GridView ID="GDBeamDescription" runat="server" CssClass="grid-views" AutoGenerateColumns="false"
                                        EmptyDataText="No Records Found...">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkBoxItemBeamDescription" runat="server" AutoPostBack="true" OnCheckedChanged="ChkBoxItemBeamDescription_CheckChanged" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Beam Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbeamdesc" Text='<%#Bind("BeamDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reqd. Pcs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblreqqty" Text='<%#Bind("reqqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issued Pcs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblissuedqty" Text='<%#Bind("Issuedqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pending Pcs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpendqty" Text='<%# Convert.ToInt32(Eval("reqqty")) - Convert.ToInt32(Eval("issuedqty")) %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("Item_finished_id") %>' runat="server" />
                                                    <asp:Label ID="lblosizeflag" Text='<%#Bind("Osizeflag") %>' runat="server" />
                                                    <asp:Label ID="lblarea" Text='<%#Bind("area") %>' runat="server" />
                                                    <asp:Label ID="LblPendingPcs" Text='<%# Convert.ToInt32(Eval("reqqty")) - Convert.ToInt32(Eval("issuedqty")) %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
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
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClientClick="return validate();"
                                OnClick="btnsave_Click" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="gride" runat="server" style="max-height: 300px">
                <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                    OnRowDeleting="DG_RowDeleting">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Beam Description">
                            <ItemTemplate>
                                <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="350px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pcs. Qty">
                            <ItemTemplate>
                                <asp:Label ID="lblorderQty" Text='<%#Bind("pcs") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No. of Beam Req.">
                            <ItemTemplate>
                                <asp:Label ID="lblnoofbeamreq" Text='<%#Bind("Noofbeamreq") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Area">
                            <ItemTemplate>
                                <asp:Label ID="lblarea" Text='<%#Bind("Area") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:HiddenField ID="HNID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
