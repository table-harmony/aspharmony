<%@ Page Async="true" Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="AspClient.Register" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="main" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-6 col-lg-5">
            <div class="text-center mb-4">
                <div class="rounded-circle bg-primary bg-opacity-10 p-4 d-inline-block mb-4">
                    <i class="bi bi-person-plus text-primary" style="font-size: 2rem;"></i>
                </div>
                <h1 class="h3 mb-3">Create Account</h1>
            </div>

            <form runat="server" class="card border-0 shadow-sm">
                <div class="card-body p-4">
                    <asp:Panel ID="ErrorPanel" runat="server" Visible="false" 
                        CssClass="alert alert-danger d-flex align-items-center" Role="alert">
                        <i class="bi bi-exclamation-circle flex-shrink-0 me-2"></i>
                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                    </asp:Panel>

                    <div class="mb-3">
                        <label class="form-label">Email</label>
                        <div class="input-group">
                            <span class="input-group-text bg-transparent">
                                <i class="bi bi-envelope"></i>
                            </span>
                            <asp:TextBox ID="EmailTextBox" runat="server" CssClass="form-control" 
                                TextMode="Email" placeholder="name@example.com" />
                        </div>
                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                            ControlToValidate="EmailTextBox" Display="Dynamic"
                            CssClass="text-danger small" ErrorMessage="Email is required" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Password</label>
                        <div class="input-group">
                            <span class="input-group-text bg-transparent">
                                <i class="bi bi-lock"></i>
                            </span>
                            <asp:TextBox ID="PasswordTextBox" runat="server" CssClass="form-control" 
                                TextMode="Password" placeholder="••••••••" />
                        </div>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                            ControlToValidate="PasswordTextBox" Display="Dynamic"
                            CssClass="text-danger small" ErrorMessage="Password is required" />
                    </div>

                    <div class="mb-4">
                        <label class="form-label">Confirm Password</label>
                        <div class="input-group">
                            <span class="input-group-text bg-transparent">
                                <i class="bi bi-lock-fill"></i>
                            </span>
                            <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" CssClass="form-control" 
                                TextMode="Password" placeholder="••••••••" />
                        </div>
                        <asp:CompareValidator ID="PasswordCompare" runat="server"
                            ControlToCompare="PasswordTextBox" ControlToValidate="ConfirmPasswordTextBox"
                            Display="Dynamic" CssClass="text-danger small"
                            ErrorMessage="Passwords must match" />
                    </div>

                    <asp:Button ID="RegisterButton" runat="server" Text="Create Account" 
                        CssClass="btn btn-primary w-100 mb-3" OnClick="RegisterButton_Click" />

                    <div class="text-center">
                        <a href="Login.aspx" class="text-decoration-none">
                            <i class="bi bi-box-arrow-in-right me-2"></i>Already have an account?
                        </a>
                    </div>
                </div>
            </form>
        </div>
    </div>
</asp:Content>