<%@ Page Async="true" Title="Create Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateBook.aspx.cs" Inherits="AspClient.CreateBook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="card">
        <div class="card-header">
            <h2>Create New Book</h2>
        </div>
        <div class="card-body">
            <form runat="server">
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="TitleTextBox" CssClass="form-label">Title</asp:Label>
                    <asp:TextBox ID="TitleTextBox" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TitleTextBox" 
                        CssClass="text-danger" ErrorMessage="Title is required." />
                </div>

                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="DescriptionTextBox" CssClass="form-label">Description</asp:Label>
                    <asp:TextBox ID="DescriptionTextBox" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DescriptionTextBox" 
                        CssClass="text-danger" ErrorMessage="Description is required." />
                </div>

                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="ImageControl" CssClass="form-label">Image URL</asp:Label>
                    <asp:FileUpload ID="ImageControl" runat="server" CssClass="form-control" required="true" />
                </div>

                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="ServerDropDown" CssClass="form-label">Server</asp:Label>
                    <asp:DropDownList ID="ServerDropDown" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Nimbus 1.0" Value="5" />
                    </asp:DropDownList>
                </div>

                <div class="mt-4">
                    <asp:Button ID="CreateButton" runat="server" Text="Create Book" CssClass="btn btn-primary" OnClick="CreateButton_Click" />
                    <a href="Books.aspx" class="btn btn-secondary">Cancel</a>
                </div>
            </form>
        </div>
    </div>
</asp:Content>