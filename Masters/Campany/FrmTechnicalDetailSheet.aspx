<%@ Page Title="Technical Detail Sheet" Language="C#" AutoEventWireup="true" CodeFile="FrmTechnicalDetailSheet.aspx.cs"
    Inherits="Masters_Campany_FrmTechnicalDetailSheet" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function getbacktostepone() {
            window.lcation = "formWareHouseMaster.aspx";
        }
        function NewForm() {
            window.location.href = "FrmTechnicalDetailSheet.aspx";
        }
        function onSuccess() {
            setTimeout(okay, 200);
        }
        function onError() {
            setTimeout(getbacktostepone, 200);
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
        }
        function Priview() {
            window.open('../../ReportViewer.aspx', '');
        }

        function addRow(Table1) {

            var table = document.getElementById(Table1);

            var rowCount = table.rows.length;
            var row = table.insertRow(rowCount);

            var cell1 = row.insertCell(0);
            var element1 = document.createElement("input");
            element1.type = "checkbox";
            cell1.appendChild(element1);

            var cell2 = row.insertCell(1);
            cell2.innerHTML = rowCount + 1;

            var cell3 = row.insertCell(2);
            var element2 = document.createElement("input");
            element2.type = "text";
            cell3.appendChild(element2);

        }

        function hideDiv() {

            if (document.getElementById('CPH_Form_chkcustomertesting').checked) {

                document.getElementById("CPH_Form_cutomertestingrequriment").style.visibility = 'visible';
            }
            else {

                document.getElementById("CPH_Form_cutomertestingrequriment").style.visibility = 'hidden';
            }

        }
        function hidecutting() {


            if (document.getElementById('CPH_Form_chkcutting').checked) {
                document.getElementById("CPH_Form_detailcutting").style.visibility = 'visible';
            }
            else {

                document.getElementById("CPH_Form_detailcutting").style.visibility = 'hidden';

            }
        }
        function hideweaving() {

            if (document.getElementById('CPH_Form_chkforweaving').checked) {
                document.getElementById("CPH_Form_detailweaving").style.visibility = 'visible';
            }
            else {
                document.getElementById("CPH_Form_detailweaving").style.visibility = 'hidden';
            }
        }
        function hideends() {

            if (document.getElementById('CPH_Form_chkends').checked) {
                document.getElementById("CPH_Form_detailends").style.visibility = 'visible';
            }
            else {
                document.getElementById("CPH_Form_detailends").style.visibility = 'hidden';
            }
        }
        function Checkforitem() {
            if (document.getElementById('chkcustomertesting').checked) {
                var answer = confirm("Do you want to ADD?")

                if (answer) {
                    var a = document.getElementById('txtEmpId').value;
                    if (a == "" || a == "0") {
                        alert('Plz Select or Insert Employee');
                        return false;
                    }


                }
            }

        }
    
             
    </script>
    <div>
        <asp:UpdatePanel ID="updatepanel" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            MASTER QUALITY
                        </td>
                        <td>
                            <asp:DropDownList AutoPostBack="true" ID="dditemname" runat="server" Width="140px"
                                CssClass="dropdown" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            FILE NO
                        </td>
                        <td>
                            <asp:TextBox ID="txtfileno" runat="server" AutoPostBack="true" CssClass="textb" OnTextChanged="txtfileno_TextChanged">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            SUB QUALITY
                        </td>
                        <td>
                            <asp:DropDownList ID="ddsubquality" AutoPostBack="true" runat="server" Width="140px"
                                CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            VERSION
                        </td>
                        <td>
                            <asp:TextBox ID="txtversion" Enabled="false" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            OUR REF #
                        </td>
                        <td>
                            <asp:TextBox ID="txtourref" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td>
                            DATE
                        </td>
                        <td>
                            <asp:TextBox ID="txtdate" runat="server" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtendertxtdate" runat="server" TargetControlID="txtdate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Design
                        </td>
                        <td>
                            <asp:DropDownList ID="dddesign" AutoPostBack="true" runat="server" Width="140px"
                                CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            SR NO.
                        </td>
                        <td>
                            <asp:TextBox ID="txtsrno" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div style="width: 500px;">
                    <asp:CheckBox ID="ChkWarp_WeftPrint" Text="Check For Print" runat="server" CssClass="checkboxbold" />
                    <asp:GridView Width="500px" ID="Gv1" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="RowNumber" HeaderText="Sr No." />
                            <asp:TemplateField HeaderText="WARP/WEFT/PILE/EMBROIDERY/POM/">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YARN">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="COUNT & DESCREPTION  IF ANY">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <FooterTemplate>
                                    <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" OnClick="ButtonAdd_Click" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="width: 515px;">
                    <asp:CheckBox ID="chkDefineProcess" Text="Check For Print" runat="server" CssClass="checkboxbold" />
                    <asp:GridView Width="515px" ID="Gv2" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                        <HeaderStyle CssClass="gvheader" Height="20px" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="RowNumber" HeaderText="Sr No." />
                            <asp:TemplateField HeaderText="DEFINE PROCESS STEPS">
                                <ItemTemplate>
                                    <asp:TextBox ID="Tb1" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NAME  OF THE PROCESS">
                                <ItemTemplate>
                                    <asp:TextBox ID="Tb2" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RESPONSIBLE DEPARTMENT">
                                <ItemTemplate>
                                    <asp:TextBox ID="Tb3" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <FooterTemplate>
                                    <asp:Button ID="ButtonAdd1" runat="server" Text="Add New Row" OnClick="ButtonAdd1_Click1" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <br />
                <div style="background-color: teal; width: 515px;">
                    <asp:CheckBox ID="chkcustomertesting" ForeColor="White" Text="CUSTOMER TESTING REQUIREMENT"
                        AutoPostBack="true" CssClass="checkboxnormal" runat="server" OnCheckedChanged="chkcustomertesting_CheckedChanged" /></div>
                <div id="cutomertestingrequriment" style="width: 515px;" visible="false" runat="server">
                    <font size="3px" color="teal"><b>CUSTOMER TESTING REQUIREMENT</b></font>
                    <asp:GridView Width="515px" ID="Gv3" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                        <HeaderStyle CssClass="gvheader" Height="20px" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="RowNumber" HeaderText="Sr No." />
                            <asp:TemplateField HeaderText="NAME OF TEST">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtcustomer1" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TEST METHOD">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtcustomer2" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="REQUIREMENT/SCOPE OF TEST">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtcustomer3" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <FooterTemplate>
                                    <asp:Button ID="btnadd2" runat="server" Text="Add New Row" OnClick="btnadd2_click" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <br />
                <div style="background-color: Teal; width: 515px;">
                    <asp:CheckBox ID="chkcutting" ForeColor="White" runat="server" AutoPostBack="true"
                        CssClass="checkboxnormal" Text="CUTTING" OnCheckedChanged="chkcutting_CheckedChanged" /></div>
                <div visible="false" id="detailcutting" runat="server" style="width: 515px">
                    <font size="3px" color="teal"><b>CUTTING</b></font>
                    <asp:GridView Width="515px" ID="Gv4" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                        <HeaderStyle CssClass="gvheader" Height="20px" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="RowNumber" HeaderText="Sr No." />
                            <asp:TemplateField HeaderText="NAME OF THE PARAMETERS">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtcutting1" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SCOPE OF REQUIREMENT / TEST">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtcutting2" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <FooterTemplate>
                                    <asp:Button ID="btnadd3" runat="server" Text="Add New Row" OnClick="btnadd3_click" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%">
                        <tr>
                            <td>
                                <font size="2px" color="teal"><b>OTHER INFORMATION</b></font><br />
                                <asp:TextBox ID="txtotherinformation" Width="750px" Height="150px" TextMode="MultiLine"
                                    runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <div style="background-color: Teal; width: 515px;">
                    <asp:CheckBox ID="chkforweaving" ForeColor="White" runat="server" AutoPostBack="true"
                        CssClass="checkboxnormal" Text="WEAVING" OnCheckedChanged="chkforweaving_CheckedChanged" /></div>
                <div visible="false" id="detailweaving" runat="server">
                    <div>
                        <font size="3px" color="teal"><b>WEAVING</b></font>
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                    FINISH WEIGHT
                                </td>
                                <td style="width: 25%;">
                                    <asp:TextBox ID="txtfinishweight" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                                <td style="width: 13%;">
                                    ACTUAL WEIGHT
                                </td>
                                <td>
                                    <asp:TextBox ID="txtactualweight" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    REED
                                </td>
                                <td>
                                    <asp:TextBox ID="txtreed" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                                <td>
                                    FEEDING
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfeeding" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    PICK / LINE
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpick" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    <asp:Label ID="lblpick" ForeColor="Red" runat="server" Text="PER INCH/FT"></asp:Label>
                                </td>
                                <td>
                                    SHAFT
                                </td>
                                <td>
                                    <asp:TextBox ID="txtshaft" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    PUNCH CARD
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpunchcard" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    <asp:Label ID="Label1" ForeColor="Red" runat="server" Text="PER INCH/FT"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    PILE SCALE
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpilescale" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                                <td>
                                    PILE HEIGHT
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpileheight" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    SIZE TOLLERANCE
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsizetollerance" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    WEIGHT TOLLERANCE
                                </td>
                                <td>
                                    <asp:TextBox ID="txtweighttollerance" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 1%;">
                        <table width="50%">
                            <tr>
                                <td style="width: 40%;">
                                    <b>
                                        <asp:Label ID="Label2" runat="server" Text="LOSS"></asp:Label></b>
                                </td>
                                <td style="width: 40%;">
                                    <b>
                                        <asp:Label ID="lblwarp" runat="server" Text="WARP"></asp:Label></b>
                                </td>
                                <td>
                                    <b>
                                        <asp:Label ID="lblweft" runat="server" Text="WEFT"></asp:Label></b>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 560px">
                        <asp:CheckBox ID="ChkProcessLoss" Text="Check For Print" runat="server" CssClass="checkboxbold" />
                        <asp:GridView Width="515px" ID="Gv5" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                            <HeaderStyle CssClass="gvheader" />
                            <AlternatingRowStyle CssClass="gvalt" />
                            <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                            <Columns>
                                <asp:BoundField DataField="RowNumber" HeaderText="Sr No." />
                                <asp:TemplateField HeaderText="PROCESS NAME">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtprocessname" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="WARP">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtwarp" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="WEFT">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtweft" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SPECIFY % OR GRM">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtgrm" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <asp:Button ID="btnadd4" runat="server" Text="Add New Row" OnClick="btnadd4_click" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div runat="server">
                        <font size="2px" color="teal"><b>OTHER INFORMATION</b></font><br />
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtotherinfo" Width="750px" Height="150px" runat="server" CssClass="textb"
                                        TextMode="MultiLine" onkeydown="return (event.keyCode!=13);" Font-Names="Arial Unicode MS"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <br />
                <br />
                <div style="background-color: Teal; width: 515px;">
                    <asp:CheckBox ID="chkends" ForeColor="White" runat="server" AutoPostBack="true" CssClass="checkboxnormal"
                        Text="ENDS" OnCheckedChanged="chkends_CheckedChanged" /></div>
                <div visible="false" id="detailends" runat="server">
                    <div>
                        <font size="3px" color="teal"><b>ENDS</b></font>
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                    FRINGES
                                </td>
                                <td style="width: 25%;">
                                    <asp:TextBox ID="txtfringes" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                                <td style="width: 13%;">
                                    LENGTH
                                </td>
                                <td>
                                    <asp:TextBox ID="txtlength" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    FOLDED EDGES
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfoldededges" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                                <td>
                                    EDGES LENGTH/WIDTH
                                </td>
                                <td>
                                    <asp:TextBox ID="txtedges" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 400px;">
                        <table width="100%">
                            <tr>
                                <td style="width: 22%;">
                                    PLAIN
                                </td>
                                <td>
                                    <asp:TextBox ID="txtplain" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 400px;">
                        <table width="100%">
                            <tr>
                                <td style="width: 22%;">
                                    CORTRISE
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcortrise" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 400px;">
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                    BEEDS
                                </td>
                                <td style="width: 12%;">
                                    GROUND
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbeedsground" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 495px;">
                        <table>
                            <tr>
                                <td style="width: 40%;">
                                    BEEDS
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbeeds" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                    BINDING
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbinding" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                    POMS
                                </td>
                                <td style="width: 25%;">
                                    <asp:TextBox ID="txtpoms" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                                <td style="width: 14%;">
                                    SPECIFY NO(s) 1 SIDE
                                </td>
                                <td>
                                    <asp:TextBox ID="txtspecify1" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                    LACE
                                </td>
                                <td style="width: 25%;">
                                    <asp:TextBox ID="txtlace" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                                <td style="width: 14%;">
                                    SPECIFY NO(s) & SIDE
                                </td>
                                <td>
                                    <asp:TextBox ID="txtspecify2" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%;">
                                    MELT
                                </td>
                                <td style="width: 25%;">
                                    <asp:TextBox ID="txtmelt" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 495px;">
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                    2 X WIDTH
                                </td>
                                <td>
                                    <asp:TextBox ID="txtwidth" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 495px;">
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                    2 X LENGTH
                                </td>
                                <td>
                                    <asp:TextBox ID="txtlength1" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 495px;">
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                    ALL AROUND
                                </td>
                                <td>
                                    <asp:TextBox ID="txtallround" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <font size="2px" color="teal"><b>OTHER INFORMATION (IF ANY)</b></font><br />
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtotherforends" Width="750px" Height="150px" runat="server" CssClass="textb"
                                        TextMode="MultiLine" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <table style="margin-left: 350px;">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnsave" Width="70px" runat="server" Text="Save" CssClass="buttonnorm"
                                OnClick="btnsave_Click1" />
                            <asp:Button ID="btnnew" Width="70px" runat="server" Text="New" CssClass="buttonnorm"
                                OnClientClick="NewForm();" />
                            <asp:Button ID="btnpreview" Width="70px" runat="server" Text="Preview" CssClass="buttonnorm"
                                OnClick="btnpreview_Click" />
                            <asp:Button ID="btnclose" Width="70px" runat="server" Text="Close" CssClass="buttonnorm"
                                OnClientClick="CloseForm();" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
