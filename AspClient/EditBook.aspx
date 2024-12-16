<%@ Page Async="true" Title="Edit Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditBook.aspx.cs" Inherits="AspClient.EditBook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="card">
        <div class="card-header">
            <h2>Edit Book</h2>
        </div>
        <div class="card-body">
            <form runat="server">
                <asp:HiddenField ID="BookIdHidden" runat="server" />
                
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
                    <asp:Label runat="server" AssociatedControlID="ImageControl" CssClass="form-label">Cover Image</asp:Label>
                    <div class="d-flex gap-3 align-items-start">
                        <asp:Image ID="CurrentImage" runat="server" CssClass="rounded" Width="100" Height="150" />
                        <div class="flex-grow-1">
                            <asp:FileUpload ID="ImageControl" runat="server" CssClass="form-control" />
                            <small class="text-muted">Leave empty to keep current image. Maximum file size: 5MB. Supported formats: .jpg, .jpeg, .png</small>
                        </div>
                    </div>
                </div>

                <div class="mt-4">
                    <asp:Button ID="UpdateButton" runat="server" Text="Update Book" CssClass="btn btn-primary" OnClick="UpdateButton_Click" />
                    <a href="Books.aspx" class="btn btn-secondary">Cancel</a>
                </div>
            </form>
        </div>
    </div>
</asp:Content>