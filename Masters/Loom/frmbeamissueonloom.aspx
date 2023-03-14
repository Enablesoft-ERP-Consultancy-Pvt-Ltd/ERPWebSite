<%@ Page Title="BEAM ISSUE" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmbeamissueonloom.aspx.cs" Inherits="Masters_Loom_frmbeamissueonloom" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Loomnoselected(source, eventArgs) {
            document.getElementById('<%=txtloomid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnSearch.ClientID%>').click();
        }
        function NewForm() {
            window.location.href = "frmbeamissueonloom.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
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
    <style type="text/css">
        .WordWrap
        {
            width: 100%;
            word-break: break-all;
        }
        .WordBreak
        {
            width: 100px;
            overflow: hidden;
            text-overflow: ellipsis;
        }
    </style>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDProdunit.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Production Unit. !!\n";
                    }
                    var txtloomno = document.getElementById('<%=txtloomno.ClientID %>');
                    if (txtloomno.value == "") {
                        Message = Message + "Please Enter Loom No. !!\n";
                    }
                    //                    selectedindex = $("#<%=DDLoomNo.ClientID %>").attr('selectedIndex');
                    //                    if (selectedindex <= 0) {
                    //                        Message = Message + "Please select Loom No. !!\n";
                    //                    }
                    selectedindex = $("#<%=DDFoliono.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Folio No. !!\n";
                    }
                    var txtissuedate = document.getElementById('<%=txtissuedate.ClientID %>');
                    if (txtissuedate.value == "") {
                        Message = Message + "Please Enter Issue Date. !!\n";
                    }
                    selectedindex = $("#<%=DDGodown.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Godown !!\n";
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
    <div>
        <asp:UpdatePanel ID="upd2" runat="server">
            <ContentTemplate>
                <script type="text/javascript" language="javascript">
                    Sys.Application.add_load(Jscriptvalidate);
                </script>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="checkboxbold" runat="server"
                                    AutoPostBack="true" OnCheckedChanged="chkEdit_CheckedChanged" />
                            </td>
                            <td id="TDcomplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="checkboxbold" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td id="TDLoomno" runat="server" visible="false">
                                            <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" Width="150px"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDLoomNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TDLoomNotextbox" runat="server">
                                            <asp:Label ID="Label10" Text=" Loom No." runat="server" CssClass="labelbold" />
                                            <asp:Button ID="btnSearch" runat="server" Text="Button" OnClick="btnSearch_Click"
                                                Style="display: none;" />
                                            <asp:TextBox ID="txtloomid" runat="server" Style="display: none"></asp:TextBox>
                                            <br />
                                            <asp:TextBox ID="txtloomno" CssClass="textb" runat="server" Width="150px" />
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtenderloomno" runat="server" BehaviorID="LoomSrchAutoComplete"
                                                CompletionInterval="20" Enabled="True" ServiceMethod="GetLoomNo" EnableCaching="true"
                                                CompletionSetCount="30" OnClientItemSelected="Loomnoselected" ServicePath="~/Autocomplete.asmx"
                                                TargetControlID="txtloomno" UseContextKey="true" ContextKey="0" MinimumPrefixLength="1">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDFoliono" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDFoliono_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissueno" CssClass="textb" Width="90px" runat="server" Enabled="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissuedate" CssClass="textb" Width="95px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtissuedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Godown" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDGodown" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDGodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDissue" runat="server" visible="false">
                                <asp:Label ID="Label7" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDissueno" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label5" runat="server" Text="BEAM STOCK DETAIL" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 300px; overflow: auto" class="WordWrap">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found.">
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
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" Width="10px" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Beam No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbeamno" Text='<%#Bind("BeamNo") %>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Beam Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("BeamDescription") %>' runat="server"
                                                        Width="150px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lot No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllotno" Text='<%#Bind("Lotno") %>' runat="server" Width="250px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tag No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltagno" Text='<%#Bind("Tagno") %>' runat="server" Width="250px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pcs(Beam)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpcs" Text='<%#Bind("pcs") %>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gross Wt(kg.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrossweight" Text='<%#Bind("grossWeight") %>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tare Wt(kg.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltareweight" Text='<%#Bind("tareWeight") %>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Net Wt(kg.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnetweight" Text='<%#Bind("netWeight") %>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("ofinishedid") %>' runat="server"
                                                        Width="50px" />
                                                    <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" Width="50px" />
                                                    <asp:Label ID="lblflagsize" Text='<%#Bind("oSizeflag") %>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
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
                <fieldset>
                    <legend>
                        <asp:Label ID="Label8" runat="server" Text="BEAM ISSUED DETAIL" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="Div1" runat="server" style="max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DGIssueDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found." OnRowDeleting="DGIssueDetail_RowDeleting">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Beam No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbeamno" Text='<%#Bind("BeamNo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Beam Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("BeamDescription") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pcs(Beam)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpcs" Text='<%#Bind("pcs") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblprmid" Text='<%#Bind("prmid") %>' runat="server" />
                                                    <asp:Label ID="lblprtid" Text='<%#Bind("prtid") %>' runat="server" />
                                                    <asp:Label ID="lblprorderid" Text='<%#Bind("prorderid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:HiddenField ID="hnprmid" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
