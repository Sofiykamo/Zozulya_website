<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Shop.User.Profile" %>
<%@Import Namespace="Shop" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5 wow fadeIn" data-wow-delay="0.1s">
        <div class="container">
            <h1 class="display-3 mb-3 animated slideInDown">Products</h1>
            <nav aria-label="breadcrumb animated slideInDown">
                <ol class="breadcrumb mb-0">
                    <li class="breadcrumb-item"><a class="text-body" href="#">Home</a></li>
                    <li class="breadcrumb-item"><a class="text-body" href="#">Pages</a></li>
                    <li class="breadcrumb-item text-dark active" aria-current="page">Products</li>
                </ol>
            </nav>
        </div>
    </div>
    <!-- Page Header End -->

    <%
        string imageUrl = Session["imageUrl"].ToString();
        %>
    
    <section class="'book_section layout-padding">

        <div class="container">
            <div class="heading_container">
                <h2>User Information</h2>
            </div>
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="card-title mb-4">
                                <div class="d-flex justify-content-start">
                                    <div class="image-container">
                                        <img src="<% =Utils.GetImageUrl(imageUrl) %>" id="imgProfile" 
                                            style="width:150px; height:150px;" class="img-thumbnail" />
                                        <div class="middle pt-2">
                                            <a href="Registration.aspx?id=<%Response.Write(Session["userId"]); %>" 
                                                class="btn btn-warning"> <i class="fadeIn fa-pencil"></i> Edit Details
                                            </a>
                                        </div>
                                    </div>


                                    <div class="userData ml-3">
                                        <h2 class="d-block" style="font-size: 1.5rem; font-weight:bold">
                                            <a href="javascript:void(0);"><%Response.Write(Session["name"]); %></a>

                                        </h2>
                                        <h6 class="d-block">
                                            <a href="javascript:void(0);">
                                                <asp:Label ID="lblUsername" runat="server" ToolTip="Unique Username">

                                                    <%Response.Write(Session["username"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>

                                        <h6 class="d-block">
                                            <a href="javascript:void(0);">
                                                <asp:Label ID="lblEmail" runat="server" ToolTip="User Email">
                                                    <%Response.Write(Session["email"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                        <h6 class="d-block">
                                            <a href="javascript:void(0);">
                                                <asp:Label ID="lblCreatedDate" runat="server" ToolTip="Account Created On">
                                                    <%Response.Write(Session["createDate"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-12">

                                        <ul class="nav nav-tabs mb-4" id="myTab" role="tablist">
                                            <li class="nav-item">
                                                <a class="nav-link active text-info" id="basicInfo-tab" data-toggle="tab" href="#basicInfo" 
                                                    role="tab" aria-controls="basicInfo" aria-selected="true">
                                                    <i class="fa fa-id-badge mr-2"></i>Basic Info
                                                </a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link active text-info" id="connectedServices-tab" data-toggle="tab" href="#connectedServices" 
                                                    role="tab" aria-controls="basicInfo" aria-selected="false">
                                                    <i class="fa fa-clock-o mr-2"></i>Purchased History
                                                </a>
                                            </li>
                                        </ul>
                                        <div class="tab-content ml-1" id="myTabContent">
                                            <div class="tab-pane fade show active" id="basicInfo" role="tabpanel" aria-labelledby="basicInfo-tab">
                                                <asp:Repeater ID="rUserProfile" runat="server">
                                                    <ItemTemplate>

                                                        <div class="row">
                                                            <div class="col-sm-3 col-md-2 col-5">
                                                                <label style="font-weight:bold;">Full Name</label>
                                                            </div>
                                                            <div class="col-md-8 col-6">
                                                                <%#Eval("Name") %>
                                                            </div>
                                                        </div>
                                                        <hr />
                                                         <div class="row">
                                                            <div class="col-sm-3 col-md-2 col-5">
                                                                <label style="font-weight:bold;">Username</label>
                                                            </div>
                                                            <div class="col-md-8 col-6">
                                                                <%#Eval("Username") %>
                                                            </div>
                                                        </div>
                                                        <hr />
                                                         <div class="row">
                                                            <div class="col-sm-3 col-md-2 col-5">
                                                                <label style="font-weight:bold;">Mobile Number</label>
                                                            </div>
                                                            <div class="col-md-8 col-6">
                                                                <%#Eval("Mobile") %>
                                                            </div>
                                                        </div>
                                                        <hr />
                                                         <div class="row">
                                                            <div class="col-sm-3 col-md-2 col-5">
                                                                <label style="font-weight:bold;">Email Address</label>
                                                            </div>
                                                            <div class="col-md-8 col-6">
                                                                <%#Eval("Email") %>
                                                            </div>
                                                        </div>
                                                        <hr />
                                                         <div class="row">
                                                            <div class="col-sm-3 col-md-2 col-5">
                                                                <label style="font-weight:bold;">Post/Zip Code</label>
                                                            </div>
                                                            <div class="col-md-8 col-6">
                                                                <%#Eval("PostCode") %>
                                                            </div>
                                                        </div>
                                                        <hr />
                                                         <div class="row">
                                                            <div class="col-sm-3 col-md-2 col-5">
                                                                <label style="font-weight:bold;">Delivery address</label>
                                                            </div>
                                                            <div class="col-md-8 col-6">
                                                                <%#Eval("Address") %>
                                                            </div>
                                                        </div>
                                                        <hr />
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                            <%--Order History Start--%>
                                            <div class="tab-tab-pane fade" id="connectedServices" role="tabpanel" 
                                                aria-labelledby="ConnectedServices-tab">
                                                <h3> </h3>
                                            </div>
                                             <%--Order History END--%>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>


            </div>
        </section>


</asp:Content>
