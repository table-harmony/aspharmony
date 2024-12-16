<%@ Page Async="true" Title="Create Library" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateLibrary.aspx.cs" Inherits="AspClient.CreateLibrary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="card">
        <div class="card-header">
            <h2 class="mb-0">Create New Library</h2>
        </div>
        <div class="card-body">
            <p class="text-muted">Create a new library to share books with others.</p>

            <form runat="server">
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="NameTextBox" CssClass="form-label">Library Name</asp:Label>
                    <div class="input-group">
                        <span class="input-group-text"><i class="bi bi-book"></i></span>
                        <asp:TextBox ID="NameTextBox" runat="server" CssClass="form-control" />
                    </div>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NameTextBox" 
                        CssClass="text-danger" ErrorMessage="Library name is required." />
                </div>

                <div class="mb-3">
                    <div class="form-check">
                        <asp:CheckBox ID="AllowCopiesCheckBox" runat="server" CssClass="form-check-input" />
                        <asp:Label runat="server" AssociatedControlID="AllowCopiesCheckBox" CssClass="form-check-label">
                            Allow Multiple Copies of Books
                        </asp:Label>
                    </div>
                    <small class="text-muted">If enabled, multiple copies of the same book can be added to the library.</small>
                </div>

                <div class="mt-4">
                    <asp:Button ID="CreateButton" runat="server" Text="Create Library" 
                        CssClass="btn btn-primary" OnClick="CreateButton_Click" />
                    <a href="Libraries.aspx" class="btn btn-secondary">
                        <i class="bi bi-x-circle"></i> Cancel
                    </a>
                </div>
            </form>
        </div>
    </div>
</asp:Content>