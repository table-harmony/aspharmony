<%@ Page Title="Access Denied" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="403.aspx.cs" Inherits="AspClient._403" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-8 text-center">
                <div class="card border-danger">
                    <div class="card-body py-5">
                        <h1 class="display-1 text-danger mb-4">
                            <i class="bi bi-shield-lock"></i> 403
                        </h1>
                        <h2 class="mb-4">Access Denied</h2>
                        <div class="alert alert-danger d-inline-block mb-4">
                            <asp:Label ID="ErrorMessage" runat="server" CssClass="h5" 
                                Text="You don't have permission to access this resource." />
                        </div>
                        <div class="mb-4">
                            <a href="Home.aspx" class="btn btn-primary me-2">
                                <i class="bi bi-house-door"></i> Return Home
                            </a>
                            <a href="javascript:history.back()" class="btn btn-outline-secondary">
                                <i class="bi bi-arrow-left"></i> Go Back
                            </a>
                        </div>
                        <p class="text-muted small">
                            Error Code: 403 Forbidden
                            <br />
                            Time: <%= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>