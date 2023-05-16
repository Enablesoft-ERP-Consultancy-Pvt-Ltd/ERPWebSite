<%@ Page Title="WARPING ORDER" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmwarpIssueNew.aspx.cs" Inherits="Masters_WARP_frmwarpIssueNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmwarpIssuenew.aspx";
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
                    selectedindex = $("#<%=DDDept.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Dept. !!\n";
                    }
                    selectedindex = $("#<%=DDProcess.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Process. !!\n";
                    }
                    selectedindex = $("#<%=DDEmp.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Employee. !!\n";
                    }
                    selectedindex = $("#<%=DDcustcode.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Customer Code. !!\n";
                    }
                    selectedindex = $("#<%=DDorderNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Order No. !!\n";
                    }
                    selectedindex = $("#<%=DDitemdescription.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Order Description !!\n";
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
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                            <td id="TDcomplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="labelbold" runat="server" />
                            </td>
                        </tr>
                    </table>
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
                                <asp:DropDownList ID="DDDept" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDProductionUnit" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Process" CssClass="labelbold"></asp:Label><br />
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
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDItemDescription" runat="server">
                                <asp:Label ID="Label13" runat="server" Text="Order Description" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDitemdescription" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="370px" OnSelectedIndexChanged="DDitemdescription_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
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
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Target Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txttargetdate" CssClass="textb" Width="90px" runat="server" />
                                <asp:CalendarExtender ID="cal2" TargetControlID="txttargetdate" Format="dd-MMM-yyyy"
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
                                <div id="Div3" runat="server" style="max-height: 400px; overflow: auto">
                                    <asp:GridView ID="GvBeamDesc" runat="server" CssClass="grid-views" AutoGenerateColumns="false"
                                        EmptyDataText="No Records Found...">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" />
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
                                                    <asp:Label ID="lblpendqty" Text='<%# Convert.ToInt32(Eval("reqqty")) -Convert.ToInt32(Eval("issuedqty")) %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Beam Issue Pcs (*)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtissueqty" runat="server" Width="100px" BackColor="Yellow" Style="text-align: center" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No. of Beam Req. (*)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtnoofbeamreq" runat="server" Text="1" Style="text-align: center"
                                                        Width="100px" BackColor="Yellow" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate (*)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrate" runat="server" Text='<%#Bind("Rate") %>' Style="text-align: center"
                                                        Width="70px" BackColor="Yellow" Enabled="false" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("Item_finished_id") %>' runat="server" />
                                                    <asp:Label ID="lblosizeflag" Text='<%#Bind("Osizeflag") %>' runat="server" />
                                                    <asp:Label ID="lblarea" Text='<%#Bind("area") %>' runat="server" />
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
                            <asp:Button ID="btnupdateconsmp" runat="server" Text="Update Current Consumption"
                                CssClass="buttonnorm" Visible="false" OnClick="btnupdateconsmp_Click" />
                            <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <div id="gride" runat="server" style="max-height: 300px">
                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        OnRowDeleting="DG_RowDeleting" OnRowCancelingEdit="DG_RowCancelingEdit" OnRowEditing="DG_RowEditing"
                        OnRowUpdating="DG_RowUpdating">
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
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtnoofbeam" runat="server" Width="70px" Text='<%#Bind("Noofbeamreq") %>'
                                        onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </EditItemTemplate>
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
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="margin-top: 20px;">
                </div>
                <div style="width: 100%">
                    <div style="width: 50%; float: left">
                        <table>
                            <tr>
                                <td>
                                    <div id="Div1" runat="server" style="max-height: 500px; overflow: auto">
                                        <asp:GridView ID="DGRawdetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                            EmptyDataText="No records found." OnRowDataBound="DGRawdetail_RowDataBound" ShowFooter="true">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <FooterStyle CssClass="gvrow" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Raw Material Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="250px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblunit" Text='<%#Bind("UnitName") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lbltotal" runat="server" Text="Total" CssClass="labelbold" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Issue Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblissueqty" Text='<%#Bind("IQty") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblfooterissueqty" runat="server" />
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 50%; float: right">
                        <table>
                            <tr>
                                <td>
                                    <div id="Div2" runat="server" style="max-height: 300px">
                                        <asp:GridView ID="DGreceiveDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views">
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
                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblunit" Text='<%#Bind("Unitname") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hnid" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
