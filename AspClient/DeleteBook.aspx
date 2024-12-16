<%@ Page Async="true" Title="Delete Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeleteBook.aspx.cs" Inherits="AspClient.DeleteBook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="card">
        <div class="card-header">
            <h2>Delete Book</h2>
        </div>
        <div class="card-body">
            <div class="alert alert-danger">
                <i class="bi bi-exclamation-triangle"></i> Are you sure you want to delete "<asp:Literal ID="BookTitleLiteral" runat="server" />"? This action cannot be undone.
            </div>

            <form runat="server">
                <asp:HiddenField ID="BookIdHidden" runat="server" />
                
                <div class="mt-4">
                    <asp:Button ID="DeleteButton" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="DeleteButton_Click" />
                    <a href="Books.aspx" class="btn btn-secondary">Cancel</a>
                </div>
            </form>
        </div>
    </div>
</asp:Content>