<%@ Page Title="Home Furnishing Department Raw Issue" Language="C#" AutoEventWireup="true" CodeFile="FrmHomeFurnishingDepartmentRowIssue.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_process_FrmHomeFurnishingDepartmentRowIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmHomeFurnishingDepartmentRowIssue.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
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
        function validate() {
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDBranchName.ClientID %>").value <= "0") {
                alert("Pls Select branch Name");
                document.getElementById("<%=DDBranchName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddProcessName.ClientID %>").value <= "0") {
                alert("Pls Select process Name");
                document.getElementById("<%=ddProcessName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDDepartmentName.ClientID %>").value <= "0") {
                alert("Pls Select department No.");
                document.getElementById("<%=DDDepartmentName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddOrderNo.ClientID %>").value <= "0") {
                alert("Pls Select Order No.");
                document.getElementById("<%=ddOrderNo.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtdate.ClientID %>").value == "") {
                alert("Please select date");
                document.getElementById("<%=txtdate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_ddCatagory').value <= "0") {
                alert("Please Select Category Name....!");
                document.getElementById("CPH_Form_ddCatagory").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_dditemname').value <= "0") {
                alert("Please Select Item Name....!");
                document.getElementById("CPH_Form_dditemname").focus();
                return false;
            }
            if (document.getElementById("<%=ql.ClientID %>")) {
                if (document.getElementById('CPH_Form_dquality').value <= "0") {
                    alert("Please Select Quality Name....!");
                    document.getElementById("CPH_Form_dquality").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=dsn.ClientID %>")) {
                if (document.getElementById('CPH_Form_dddesign').value <= "0") {
                    alert("Please Select design Name....!");
                    document.getElementById("CPH_Form_dddesign").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=clr.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddcolor').value <= "0") {
                    alert("Please Select color Name....!");
                    document.getElementById("CPH_Form_ddcolor").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=shp.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddshape').value <= "0") {
                    alert("Please Select shape Name....!");
                    document.getElementById("CPH_Form_ddshape").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=sz.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddsize').value <= "0") {
                    alert("Please Select size Name....!");
                    document.getElementById("CPH_Form_ddsize").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=shd.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddlshade').value <= "0") {
                    alert("Please Select shadecolor Name....!");
                    document.getElementById("CPH_Form_ddlshade").focus();
                    return false;
                }
            }

            if (document.getElementById('CPH_Form_ddlunit').value <= "0") {
                alert("Please Select Unit....!");
                document.getElementById("CPH_Form_ddlunit").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_ddgodown').value <= "0") {
                alert("Please Select Godown Name....!");
                document.getElementById("CPH_Form_ddgodown").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_ddlotno').value <= "0") {
                alert("Please Select Lot No....!");
                document.getElementById("CPH_Form_ddlotno").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDTagno').value <= "0") {
                alert("Please Select tag no....!");
                document.getElementById("CPH_Form_DDTagno").focus();
                return false;
            }
            if (document.getElementById("<%=TDBinNo.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDBinNo').value <= "0") {
                    alert("Please Select Bin no....!");
                    document.getElementById("CPH_Form_DDBinNo").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtissue.ClientID %>").value == "0" || document.getElementById("<%=txtissue.ClientID %>").value == "") {
                alert("Qty can not be blank or zero");
                document.getElementById("<%=txtissue.ClientID %>").focus();
                return false;
            }

            else {
                return confirm('Do You Want To Save?')
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label Text="Master Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table>
                        <tr id="Tr1" runat="server">
                            <td>
                                <asp:Label ID="lbl" Text="POrder No" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtPOrderNo" runat="server" Width="80px" AutoPostBack="True" CssClass="textb"
                                    OnTextChanged="TxtPOrderNo_TextChanged"></asp:TextBox>
                            </td>
                            <td id="Td1">
                                <asp:Label ID="Label1" Text=" Company Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddCompName" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label36" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td id="Td2">
                                <asp:Label ID="Label2" Text="  Process Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddProcessName" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td4">
                                <asp:Label ID="Label3" Text=" Department" runat="server" CssClass="labelbold" />
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="ChKForEdit" runat="server" Text="For Edit" CssClass="checkboxbold"
                                    AutoPostBack="true" OnCheckedChanged="ChKForEdit_CheckedChanged" />
                                <br />
                                <asp:DropDownList ID="DDDepartmentName" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDDepartmentName_SelectedIndexChanged" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td3">
                                <asp:Label ID="Label4" Text="  PO No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddOrderNo" runat="server" Width="130px" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddOrderNo_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td id="Td7" runat="server" visible="false">
                                <asp:Label ID="Label5" Text="  Challan No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDChallanNo" runat="server" Width="130px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Td5">
                                <asp:Label ID="Label6" Text="   Issue Date" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtdate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>
                            </td>
                            <td id="Td6">
                                <asp:Label ID="Label7" Text="  Challan No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtchalanno" Width="100px" runat="server" OnTextChanged="txtchalan_ontextchange"
                                    AutoPostBack="True" CssClass="textb"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label Text="Item Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table>
                        <tr id="Tr3" runat="server">
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" AutoPostBack="True"
                                    CssClass="dropdown" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dditemname" runat="server" Width="150px" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                                    AutoPostBack="True" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="ql" runat="server" visible="false">
                                <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dquality" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="dsn" runat="server" visible="false">
                                <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="clr" runat="server" visible="false">
                                <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="shp" runat="server" visible="false">
                                <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                    CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="sz" runat="server" visible="false">
                                <asp:Label ID="LblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <asp:CheckBox ID="ChkForMtr" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForMtr_CheckedChanged" />
                                <asp:Label ID="Label9" Text=" For Mtr" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="shd" runat="server" visible="false">
                                <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <fieldset>
                    <legend></legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" Text="   Unit" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddlunit" runat="server" Width="75px" AutoPostBack="true" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label12" Text="  Godown Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddgodown" runat="server" Width="150px" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddgodown_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label13" Text="   Lot No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddlotno" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlotno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDTagno" runat="server" visible="false">
                                <asp:Label ID="Label15" Text="  Tag No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDTagno" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDTagno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBinNo" runat="server" visible="false">
                                <asp:Label ID="Label19" Text="  Bin No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDBinNo" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Tdconetype" runat="server">
                                <asp:Label Text="Cone Type" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDconetype" CssClass="dropdown" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label32" Text="No. Of Cone" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtnoofcone" CssClass="textb" Width="75px" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label14" Text="  Stock" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtstock" runat="server" Width="75px" ReadOnly="true" CssClass="textb"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td runat="server" visible="false">
                                <asp:TextBox ID="txtconqty" runat="server" Width="0px" Height="0px" ReadOnly="true"
                                    CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label16" Text="  Pend Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtPendQty" runat="server" Width="75px" ReadOnly="true" CssClass="textb"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label17" Text="Issue Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtissue" runat="server" Width="75px" OnTextChanged="txtissue_TextChanged"
                                    CssClass="textb" AutoPostBack="True" onkeypress="return isNumberKey(event);"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="clear: both">
            </div>
            <table width="100%">
                <tr id="Tr7">
                    <td style="width: 55%">
                        <asp:TextBox ID="TxtProdCode" runat="server" Width="0px" Height="0px" CssClass="labelbold"></asp:TextBox>
                        <asp:Label Text="Remark" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtremark" CssClass="textb" Width="100%" runat="server" TextMode="MultiLine" />
                    </td>
                    <td style="width: 45%; text-align: right;">
                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click"  OnClientClick="return validate()"
                            CssClass="buttonnorm" />
                        <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" />
                        <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                            OnClick="btnpreview_Click" />
                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                            CssClass="buttonnorm" OnClick="btnclose_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="LblError" runat="server" Text="Label" CssClass="labelbold" ForeColor="Red"
                            Font-Size="Small" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 80%" valign="top">
                        <div style="max-height: 300px; overflow: auto;">
                            <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                DataKeyNames="prtid" OnRowDeleting="gvdetail_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Catagory">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcatgrid" Text='<%#Bind("CATEGORY_NAME") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Label20" Text='<%#Bind("Item_name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="Label21" Text='<%#Bind("Description") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="Label22" Text='<%#Bind("Qty") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lot No.">
                                        <ItemTemplate>
                                            <asp:Label ID="Label23" Text='<%#Bind("Lotno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag No.">
                                        <ItemTemplate>
                                            <asp:Label ID="Label24" Text='<%#Bind("Tagno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bin No.">
                                        <ItemTemplate>
                                            <asp:Label ID="Label25" Text='<%#Bind("BinNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Godown Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Label26" Text='<%#Bind("godownname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="DEL" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do You Want To Delete?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
