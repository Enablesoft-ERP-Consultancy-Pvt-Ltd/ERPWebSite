<%@ Page Title="ProcessIssue" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="ProcessIssueNew.aspx.cs" Inherits="Masters_Process_ProcessIssueNew"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"> </script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script type="text/javascript">

        function SaveData() {

            var id = document.getElementById('<%=hncomp.ClientID %>').value;

            if (id != "20") {
                var answer = confirm("Do you want to Save?")

                if ((answer)) {
                    return true;
                }
                else {
                    return false;
                }

            }
            return true;
        }


        function UpdateData() {

            var id = document.getElementById('CPH_Form_hncomp').value;
            if (id != "20") {
                alert('Do you want to Update?');
                return false;
            }
        }      
    
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#CPH_Form_chkforsms").click(function () {
                $("#CPH_Form_BtnSendSms").toggle();

            });
        });
        function isnumber(event, pointflag) {
            var keycode = event.which;
            if ((keycode > 47 && keycode <= 58) || (keycode == 46 && pointflag == true)) {
                return true;
            }
            else {
                return false;
            }
        }       
     
    </script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "ProcessIssueNew.aspx";
        }
        function Preview() {
            window.open('../../reportViewer1.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate_RequiredDate() {
            var required_date = document.getElementById('TxtRequiredDate').Value;
            var assign_date = document.getElementById('TxtAssignDate').value;
            if (assign_date < required_date) {
                alert("Required Date must Be greater than assign Date");
            }
        }
        function AddLoomDetail() {
            var answer = confirm("Do you want to ADD?")

            if (answer) {

                var a = document.getElementById('CPH_Form_hnIssueOrderId').value;
                var b = document.getElementById('CPH_Form_DDProcessName');
                var processId = b.options[b.selectedIndex].value;

                if (a == "" || a == "0") {
                    alert('Plz fill Order Detail first');
                    return false;
                }
                var left = (screen.width / 2) - (650 / 2);
                var top = (screen.height / 2) - (300 / 2);

                //window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
                window.open('frmWeaverLoomDetail.aspx?a=' + a + '&b=' + processId + '', 'Loom Detail', 'width=650px, height=400px, top=' + top + ', left=' + left);
            }
        }

        function AddEmployee() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (1000 / 2);
            var top = (screen.height / 2) - (800 / 2);

            if (answer) {
                window.open('../Campany/AddFrmWeaver.aspx', '', 'width=1200px,Height=801px,top=' + top + ',left=' + left, 'resizeable=yes');
            }
        }        
        
    </script>
    <div id="maindiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblLocalOrderNo" Text="SR No." runat="server" CssClass="labelbold"
                                Visible="false" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lbl" Text=" Company Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle" id="tdLProcessName" runat="server" visible="false">
                            <asp:Label ID="Label1" Text="Process Name " runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" Text="Vendor Name " runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label3" Text="Customer Code" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" Text="Customer Order No." runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" Text=" Challan No" runat="server" CssClass="labelbold" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtlocalorerNo" runat="server" Width="80px" CssClass="textb" Visible="false"
                                OnTextChanged="txtlocalorerNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="tdProcessName" runat="server" visible="false">
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDEmployeeName" Width="150px" runat="server"
                                OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:Button ID="refreshEmployee" runat="server" Style="display: none" OnClick="refreshEmployee_Click" />
                            <asp:Button ID="btnaddEmployee" runat="server" CssClass="buttonsmall" OnClientClick="return AddEmployee();"
                                Text="&#43;" />
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerOrderNumber" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerOrderNumber_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtChallanNo" runat="server" Width="90px" CssClass="textb" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="tdstyle">
                            Assign Date
                        </td>
                        <td class="tdstyle">
                            Required Date
                        </td>
                        <td class="tdstyle">
                            Unit
                        </td>
                        <td class="tdstyle">
                            Cal Type
                        </td>
                    </tr>--%>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" Text=" Assign Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtAssignDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtAssignDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label7" Text=" Required Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRequiredDate" runat="server" OnTextChanged="TxtRequiredDate_TextChanged"
                                AutoPostBack="true" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtRequiredDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label8" Text="Unit" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDunit" runat="server" Width="100px" AutoPostBack="True"
                                OnSelectedIndexChanged="DDunit_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label9" Text=" Cal Type" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px">
                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                <asp:ListItem Value="4">L-2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="tdChkforFix" runat="server" visible="false">
                            <br />
                            <b>
                                <asp:CheckBox ID="ChkForFix" class="tdstyle" runat="server" Text="Chk For Fix" />
                        </td>
                        <td>
                            &nbsp;<asp:CheckBox ID="chkboxRoundFullArea" runat="server" />
                            &nbsp;<asp:Label ID="Label38" runat="server" Text="Check For Full Area When Round"
                                CssClass="labelbold"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="TDLblProdCode" runat="server">
                            <asp:Label ID="LblProdCode" class="tdstyle" runat="server" Text="Product Code" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td id="tdLCategoryName" runat="server" visible="false">
                            <asp:Label ID="lblcategoryname" class="tdstyle" runat="server" Text="Category Name"
                                CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label10" Text="  Description" runat="server" CssClass="labelbold" />
                        </td>
                    </tr>
                    <tr>
                        <td id="TDProductCode" runat="server">
                            <asp:TextBox ID="TxtProductCode" CssClass="textb" runat="server" AutoPostBack="True"
                                Width="110px" OnTextChanged="TxtProductCode_TextChanged"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProductCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td id="tdCategoryName" runat="server" visible="false">
                            <asp:DropDownList CssClass="dropdown" ID="DDCategoryName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList CssClass="dropdown" ID="DDDescription" runat="server" Width="500px"
                                AutoPostBack="True" OnSelectedIndexChanged="DDDescription_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="tdChkPPApproval" runat="server" visible="false">
                            <b>
                                <asp:CheckBox ID="ChkPPApproval" runat="server" Text=" PP_Approval" /></b>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <%--<td id="LblArea" runat="server" visible="false">--%>
                        <td class="tdstyle">
                            <asp:Label ID="Label11" Text=" T.O.Qty" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label12" Text="PQty" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label19" Text="IssueQty" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label13" Text=" Width" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label14" Text="  Length" runat="server" CssClass="labelbold" />
                        </td>
                        <td id="LblArea" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="Label15" Text=" Area" runat="server" CssClass="labelbold" />
                        </td>
                        <td id="lblWeight" runat="server" class="tdstyle" visible="false">
                            <asp:Label ID="Label16" Text=" Weight/Pc." runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label17" Text="  Rate" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label18" Text="  Comm" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label22" Text="Khap" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label23" Text="Consump" runat="server" CssClass="labelbold" />
                        </td>
                    </tr>
                    <tr>
                        <td width="90px">
                            <asp:TextBox ID="TxtTotalQty" runat="server" Width="90px" Enabled="false" CssClass="textb1"></asp:TextBox>
                        </td>
                        <td width="90px" id="tdPreQty" runat="server" visible="true">
                            <asp:TextBox ID="TxtPreQuantity" runat="server" Width="90px" Enabled="false" CssClass="textb1"></asp:TextBox>
                        </td>
                        <td width="90px">
                            <asp:TextBox ID="TxtQtyRequired" runat="server" Width="90px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtQtyRequired_TextChanged"></asp:TextBox>
                        </td>
                        <td width="90px">
                            <asp:TextBox ID="TxtWidth" runat="server" Width="90px" AutoPostBack="True" CssClass="textb1"
                                OnTextChanged="TxtWidth_TextChanged" Enabled="false"></asp:TextBox>
                        </td>
                        <td width="90px">
                            <asp:TextBox ID="TxtLength" runat="server" Width="90px" AutoPostBack="True" CssClass="textb1"
                                OnTextChanged="TxtLength_TextChanged" Enabled="false"></asp:TextBox>
                        </td>
                        <td id="TdArea" runat="server" visible="false" width="90px">
                            <asp:TextBox ID="TxtArea" runat="server" Width="90px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>
                        <td id="TdtxtWeight" runat="server" width="90px" visible="false">
                            <asp:TextBox ID="txtWeight" runat="server" Width="90px" CssClass="textb1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRate" runat="server" Width="90px" AutoPostBack="True" CssClass="textb1"
                                OnTextChanged="TxtRate_TextChanged" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtCommission" runat="server" Width="90px" CssClass="textb1" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtKhap" runat="server" Width="90px" CssClass="textb1" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtConsump" runat="server" Width="90px" CssClass="textb1" Enabled="false"></asp:TextBox>
                        </td>
                        <td id="tdAfterKhapSize" runat="server" visible="false">
                            <asp:TextBox ID="txtAfterKhapSizeOrder" runat="server" Width="90px" CssClass="textb1"
                                Enabled="false" Visible="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <b>
                                <asp:CheckBox ID="chkforsms" Visible="false" AutoPostBack="true" Text="Check For SMS"
                                    runat="server" class="tdstyle" OnCheckedChanged="chkforsms_CheckedChanged" /></b>
                            <asp:Button ID="BtnSendSms" Visible="false" runat="server" Text="Send Sms" CssClass="buttonnorm"
                                OnClick="BtnSendSms_Click" />
                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" UseSubmitBehavior="false"
                                OnClientClick="if (!SaveData()) return; this.disabled=true;this.value = 'wait ...';"
                                CssClass="buttonnorm" />
                            <%-- OnClientClick="SaveData();"--%>
                            <%-- OnClientClick="if (!confirm('Do you want to save Date?')) return; this.disabled=true;this.value = 'wait ...';"--%>
                            <asp:Button ID="btnLoomDetail" runat="server" Visible="false" Text="Add Loom" CssClass="buttonnorm"
                                OnClientClick="return AddLoomDetail();" />
                            <asp:Button ID="BtnNew" runat="Server" Text="New" OnClientClick="return reloadPage();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="BtnUpdate" runat="server" Text="Update" OnClick="BtnUpdate_Click"
                                Visible="False" OnClientClick="UpdateData();" CssClass="buttonnorm" />
                            <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                Visible="true" CssClass="buttonnorm preview_width" />
                            <b>
                                <asp:CheckBox ID="chkforsummary" Visible="false" AutoPostBack="true" Text="Check For Summary"
                                    runat="server" class="tdstyle" /></b> &nbsp;<asp:Button ID="BtnClose" runat="server"
                                        Text="Close" OnClientClick="return CloseForm();" CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="true" ForeColor="RED"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="7">
                            <div style="width: 100%; max-height: 250px; overflow: auto">
                                <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="Sr_No"
                                    OnRowDataBound="DGOrderdetail_RowDataBound" OnRowDeleting="DGOrderdetail_RowDeleting"
                                    ShowFooter="true" CssClass="grid-views" OnRowCancelingEdit="DGOrderdetail_RowCancelingEdit"
                                    OnRowEditing="DGOrderdetail_RowEditing" OnRowUpdating="DGOrderdetail_RowUpdating">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <FooterStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" />
                                                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <%--   <asp:BoundField DataField="Category" HeaderText="Category">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="125px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("Category") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField DataField="Item" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Item">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" Text='<%#Bind("Item") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField DataField="Description" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>--%>
                                        <%-- <asp:BoundField DataField="Quality" HeaderText="Quality">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Quality">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("Quality") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField DataField="Design" HeaderText="Design">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Design">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("Design") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField DataField="Color" HeaderText="Color">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Color">
                                            <ItemTemplate>
                                                <asp:Label ID="lblColor" runat="server" Text='<%#Bind("Color") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Shape" HeaderText="Shape">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Shape">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShape" runat="server" Text='<%#Bind("Shape") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%--  <asp:BoundField DataField="Size" HeaderText="Size">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Length" HeaderText="Length">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Length">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLength" runat="server" Text='<%#Bind("Length") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField DataField="Width" HeaderText="Width">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Width">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWidth" runat="server" Text='<%#Bind("Width") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField DataField="Khap" HeaderText="Khap">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Khap">
                                            <ItemTemplate>
                                                <asp:Label ID="lblKhap" runat="server" Text='<%#Bind("Khap") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Consump" HeaderText="Consump">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Consump">
                                            <ItemTemplate>
                                                <asp:Label ID="lblConsump" runat="server" Text='<%#Bind("Consump") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Area">
                                          <ItemTemplate>
                                           <asp:Label ID="lblArea" runat="server" Text='<%#Eval("Area") %>' />
                                          </ItemTemplate>
                                          <ItemStyle HorizontalAlign="Left" Width="80px" />
                                          <FooterTemplate>
                                           <asp:Label ID="lblTotalArea" runat="server" Text="Total Area" />
                                          </FooterTemplate>
                                          </asp:TemplateField>--%>
                                        <%-- <asp:BoundField DataField="Area" HeaderText="Area">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />                                            
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Area">
                                            <ItemTemplate>
                                                <asp:Label ID="lblArea" runat="server" Text='<%#Bind("Area") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>--%>
                                        <%-- <asp:BoundField DataField="Rate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%#Bind("Rate") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Comm" HeaderText="Comm">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Comm">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComm" runat="server" Text='<%#Bind("Comm") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" Text='<%#Bind("Qty") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <EditItemTemplate>
                                                <asp:Label ID="lblQtyEdit" runat="server" Text='<%#Bind("Qty") %>' Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtReqQtyEdit" runat="server" Text='<%#Eval("Qty") %>' Width="40px"
                                                    onkeypress="return isNumberKey(event);" onFocus="this.select()"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTQty" runat="server" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TArea">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTArea" runat="server" Text='<%#Bind("TArea") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTArea" runat="server" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WAmount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWAmount" runat="server" Text='<%#Bind("Amount") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandWAmount" runat="server" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CommAmt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCommAmt" runat="server" Text='<%#Bind("CommAmt") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandCommAmt" runat="server" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GTotal">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGTotal" runat="server" Text='<%#Bind("GTotal") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandGTotal" runat="server" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--  <asp:BoundField DataField="TArea" HeaderText="TArea">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Amount" HeaderText="WAmount">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CommAmt" HeaderText="CommAmt">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="GTotal" HeaderText="GTotal">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemFinishedId" Text='<%#Bind("Item_Finished_id") %>' runat="server" />
                                                <asp:Label ID="lblQualityid" Text='<%#Bind("Qualityid") %>' runat="server" />
                                                <asp:Label ID="lblDesignid" Text='<%#Bind("Designid") %>' runat="server" />
                                                <asp:Label ID="lblIssueDetailId" Text='<%#Bind("Sr_No") %>' runat="server" />
                                                <asp:Label ID="lblCancelQty" Text='<%#Bind("CancelQty") %>' runat="server" />
                                                <asp:Label ID="hnlblQty" runat="server" Text='<%#Bind("Qty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label24" Text="HSCode" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtHSCode" runat="server" Width="200px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label21" Text="Remarks" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRemarks" runat="server" Width="500px" TextMode="MultiLine" Height="30px"
                                CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label20" Text=" Instructions" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtInstructions" runat="server" Width="800px" Height="50px" CssClass="textb"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <asp:HiddenField ID="hncomp" runat="server" />
                        <asp:HiddenField ID="hnIssueOrderId" runat="server" />
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
