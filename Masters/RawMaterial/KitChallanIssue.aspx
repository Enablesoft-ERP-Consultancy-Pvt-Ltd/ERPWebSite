<%@ Page Title="Kit Challan Issue" Language="C#" AutoEventWireup="true" CodeFile="KitChallanIssue.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_RawMaterial_KitChallanIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "KitChallanIssue.aspx";
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
        function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnStockNo.ClientID %>').click();
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
                        <tr id="TRempcodescan" runat="server" visible="false">
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="Label18" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                    Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                            </td>
                            <td>
                                <asp:Label ID="LblCustomerCodeAndOrderNo" Text="" runat="server" CssClass="labelbold"
                                    ForeColor="Red" />
                            </td>
                            <td>
                                <td id="TDForCompleteStatus" runat="server" visible="false">
                                    <asp:CheckBox ID="ChkForCompleteStatus" Text="For Complete Order" CssClass="labelbold"
                                        runat="server" AutoPostBack="true" OnCheckedChanged="ChkForCompleteStatus_CheckedChanged" />
                                </td>
                            </td>
                        </tr>
                        <tr id="Tr1" runat="server">
                            <td>
                                <asp:Label ID="lbl" Text="Folio No" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtPOrderNo" runat="server" Width="90px" AutoPostBack="True" CssClass="textb"
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
                                <asp:Label ID="Label3" Text=" Party Name" runat="server" CssClass="labelbold" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="ChKForEdit" runat="server" Text=" Check For Edit" CssClass="checkboxbold"
                                    AutoPostBack="true" OnCheckedChanged="ChKForEdit_CheckedChanged" />
                                <br />
                                <asp:DropDownList ID="ddempname" runat="server" Width="230px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddempname_SelectedIndexChanged" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td3">
                                <asp:Label ID="Label4" Text="  Folio No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddOrderNo" runat="server" Width="130px" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddOrderNo_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td id="TdDDItemDesignName" runat="server" visible="false">
                                <asp:Label ID="Label35" Text="Item Design" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDItemDesignName" runat="server" Width="130px" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDItemDesignName_SelectedIndexChanged" AutoPostBack="True">
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
                    <%--<table>
                        <tr id="Tr3" runat="server">
                            <td runat="server" id="procode">
                                <asp:Label ID="Label8" Text="ProdCode" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtProdCode" runat="server" OnTextChanged="TxtProdCode_TextChanged"
                                    AutoPostBack="True" Width="100px" CssClass="textb"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                    UseContextKey="True">
                                </cc1:AutoCompleteExtender>
                            </td>
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
                                <asp:Label ID="Label9" Text="  Check for Mtr" runat="server" CssClass="labelbold" />
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
                        <tr>
                            <td id="TDEstimatedRate" runat="server" visible="false">
                                <asp:Label ID="Label31" runat="server" Text="Estimated Rate" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:TextBox ID="txtEstimatedRate" Width="100px" runat="server" CssClass="textb"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td id="TDTanaBana" runat="server" visible="false">
                                <asp:Label ID="Label27" runat="server" Text="Tana/Bana" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:TextBox ID="txtTanaBana" Width="100px" runat="server" CssClass="textb"></asp:TextBox>
                            </td>
                            <td id="TDTransportName" runat="server" visible="false">
                                <asp:Label ID="Label28" runat="server" Text="Transport Name" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:TextBox ID="txtTransportName" Width="100px" runat="server" CssClass="textb"></asp:TextBox>
                            </td>
                            <td id="TDBiltyNo" runat="server" visible="false">
                                <asp:Label ID="Label29" runat="server" Text="Bilty No" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:TextBox ID="txtBiltyNo" Width="100px" runat="server" CssClass="textb"></asp:TextBox>
                            </td>
                            <td id="TDVehicleNo" runat="server" visible="false">
                                <asp:Label ID="Label30" runat="server" Text="Vehicle No" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:TextBox ID="txtVehicleNo" Width="100px" runat="server" CssClass="textb"></asp:TextBox>
                            </td>
                            <td id="TDCGSTSGST" runat="server" visible="false">
                                <asp:Label ID="Label34" runat="server" Text="CGST/SGST" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:TextBox ID="txtCGSTSGST" Width="100px" runat="server" CssClass="textb"></asp:TextBox>
                            </td>
                             <td id="TDEWayBillNo" runat="server" visible="false">
                                <asp:Label ID="Label37" runat="server" Text="EWayBillNo" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:TextBox ID="txtEWayBillNo" Width="120px" runat="server" CssClass="textb"></asp:TextBox>
                            </td>
                        </tr>
                    </table>--%>
                </fieldset>
            </div>
           <%-- <div>
                <fieldset>
                    <legend></legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" Text="   Unit" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddlunit" runat="server" Width="100px" AutoPostBack="true" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" Text="  Process" runat="server" CssClass="labelbold" />
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
                                <asp:TextBox ID="txtnoofcone" CssClass="textb" Width="70px" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label14" Text="  Stock" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtstock" runat="server" Width="80px" ReadOnly="true" CssClass="textb"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td runat="server" visible="false">
                                <asp:TextBox ID="txtconqty" runat="server" Width="80px" ReadOnly="true" CssClass="textb"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label16" Text="  Pend Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtPendQty" runat="server" Width="70px" ReadOnly="true" CssClass="textb"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label17" Text="Issue Quantity" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtissue" runat="server" Width="90px" OnTextChanged="txtissue_TextChanged"
                                    CssClass="textb" AutoPostBack="True" onkeypress="return isNumberKey(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label33" Text="BellWt" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtBellWt" runat="server" Width="90px" CssClass="textb" onkeypress="return isNumberKey(event);"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>--%>
            <div style="clear: both">
            </div>
            <table width="100%">
                <tr id="Tr7">
                    <td style="width: 35%">
                        <asp:Label Text="Enter Kit No./Scan" runat="server" CssClass="labelbold" />
                        <br />
                       <%-- <asp:TextBox ID="txtremark" CssClass="textb" Width="100%" runat="server" TextMode="MultiLine"
                            Height="51px" />--%>
                              <asp:TextBox ID="txtkitno" CssClass="textb" Height="40px" Width="250px" runat="server"
                                    onKeypress="KeyDownHandler(event);" />
                                     <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="txtkitno_TextChanged" />
                                     <strong> ARTICLE : </strong><asp:Label ID="lblkitarticle" runat="server"></asp:Label>
                  

                    <strong>SIZE : </strong><asp:Label ID="lblkitsize" runat="server"></asp:Label>
                  
                   <strong> PCS : </strong><asp:Label ID="lblkitpcs" runat="server"></asp:Label>

                    </td>
                   <%-- <td style="width: 35%;text-align: left;">
                   
                    </td>--%>
                    <td style="width: 30%; text-align: right;">
                      <%--  &nbsp;<asp:CheckBox ID="ChkWayChallanFormat" runat="server" Text="Print WayChallan Format"
                            CssClass="checkboxbold" Visible="false" />--%>
                       <%-- &nbsp;<asp:CheckBox ID="ChkWayChallanFormatBack" runat="server" Text="Print WayChallan FormatBack"
                            CssClass="checkboxbold" Visible="false" />--%>
                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return confirm('Do You Want To Save?')"
                            CssClass="buttonnorm" />
                        <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" />
                       <%-- <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                            OnClick="btnpreview_Click" />--%>
                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                            CssClass="buttonnorm" OnClick="btnclose_Click" />
                       <%-- <asp:Button ID="BtnUpdateRemark" runat="server" Text="Update Remark" OnClick="BtnUpdateRemark_Click"
                            OnClientClick="return confirm('Do You Want To Save?')" CssClass="buttonnorm" />
                        <asp:Button ID="BtnPanipatPNM2" runat="server" Text="WEAVING CENTRE 2" CssClass="buttonnorm"
                            Visible="false" OnClick="BtnOrderProcessToChampoPanipatPNM2_Click" />--%>
                    </td>
                </tr>
                <tr>
                <td style="width: 60%" id="TDtxtremarks" runat="server">
                            <asp:Label ID="Label16" Text="Remark" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtremarks" runat="server" Width="90%" CssClass="textb" TextMode="MultiLine"
                                Height="50px"></asp:TextBox>
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
                    <td style="width: 50%">
                        <div style="height: 300px; overflow: auto;">
                        <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                DataKeyNames="prtid" OnSelectedIndexChanged="gvdetail_SelectedIndexChanged" OnRowEditing="gvdetail_RowEditing" OnRowUpdating="gvdetail_RowUpdating" OnRowDeleting="gvdetail_RowDeleting">
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
                                    <%--<asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="Label22" Text='<%#Bind("Qty") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Qty">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtqty" Text='<%#Bind("Qty") %>' Width="70px" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblqty" Text='<%#Bind("Qty") %>' runat="server" />
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
                                       <asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                                </Columns>
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>

                            <%--<asp:GridView ID="GDGridShow" runat="server" AutoGenerateColumns="False">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ConsmpQTY" HeaderText="Consmp Qty">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IssQty" HeaderText="Iss Qty">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PendQty" HeaderText="Pend Qty">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>--%>
                        </div>
                    </td>
                    <td style="width: 50%" valign="top">
                        <div style="max-height: 300px; overflow: auto;">
                            <asp:GridView ID="gvkit" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvkit_RowDataBound"
                                DataKeyNames="prtid" >
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="Catagory">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcatgrid" Text='<%#Bind("CATEGORY_NAME") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Label20" Text='<%#Bind("Item_name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesc" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblissueqty" Text='<%#Bind("IssueQuantity") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lot No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbllotno" Text='<%#Bind("Lotno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltagno" Text='<%#Bind("Tagno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <%-- <asp:TemplateField HeaderText="Bin No.">
                                        <ItemTemplate>
                                            <asp:Label ID="Label25" Text='<%#Bind("BinNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                   <%-- <asp:TemplateField HeaderText="Godown Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Label26" Text='<%#Bind("godownname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:TemplateField HeaderText="DEL" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do You Want To Delete?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                     <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblitemid" Text='<%#Bind("itemid") %>' runat="server" />
                                      <asp:Label ID="lblprmid" Text='<%#Bind("prmid") %>' runat="server" />
                                     <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                    <asp:Label ID="lblfinishedid" Text='<%#Bind("Finishedid") %>' runat="server" />
                                    <asp:Label ID="lblgodownid" Text='<%#Bind("godownid") %>' runat="server" />
                                    <%--<asp:Label ID="lblprorderid" Text='<%#Bind("prorderid") %>' runat="server" />--%>
                                    <asp:Label ID="lblprocessid" Text='<%#Bind("processid") %>' runat="server" />
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
            <asp:HiddenField ID="hnissampleorder" runat="server" Value="0" />
            <asp:HiddenField ID="hdnfinishedid" runat="server" Value="0" />
            <asp:HiddenField ID="hdnchallanfinishedid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
