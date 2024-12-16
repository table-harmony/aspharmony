<%@ Page Title="Logout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="AspClient.Logout" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="main" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-6 col-lg-5">
            <div class="text-center mb-4">
                <div class="rounded-circle bg-primary bg-opacity-10 p-4 d-inline-block mb-4">
                    <i class="bi bi-box-arrow-right text-primary" style="font-size: 2rem;"></i>
                </div>
                <h1 class="h3 mb-3">Logout Confirmation</h1>
            </div>

            <div class="card border-0 shadow-sm">
                <div class="card-body p-4">
                    <p class="text-center mb-4">Are you sure you want to logout?</p>

                    <form runat="server" class="d-flex gap-3 justify-content-center">
                        <asp:Button ID="LogoutButton" runat="server" Text="Logout" 
                            CssClass="btn btn-danger" OnClick="LogoutButton_Click" />
                        <asp:Button ID="CancelButton" runat="server" Text="Cancel" 
                            CssClass="btn btn-secondary" OnClick="CancelButton_Click" />
                    </form>
                </div>
            </div>
        </div>
    </div>
</asp:Content>