<%@ Page Title="" Language="C#" MasterPageFile="~/Erplogin.master" AutoEventWireup="true"
    CodeFile="JoinNow.aspx.cs" Inherits="JoinNow" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .VeryPoor
        {
            background-color: red;
        }
        .Weak
        {
            background-color: orange;
        }
        .Average
        {
            background-color: #A52A2A;
        }
        .Excellent
        {
            background-color: yellow;
        }
        .Strong
        {
            background-color: green;
        }
        .border
        {
            border: thin solid #800000;
            width: 300px;
        }
        .style2
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
        <tr align="center">
            <td>
                <%--<div class="divloginsize" align="center">--%>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table class="style1">
                            <tr>
                                <td style="color: Maroon; font-weight: 600;">
                                    &nbsp;
                                </td>
                                <td style="color: Maroon; font-weight: 600; text-align: left;">
                                    Basic Details
                                </td>
                            </tr>
                            <tr>
                                <td class="style2" style="text-align: left">
                                    Contact Name<br />
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtContactname" runat="server" TabIndex="1" Style="text-align: left"
                                        Width="350px" AutoPostBack="True"></asp:TextBox>
                                  
                                </td>
                            </tr>
                            <tr>
                                <td class="style2" style="text-align: left">
                                    Company Name
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtcompanyname" runat="server" TabIndex="2" Width="350px" AutoPostBack="true"
                                        Style="text-align: left" ontextchanged="txtcompanyname_TextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="Filteredcompany" TargetControlID="txtcompanyname"
                                        FilterType="Custom" InvalidChars="0123456789!@#$%^&*()?" FilterMode="InvalidChars"
                                        runat="server">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr visible="false" runat="server">
                                <td class="style2">
                                    Type Of Business
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbusiness" runat="server" TabIndex="3" Width="350px" Style="margin-left: 0px;
                                        text-align: left;">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    Phone/Mobile
                                </td>
                                <td>
                                    <asp:TextBox ID="txtmobile" runat="server" Width="350px" TabIndex="4" MaxLength="15"
                                        Style="text-align: left"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredMobile" TargetControlID="txtmobile" FilterType="Numbers"
                                        runat="server">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    Email
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtemail" runat="server" Width="350px" TabIndex="5" OnTextChanged="txtemail_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3" style="color: Maroon; font-weight: 600;">
                                    &nbsp;
                                </td>
                                <td class="style3" style="color: Maroon; font-weight: 600; text-align: left;">
                                    Choose Username & Password
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    UserName
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TxtUserName" runat="server" Width="350px" TabIndex="5" AutoPostBack="True"
                                        OnTextChanged="TxtUserName_TextChanged"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="Filteredcontact" TargetControlID="TxtUserName"
                                        FilterType="Custom" InvalidChars="0123456789!@#$%^&*()?" FilterMode="InvalidChars"
                                        runat="server">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    Password
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtpwd" runat="server" Width="350px" TabIndex="6" TextMode="Password"
                                        MaxLength="15"></asp:TextBox>
                                    <asp:PasswordStrength ID="pwdstrength" runat="server" DisplayPosition="RightSide"
                                        StrengthIndicatorType="BarIndicator" TargetControlID="txtpwd" PrefixText="Stength:"
                                        Enabled="true" RequiresUpperAndLowerCaseCharacters="true" PreferredPasswordLength="10"
                                        TextStrengthDescriptions="VeryPoor; Weak; Average; Strong; Excellent" StrengthStyles="VeryPoor; Weak; Average; Excellent; Strong;"
                                        CalculationWeightings="25;25;15;35" BarBorderCssClass="border" HelpStatusLabelID="lblpwd">
                                    </asp:PasswordStrength>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    Confirm Passwrod
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtconfirmpwd" ClientIDMode="Static" TabIndex="7" OnBlur="javascript:pwdvalidate();"
                                        runat="server" Width="350px" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    &nbsp;
                                </td>
                                <td class="style2">
                                    <asp:CheckBox ID="chkterm" runat="server" TabIndex="8" Style="text-align: left" />
                                    &nbsp;<span style="color: rgb(0, 0, 0); font-family: Arial, Helvetica, sans-serif;
                                        font-size: 12px; font-style: normal; font-variant: normal; font-weight: normal;
                                        letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto;
                                        text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;
                                        -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; background-color: rgb(246, 247, 247);
                                        display: inline !important; float: none;">Yes, I agree to the<span class="Apple-converted-space">&nbsp;</span><asp:HyperLink
                                            ID="HyperLink1" runat="server" ForeColor="#FF6600">terms of service &amp; privacy policy.</asp:HyperLink>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    &nbsp;
                                </td>
                                <td style="text-align: left">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    &nbsp;<span style="color: rgb(0, 0, 0); font-family: Arial, Helvetica, sans-serif;
                                        font-size: 12px; font-style: normal; font-variant: normal; font-weight: normal;
                                        letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto;
                                        text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;
                                        -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; background-color: rgb(246, 247, 247);
                                        display: inline !important; float: none;">Already a Member ? <span class="Apple-converted-space">
                                            &nbsp;</span><asp:HyperLink ID="Loginhere" runat="server" ForeColor="#FF6600" NavigateUrl="~/LoginErp.aspx">Login Here</asp:HyperLink>
                                    </span>
                                </td>
                                <td style="text-align: left">
                                    <asp:Button ID="btncreate" runat="server" TabIndex="9" OnClientClick=" return validate()"
                                        Font-Size="Large" Text="Create Your Account" Height="29px" OnClick="btncreate_Click"
                                        CssClass="button" Width="200px" />All fields are mandatory
                                </td>                                
                            </tr>
                            <tr>
                            <td>
                                <asp:Label ID="lblerr" runat="server" Text=""></asp:Label>
                            </td></tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        function validate() {
            if (document.getElementById("<%=txtContactname.ClientID%>").value == "") {
                alert("Contact Field can not be blank!");
                document.getElementById("<%=txtContactname.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtcompanyname.ClientID %>").value == "") {
                alert("Company Name can not be blank!");
                document.getElementById("<%=txtcompanyname.ClientID %>").focus();
                return false;
            }
            //            var business = document.getElementById('<%=ddlbusiness.ClientID%>');
            //            if (business.value == 0) {
            //                alert("Please Choose Business Type!");
            //                document.getElementById("<%=ddlbusiness.ClientID %>").focus();
            //                return false;
            //            }


            if (document.getElementById("<%=txtmobile.ClientID %>").value == "") {
                alert("Phone or Mobile should not be blank!");
                document.getElementById("<%=txtmobile.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=txtemail.ClientID %>").value == "") {
                alert("Email Id can not be blank!");
                document.getElementById("<%=txtemail.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtemail.ClientID %>").value == "") {
                var emailPat = /^(\".*\"|[A-Za-z]\w*)@(\[\d{1,3}(\.\d{1,3}){3}]|[A-Za-z]\w*(\.[A-Za-z]\w*)+)$/;
                var emailid = document.getElementById("<%=txtemail.ClientID %>").value;
                var matchArray = emailid.match(emailPat);
                if (matchArray == null) {
                    alert("Your email address seems incorrect. Please try again.");
                    document.getElementById("<%=txtemail.ClientID %>").focus();
                    return false;

                }
            }
            if (document.getElementById("<%=TxtUserName.ClientID %>").value == "") {
                alert("UserName can not be blank!");
                document.getElementById("<%=TxtUserName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtpwd.ClientID %>").value == "") {
                alert("Password can not be blank!");
                document.getElementById("<%=txtpwd.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtconfirmpwd.ClientID %>").value == "") {
                alert("Confirm Password can not be blank!");
                document.getElementById("<%=txtconfirmpwd.ClientID %>").focus();
                return false;
            } 
        }
        function pwdvalidate() {
            var pwd = document.getElementById('<%=txtpwd.ClientID%>');
            var cnfrm = document.getElementById('<%=txtconfirmpwd.ClientID%>');
            if (pwd.value != cnfrm.value) {
                alert("Password do not Match..!");
                document.getElementById("<%=txtconfirmpwd.ClientID %>").focus();
                return false;
            }
        }     

       
       
                   
    </script>
</asp:Content>
