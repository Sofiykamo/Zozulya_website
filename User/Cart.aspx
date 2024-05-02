<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="Shop.User.Cart" %>
<%@ Import Namespace="Shop" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5 wow fadeIn" data-wow-delay="0.1s">
        <div class="container">
            <h1 class="display-3 mb-3 animated slideInDown">Cart</h1>
            <nav aria-label="breadcrumb animated slideInDown">
                <ol class="breadcrumb mb-0">
                    <li class="breadcrumb-item"><a class="text-body" href="#">Home</a></li>
                    <li class="breadcrumb-item"><a class="text-body" href="#">Pages</a></li>
                    <li class="breadcrumb-item text-dark active" aria-current="page">Cart</li>
                </ol>
            </nav>
        </div>
    </div>
    <!-- Page Header End -->


    <!-- Product Start -->
    <div class="container-xxl py-5">
        <div class="container">
            <div class="row g-0 gx-5 align-items-end">
                <div class="col-lg-6">
                    <div class="section-header text-start mb-5 wow fadeInUp" data-wow-delay="0.1s" style="max-width: 500px;">
                       
                        <div class="align-self-end">
 
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        
               
                   
                        </div>


                       <h1 class="display-5 mb-3">Your Shopping Cart</h1>
                        <p>I had to listen to the classical music because it calms me down, calms my nerves down.</p>
                    </div>
                </div>
                </div>
            </div>
        <div class="container">
                            <asp:Repeater ID="rCartItem" runat="server" OnItemCommand="rCartItem_ItemCommand" OnItemDataBound="rCartItem_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="table ">
                                        <thead>
                                            <tr>
                                                <th>Name</th>
                                                <th>Image</th>
                                                <th>Unit Price</th>
                                                <th>Quantity</th>
                                                <th>Total Price</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>

                                      
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                    <td>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </td>
                                        <td>
                                            <img width="60" height="65" src="<%# Utils.GetImageUrl(Eval("ImageUrl")) %>" alt="" />
                                        </td>
                                        <td>
                                            $<asp:Label ID="lblPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnInstrumentId" runat="server" Value='<%# Eval("InstrumentId") %>' />
                                                <asp:HiddenField ID="hdnQuantity" runat="server" Value='<%# Eval("Qty") %>' />
                                                <asp:HiddenField ID="hdnInsQuantity" runat="server" Value='<%# Eval("InsQty") %>' />
                                            
                                        </td>

                                        <td>
                                            <div class="instrument__detail__option">
                                                <div class="quantity">
                                                    <div class="pro-qty">
                                                        <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number" Text='<%# Eval("Quantity") %>' ></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="revQuantity" runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"
                                                             ValidationExpression="[1-9]*" ControlToValidate="txtQuantity"
                                                             Display="Dynamic" SetFocusOnError="true" EnableClientScript="true"></asp:RegularExpressionValidator>
                                                        </div>
                                                </div>
                                            </div>
                                        </td>

                                        <td>
                                              $<asp:Label ID="lblTotalPrice" runat="server"></asp:Label>
                                        </td>

                                 <td>
    <asp:LinkButton ID="lblDelete" runat="server" CommandName="remove" 
        CommandArgument='<%# Eval("InstrumentId") %>' 
        OnClientClick="return confirm('Do you want to remove this item from cart?');"
        CssClass="btn btn-danger">
        X
    </asp:LinkButton>
</td>


                                        </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr>
                                        <td colspan="3"></td>
                                        <td class="pl-lg-5">
                                            <b>Grand Total:-</b>
                                        </td>
                                        <td>$<% Response.Write(Session["grandTotalPrice"]); %></td>
                                        <td></td>
                                      <tr>
    <td colspan="2" class="continue__btn">
        <a href="Catalog.aspx" class="btn btn-info">
            <i class="fa fa-arrow-circle-left mr-2"></i> Continue Shopping
        </a>
    </td>
    <td colspan="2">
        <asp:LinkButton ID="lbUpdateCart" runat="server" CommandName="updateCart" CssClass="btn btn-warning">
            <i class="fa fa-refresh mr-2"></i> Update Cart
        </asp:LinkButton>
    </td>
    <td colspan="2">
        <asp:LinkButton ID="lbCheckout" runat="server" CommandName="checkout" CssClass="btn btn-success">
            Checkout <i class="fa fa-arrow-circle-right mr-2"></i>
        </asp:LinkButton>
    </td>
</tr>

                                      </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
        </div>
        </div>


   


</asp:Content>
