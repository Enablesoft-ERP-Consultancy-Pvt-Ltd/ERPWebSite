<%@ Page Title="Sample Master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmsamplemaster.aspx.cs" Inherits="Masters_Sample_frmsamplemaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmsamplemaster.aspx";
        }
        function Samplecodeselected(source, eventArgs) {
            document.getElementById('<%=hnid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnSearch.ClientID%>').click();
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {

                    var Message = "";
                    if ($("#<%=TDbuyer.ClientID %>").is(':visible')) {
                        selectedindex = $("#<%=DDbuyer.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Buyer. !!\n";
                        }
                    }
                    var txtproduct = document.getElementById('<%=txtproduct.ClientID %>');
                    if (txtproduct.value == "") {
                        Message = Message + "Please Enter Product. !!\n";
                    }
                    var txtSize = document.getElementById('<%=txtSize.ClientID %>');
                    if (txtSize.value == "") {
                        Message = Message + "Please Enter Size. !!\n";
                    }
                    var txtFabric = document.getElementById('<%=txtFabric.ClientID %>');
                    if (txtFabric.value == "") {
                        Message = Message + "Please Enter Fabric. !!\n";
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div style="margin-left: 20%">
                <div style="width: 50%; float: left">
                    <table style="height: 100%; width: 100%;" cellspacing="5" border="1px Solid Grey">
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkedit" Text="For Edit" runat="server" CssClass="checkboxbold"
                                    AutoPostBack="true" OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                        </tr>
                        <tr id="TRSamplecode" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label1" Text="Type Sample Code" CssClass="labelbold" runat="server" />
                                <%--<asp:TextBox ID="txtsampleid" runat="server" Style="display: none"></asp:TextBox>--%>
                                <asp:Button ID="btnSearch" runat="server" Text="Button" OnClick="btnSearch_Click"
                                    Style="display: none" />
                            </td>
                            <td>
                                <asp:TextBox ID="txttypesamplecode" CssClass="textb" runat="server" Width="120px" />
                                <asp:AutoCompleteExtender ID="AutoCompleteExtendersamplecode" runat="server" BehaviorID="SampleSrchAutoComplete"
                                    CompletionInterval="20" Enabled="True" ServiceMethod="Getsamplecode" EnableCaching="true"
                                    CompletionSetCount="30" OnClientItemSelected="Samplecodeselected" ServicePath="~/Autocomplete.asmx"
                                    TargetControlID="txttypesamplecode" UseContextKey="true" ContextKey="0" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsample" Text="Sample Code" CssClass="labelbold" runat="server" />
                            </td>
                            <td id="TDtxtsamplecode" runat="server">
                                <asp:TextBox ID="txtsamplecode" Enabled="false" CssClass="textb" runat="server" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDate" Text="Date" CssClass="labelbold" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDate" runat="server" CssClass="textb" Width="120px" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtDate" Format="dd-MMM-yyyy" runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblpurpose" Text="Purpose" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DDpurpose" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDpurpose_SelectedIndexChanged" Width="125px">
                                    <asp:ListItem Text="GENERAL" />
                                    <asp:ListItem Text="BUYER" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label Text="Buyer/General" CssClass="labelbold" runat="server" />
                            </td>
                            <td id="TDgeneral" runat="server">
                                <asp:TextBox ID="txtgeneral" CssClass="textb" runat="server" Width="120px" />
                            </td>
                            <td id="TDbuyer" runat="server" visible="false">
                                <asp:DropDownList ID="DDbuyer" CssClass="dropdown" Width="150px" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblproduct" CssClass="labelbold" Text="Product" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtproduct" CssClass="textb" Width="250px" runat="server" TextMode="MultiLine"
                                    Height="60px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsize" CssClass="labelbold" Text="Size" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtSize" CssClass="textb" runat="server" Width="120px" />
                                <asp:DropDownList ID="DDsizetype" CssClass="dropdown" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblFabric" CssClass="labelbold" Text="Fabric" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFabric" CssClass="textb" Width="250px" runat="server" TextMode="MultiLine"
                                    Height="60px" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lbldesc" CssClass="labelbold" Text="Description" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtdesc" CssClass="textb" Width="250px" runat="server" TextMode="MultiLine"
                                    Height="60px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btndelete" CssClass="buttonnorm" Text="Delete" runat="server" Visible="false"
                                    OnClick="btndelete_Click" OnClientClick="return confirm('Do you want to delete this sample code?');" />
                                <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click" />
                                <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm(); " />
                                <asp:Button ID="btnnew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return ClickNew()" />
                                <asp:Button ID="btnpreview" CssClass="buttonnorm" Text="Preview" runat="server" OnClick="btnpreview_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 40%; float: right">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Image ID="lblimage" runat="server" Height="150px" Width="170px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:FileUpload ID="PhotoImage" ViewStateMode="Enabled" runat="server" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                    ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="PhotoImage"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="clear: both">
                </div>
                <div>
                    <table style="width: 100%; text-align: center">
                        <tr>
                            <td style="text-align: left">
                                <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:HiddenField ID="hnid" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
