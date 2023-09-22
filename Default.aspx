﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Src="~/UserControls/iexproMenu.ascx" TagPrefix="uc1" TagName="iexproMenu" %>


<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Bootstrap Example</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="<%= Page.ResolveUrl("~/lib/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/lib/bootstrap-icons/font/bootstrap-icons.css")%>"
        rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/lib/font-awesome/css/font-awesome.min.css")%>"
        rel="stylesheet" />
    <!--Loading style-->
    <link type="text/css" rel="stylesheet" href="<%= Page.ResolveUrl("~/Content/css/style.css")%>" />
    <%--    <link type="text/css" rel="stylesheet" href="<%= Page.ResolveUrl("~/Content/css/style-responsive.css")%>" />--%>

    <style type="text/css">
        .menuss {
            font-weight: bold;
            width: 100%;
            margin: 0px 0px 0px 0px;
            padding: 6px 6px 4px 6px;
            height: 40px;
            line-height: 100%;
            -webkit-border-radius: 24px;
            -webkit-box-shadow: 2px 2px 3px #666666;
            background: -webkit-gradient(linear, left top, left bottom, from(#FCFAFA), to(#7A7A7A));
            border: solid 1px #6D6D6D;
            position: relative;
            z-index: 1000;
            box-sizing: border-box;
            border-radius: 24px !important
        }

        .nav-link {
            font-size: 11px;
            color: #000
        }

        .dropdown-menu {
            background-color: #d2b48c !important
        }


        .dropdown-item {
            font-size: 11px !important;
            font-weight: bold !important;
        }

        a.dropdown-item:hover, a.ex1:active {
            background-color: #8B7355 !important;
            color: #fff !important
        }

        .img-css {
            width: 100%;
            height: 78px;
            padding-top: 10px;
            margin-bottom: 11px;
            border: solid 1px #eee;
        }

        .footer-content {
            border: solid 1px #333;
            padding: 5px;
            text-align: center
        }

        .dropdown-submenu {
            position: relative;
        }

            .dropdown-submenu .dropdown-menu {
                top: 10%;
                left: 100%;
                margin-top: -1px;
            }

        .navbar-nav li:hover > ul.dropdown-menu {
            display: block;
        }
    </style>

</head>
<body>

    <form id="frmSite" runat="server">


        <div class="container-fluid" style="background-color: #eee;">
            <div class="row" style="border: solid 1px #333; margin-left: -10px; margin-top: 2px;">
                <div class="col-9 ps-0 pe-0" style="background-color: #E3E3E3;">
                    <img src="<%= Page.ResolveUrl("~/images/header.jpg")%>" class="img-css">
                </div>
                <div class="col-3" style="border: solid 1px #eee; background-color: #499ad3;">
                    <div class="row">
                        <div class="col-5">
                            <%--<img src="<%= Page.ResolveUrl("~/images/champo-logo.png")%>" />--%>
                            <asp:Image ID="imgLogo" ImageUrl="~/images/champo-logo.png"
                                runat="server" Height="66px" Width="111px" />
                        </div>
                        <div class="col-7 text-center text-white pt-3">
                            <h6 class="mb-3">
                                <asp:Label ID="LblCompanyName" runat="server" Text="AGNEE INNOVATES(AI)"></asp:Label>
                            </h6>
                            <h6>
                                <asp:Label ID="LblUserName" runat="server" Text="ERP"></asp:Label></h6>

                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="background-color: #999; border: solid 1px #333;">
                <div class="col-9 ps-0 pe-0" style="border: solid 1px #eee; background-color: ##999999;">

                    <uc1:iexproMenu runat="server" ID="iexproMenu" />












                </div>
                <div class="col-3 ps-0 text-center" style="border: solid 1px #eee;">
                    <asp:LinkButton ID="btnLogout" runat="server" class="btn  btn-lg mt-2" OnClick="btnLogout_Click">
                        <strong>
                        <i class="fa fa-sign-out"></i>Sign out


                            </strong>
                    </asp:LinkButton>
                </div>
            </div>
        </div>


        <div class="container-fluid mt-3">
            <h3>Navbar With Dropdown</h3>
            <p>This example adds a dropdown menu in the navbar.</p>
        </div>
        <footer>
            <div class="container-fluid mt-3 footer-content">

                <p class="mb-0" style="font-size: 13px">
                    <strong>© Enablesoft Erp Consultancy Pvt. Ltd.All
                    rights reserved.</strong>
                </p>
            </div>


        </footer>
    </form>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/lib/jquery/dist/jquery.min.js")%>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/lib/bootstrap/js/bootstrap.bundle.min.js")%>"></script>
</body>
</html>



