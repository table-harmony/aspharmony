<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="AspClient.Home" %>
<%@ Import Namespace="AspClient.Utils" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="main" runat="server">
    <section class="d-flex flex-column align-items-center gap-4 py-5 text-center">
        <div class="rounded-circle bg-primary bg-opacity-10 p-4">
            <i class="bi bi-book text-primary display-6"></i>
        </div>
        
        <div class="mb-4">
            <h1 class="display-4 fw-bold mb-3">Welcome to AspHarmony</h1>
            <p class="lead text-muted mx-auto" style="max-width: 600px;">
                Your personal library in the cloud
            </p>
        </div>

        <div class="d-flex gap-3" style="max-width: 400px; width: 100%;">
            <% if (SessionManager.IsUserLoggedIn()) { %>
                <div runat="server" class="d-flex gap-3 w-100">
                    <a href="Books.aspx" class="btn btn-primary flex-grow-1">
                        <i class="bi bi-book me-2"></i> Books
                    </a>
                    <a href="Libraries.aspx" class="btn btn-outline-primary flex-grow-1">
                        <i class="bi bi-collection me-2"></i> Libraries
                    </a>
                </div>
            <% } else { %>
                <div runat = "server" class="d-flex gap-3 w-100">
                    <a href = "Login.aspx" class="btn btn-primary flex-grow-1">
                        <i class="bi bi-box-arrow-in-right me-2"></i> Login
                    </a>
                    <a href = "Register.aspx" class="btn btn-outline-primary flex-grow-1">
                        <i class="bi bi-person-plus me-2"></i> Register
                    </a>
                </div>
            <% } %>       
        </div>
    </section>
</asp:Content>