<%@ Page Title="ORDER WISE DYEING CONSUMPTION" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmOrderWiseDyeingConsumption.aspx.cs" Inherits="Masters_ReportForms_frmOrderWiseDyeingConsumption" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">

        function Validate() {
            if (document.getElementById("<%=DDCompany.ClientID %>").value < "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=DDCompany.ClientID %>").focus();
                return false;
            }
            else {
                var isValid = false;
                var gridView = document.getElementById('<%= DGOrderDetail.ClientID %>');
                for (var i = 1; i < gridView.rows.length; i++) {
                    var inputs = gridView.rows[i].getElementsByTagName('input');
                    if (inputs != null) {
                        if (inputs[0].type == "checkbox") {
                            if (inputs[0].checked) {
                                isValid = true;
                                return confirm('Please Click Ok For Report Open')
                            }
                        }
                    }
                }
                alert("Please select atleast one checkbox");
                return false;
            }
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
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td style="width: 50%" valign="top">
                        <table style="width: 100%" border="1" cellpadding="0" cellspacing="5">
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="lblordertype" Text="Company Name" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="276px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label4" Text="Buyer" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 70%">
                                    <asp:DropDownList CssClass="dropdown" ID="DDbuyer" runat="server" Width="90%" AutoPostBack="True"
                                        OnSelectedIndexChanged="DDbuyer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                            <tr align="center">
                                <td align="right" colspan="2">
                                    <asp:Button CssClass="buttonnorm" ID="BtnPreview" Text="Preview" runat="server"
                                        OnClick="BtnPreview_Click" OnClientClick="return Validate();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%" valign="top">
                        <table style="width: 100%">
                            <tr id="trdrig" runat="server">
                                <td style="width: 100%">
                                    <div style="width: 100%; max-height: 400px; overflow: auto">
                                        <asp:GridView ID="DGOrderDetail" runat="server" DataKeyNames="orderid" AutoGenerateColumns="False"
                                            EmptyDataText="No records fetched..." CssClass="grid-views" Width="100%">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chkbox" runat="server" />
                                                        <%-- <asp:CheckBox ID="CheckBox1" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="OrderNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderNo" Text='<%#Bind("OrderNo")%>' runat="server" />
                                                </ItemTemplate>
                                                 <HeaderStyle Width="20%" Font-Size="16px" />
                                                  <ItemStyle  Font-Size="14px" />
                                            </asp:TemplateField>  
                                             <asp:TemplateField HeaderText="" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderId" Text='<%#Bind("OrderId")%>' runat="server" />
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>  

                                                
                                               <%-- <asp:TemplateField HeaderText="Order Remark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemark" runat="server" align="right" TextMode="MultiLine" Width="90%"
                                                            Text='<%# Bind("Remark") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="70%" />
                                                    <ItemStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>--%>
                                            </Columns>
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
