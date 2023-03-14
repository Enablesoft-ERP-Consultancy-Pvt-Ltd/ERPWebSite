<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmprocessWiseDebitNote.aspx.cs"
    Inherits="Masters_Hissab_frmprocessWiseDebitNote" MasterPageFile="~/ERPmaster.master"
    Title="DEBIT NOTE" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmprocessWiseDebitNote.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isnumber(evt) {
            var Charcode = (evt.which) ? evt.which : event.keycode
            if (Charcode != 46 && Charcode > 31 && (Charcode < 48 || Charcode > 57)) {
                alert('Plz Enter numeric value only...')
                return false;
            }
            else {
                return true;
            }
        }
        function validate() {
            if (document.getElementById("<%=DDCompanyName.ClientID %>").value <= "0") {
                alert("Plz Select Company Name");
                document.getElementById("<%=DDCompanyName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDProcessName.ClientID %>").value <= "0") {
                alert("Plz Select Process Name");
                document.getElementById("<%=DDProcessName.ClientID %>").focus();
                return false;

            }
            if (document.getElementById("<%=DDParty.ClientID %>").value <= "0") {
                alert("Plz Select Party Name");
                document.getElementById("<%=DDParty.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDBillNo.ClientID %>").value <= "0") {
                alert("Plz Select BillNo./IndentNo./FolioNo ");
                document.getElementById("<%=DDBillNo.ClientID %>").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_txtDebitAmt').value == "" || document.getElementById('CPH_Form_txtDebitAmt').value == "0") {
                alert("Pls fill Debit amount....!");
                document.getElementById('CPH_Form_txtDebitAmt').focus();
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    </script>
    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 20%;">
                <asp:Panel ID="panel1" runat="server" BorderStyle="Groove" Width="350px" BackColor="#8B7B8B"
                    ForeColor="White">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="RDForProduction" runat="server" Text=" For Production" CssClass="radiobuttonnormal"
                                    GroupName="DD" OnCheckedChanged="RDForProduction_CheckedChanged" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:RadioButton ID="RDForJoborder" runat="server" Text=" For Job Order" CssClass="radiobuttonnormal"
                                    GroupName="DD" OnCheckedChanged="RDForJoborder_CheckedChanged" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:RadioButton ID="RDForPurchase" runat="server" Text=" For Purchase" CssClass="radiobuttonnormal"
                                    GroupName="DD" OnCheckedChanged="RDForPurchase_CheckedChanged" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkForSample" runat="server" Text="Check For Sample Indent" CssClass="checkboxbold"
                                    Visible="false" Width="100px" AutoPostBack="true" OnCheckedChanged="ChkForSample_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div style="width: 100%">
                <div>
                    <table>
                        <tr>
                            <td>
                                <span class="labelbold">CompanyName</span>
                                <br />
                                <asp:DropDownList ID="DDCompanyName" runat="server" Width="170px" CssClass="dropdown">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchDDCompanyName" runat="server" TargetControlID="DDCompanyName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td>
                                <span class="labelbold">ProcessName</span>
                                <br />
                                <asp:DropDownList ID="DDProcessName" runat="server" Width="150px" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtenderDDProcess" runat="server" TargetControlID="DDProcessName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td>
                                <span class="labelbold">Party Name</span>
                                <br />
                                <asp:DropDownList ID="DDParty" runat="server" Width="150px" CssClass="dropdown" OnSelectedIndexChanged="DDParty_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtenderDDParty" runat="server" TargetControlID="DDParty"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td>
                                <span class="labelbold">Bill No./Indent No./Folio No.</span><br />
                                <asp:DropDownList ID="DDBillNo" runat="server" Width="160px" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDBillNo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtenderDDBill" runat="server" TargetControlID="DDBillNo"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td>
                                <span class="labelbold">Debit Date</span>
                                <br />
                                <asp:TextBox ID="txtDate" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalenderExt" runat="server" TargetControlID="txtDate" Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <span class="labelbold">Debit Amount</span>
                                <br />
                                <asp:TextBox ID="txtDebitAmt" runat="server" CssClass="textb" Width="90px" BackColor="Beige"
                                    onkeypress="return isnumber(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>

                            <td id="TDGSTPercentage" runat="server" visible="false">
                                <span class="labelbold">Gst(%)</span>
                                <br />
                                <asp:TextBox ID="txtGst" runat="server" CssClass="textb" Width="90px" BackColor="Beige"
                                    onkeypress="return isnumber(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>

                        </tr>
                    </table>
                    <table width="80%">
                        <tr>
                            <td>
                                <asp:Label ID="lblremarks" runat="server" Text="Remarks" class="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtremarks" runat="server" CssClass="textb" Width="342px" TextMode="MultiLine"
                                    Height="49px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblGateNo" runat="server" Text="" class="labelbold" ForeColor="Red"
                                    Font-Size="14px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="right">
                                <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                    TabIndex="6" />
                                &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClientClick="return validate();"
                                    TabIndex="7" OnClick="BtnSave_Click" />
                                &nbsp;<asp:Button ID="BtnPriview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                                    TabIndex="8" OnClick="BtnPriview_Click" />
                                &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                                    OnClientClick="return CloseForm();" TabIndex="9" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 200px; background-color: Gray; overflow: auto; width: 700px">
                    <asp:GridView ID="GVDetail" runat="server" AutoGenerateColumns="False">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <asp:BoundField DataField="OrderNo" HeaderText="OrderNo">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Date" HeaderText="Date">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Amount" HeaderText="Amount">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Id" HeaderText="DebitNo.">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                    <asp:Label ID="lbltype" Text='<%#Bind("Type") %>' runat="server" />
                                    <asp:Label ID="lblbillid" Text='<%#Bind("billid") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--   <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnlDel" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" OnClientClick="return confirm('Do you want to delete data?')"
                                        OnClick="lnkdel_Click" Text="Del"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkpreview" runat="server" CausesValidation="False" Text="Preview"
                                        OnClick="lnkpreview_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <table border="1">
                        <tr>
                            <td>
                                <asp:Label ID="lblfromdate" Text="FromDate" CssClass="labelbold" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label1" Text="ToDate" CssClass="labelbold" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="btndebitdetail" Text="Debit Detail" runat="server" CssClass="buttonnorm"
                                    OnClick="btndebitdetail_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:HiddenField ID="hnDebitNo" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
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
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px"
                        OnTextChanged="txtpwd_TextChanged" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <input type="button" value="Cancel" class="btnPwd" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label19" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
