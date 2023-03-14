<%@ Page Title="Process Raw Change" Language="C#" AutoEventWireup="true" CodeFile="FrmProcessRawChange.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_RawMaterial_FrmProcessRawChange" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmProcessRawChange.aspx";
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
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr id="Tr1" runat="server">
                        <td>
                            <asp:Label ID="lbl" Text="POrder No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPOrderNo" runat="server" Width="90px" AutoPostBack="True" CssClass="textb"
                                OnTextChanged="TxtPOrderNo_TextChanged"></asp:TextBox>
                        </td>
                        <td id="Td1">
                            <asp:Label ID="Label1" Text=" Company Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddCompName" runat="server" Width="200px" CssClass="dropdown">
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
                            &nbsp;&nbsp;
                            <asp:CheckBox ID="ChKForComplete" runat="server" Text="For Complete" CssClass="checkboxbold"
                                AutoPostBack="true" OnCheckedChanged="ChKForComplete_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="ddempname" runat="server" Width="200px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddempname_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="Td3">
                            <asp:Label ID="Label4" Text="  PO No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddOrderNo" runat="server" Width="130px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddOrderNo_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddCatagory" runat="server" Width="120px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table>
                    <tr id="Tr3" runat="server">
                        <td runat="server" id="procode">
                            <asp:Label ID="Label8" Text="ProdCode" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtProdCode" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
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
                            <asp:DropDownList ID="ddshape" runat="server" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="sz" runat="server" visible="false">
                            <asp:Label ID="LblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
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
                <table width="100%">
                    <tr>
                        <td style="width: 50%">
                            <div style="height: 300px; overflow: auto;">
                                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                    DataKeyNames="Item_Finished_ID">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="Label21" Text='<%#Bind("Description") %>' Width="400px" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ConsmpQty">
                                            <ItemTemplate>
                                                <asp:Label ID="Label22" Text='<%#Bind("ConsmpQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtConsmpQty" runat="server" align="right" Width="75px" Text='<%# Bind("ConsmpQty") %>'
                                                    onkeypress="return isNumber(event);"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <table width="100%">
                <tr id="Tr7">
                    <td style="width: 45%; text-align: right;">
                        <asp:Label ID="LblError" runat="server" Text="Label" CssClass="labelbold" ForeColor="Red"
                            Font-Size="Small" Visible="false"></asp:Label>
                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return confirm('Do You Want To Save?')"
                            CssClass="buttonnorm" />
                        <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" />
                        <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                            OnClick="btnpreview_Click" Visible="false" />
                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            CssClass="buttonnorm" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
