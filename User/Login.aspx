<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Shop.User.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

           
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5 wow fadeIn" data-wow-delay="0.1s">
        <div class="container">
            <h1 class="display-3 mb-3 animated slideInDown">Login</h1>
            <nav aria-label="breadcrumb animated slideInDown">
                <ol class="breadcrumb mb-0">
                    <li class="breadcrumb-item"><a class="text-body" href="#">Home</a></li>
                    <li class="breadcrumb-item"><a class="text-body" href="#">Pages</a></li>
                    <li class="breadcrumb-item text-dark active" aria-current="page">Login</li>
                </ol>
            </nav>
        </div>
    </div>
    <!-- Page Header End -->


    <script>  
        /*For disappearing alert message*/
        window.onload = function () {

            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <section class="book_section layout-padding">
    <div class="container">
        <div class="heading_container">
            <div class="align-self-end">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
            <h2>Login</h2>
        </div>
        <div class="row">
            <div class="col-md-my-6">
                <div class="form_container">
                    <img id="userLogin" src="../Images/login.jpg" alt="" class="img-thumbnail" />
                </div>
            </div>
            <div class="col-md-my-6">
                <div class="form_container">
                    <div>
                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="Username is required" ControlToValidate="txtUsername"
                            ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter Username"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password is required" ControlToValidate="txtPassword"
                            ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Enter Password" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="btn_box">
                        <asp:Button ID="btnLofin" runat="server" Text="Login" CssClass="btn btn-success rounded-pill pl-4 pr-4 text-white" OnClick="btnLofin_Click" />
                        <span class="pl-3 text-info">New User? <a href="Registration.aspx" class="badge badge-info">Register Here..</a></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>

<style>
    .book_section {
        padding: 60px 0;
        background-color: #f8f9fa;
    }

    .book_section h2 {
        margin-bottom: 30px;
    }

    .form_container {
        background-color: #fff;
        padding: 30px;
        border-radius: 8px;
        box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
    }

    .form_container img {
        max-width: 100%;
    }

    .form_container input[type="text"],
    .form_container input[type="password"] {
        width: 100%;
        padding: 10px;
        margin-bottom: 20px;
        border: 1px solid #ced4da;
        border-radius: 5px;
    }

    .btn_box {
        text-align: center;
    }

    .btn_box a {
        text-decoration: none;
        color: #17a2b8;
    }

    .btn_box a:hover {
        color: #138496;
    }
</style>


</asp:Content>
