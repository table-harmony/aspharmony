<%@ Page Title="Page Not Found" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="AspClient._404" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-8 text-center">
                <div class="card border-warning">
                    <div class="card-body py-5">
                        <h1 class="display-1 text-warning mb-4">
                            <i class="bi bi-exclamation-circle"></i> 404
                        </h1>
                        <h2 class="mb-4">Page Not Found</h2>
                        <p class="lead text-muted mb-4">
                            The page you're looking for doesn't exist or has been moved.
                        </p>
                        <div class="mb-4">
                            <a href="Home.aspx" class="btn btn-primary">
                                <i class="bi bi-house-door"></i> Return Home
                            </a>
                        </div>
                        <p class="text-muted small">
                            Error Code: 404 Not Found
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>