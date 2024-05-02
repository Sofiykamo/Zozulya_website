<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Shop.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Включіть ресурси для візуалізації даних, наприклад, Google Charts -->
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Контейнер для відображення діаграми -->
    <div id="curve_chart" style="width: 900px; height: 500px"></div>




</asp:Content>
