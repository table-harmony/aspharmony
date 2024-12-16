<%@ Page Title="Server Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="500.aspx.cs" Inherits="AspClient._500" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-8 text-center">
                <div class="card border-danger">
                    <div class="card-body py-5">
                        <h1 class="display-1 text-danger mb-4">
                            <i class="bi bi-x-circle"></i> 500
                        </h1>
                        <h2 class="mb-4">Server Error</h2>
                        <div class="alert alert-danger d-inline-block mb-4">
                            <asp:Label ID="ErrorMessage" runat="server" CssClass="h5" />
                        </div>
                        <div class="mb-4">
                            <a href="Home.aspx" class="btn btn-primary">
                                <i class="bi bi-house-door"></i> Return Home
                            </a>
                        </div>
                        <p class="text-muted small">
                            Error Code: 500 Internal Server Error
                            <br />
                            Time: <%= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>