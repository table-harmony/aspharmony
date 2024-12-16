<%@ Page Async="true" Title="Edit Library" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditLibrary.aspx.cs" Inherits="AspClient.EditLibrary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="card">
        <div class="card-header">
            <h2>Edit Library</h2>
        </div>
        <div class="card-body">
            <form runat="server">
                <asp:HiddenField ID="LibraryIdHidden" runat="server" />
                
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="NameTextBox" CssClass="form-label">Name</asp:Label>
                    <asp:TextBox ID="NameTextBox" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NameTextBox" 
                        CssClass="text-danger" ErrorMessage="Name is required." />
                </div>

                <div class="mb-3">
                    <div class="form-check">
                        <asp:CheckBox ID="AllowCopiesCheckBox" runat="server" CssClass="form-check-input" />
                        <asp:Label runat="server" AssociatedControlID="AllowCopiesCheckBox" CssClass="form-check-label">
                            Allow Multiple Copies
                        </asp:Label>
                    </div>
                </div>

                <div class="mt-4">
                    <asp:Button ID="UpdateButton" runat="server" Text="Update Library" CssClass="btn btn-primary" OnClick="UpdateButton_Click" />
                    <a href="Libraries.aspx" class="btn btn-secondary">Cancel</a>
                </div>
            </form>
        </div>
    </div>
</asp:Content>