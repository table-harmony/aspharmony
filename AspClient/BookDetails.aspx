<%@ Page Async="true" Title="Book Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="AspClient.BookDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
            <h2><asp:Literal ID="BookTitle" runat="server" /></h2>
            <asp:Panel ID="AuthorPanel" runat="server" CssClass="d-flex gap-2" Visible="false">
                <asp:HyperLink ID="EditLink" runat="server" CssClass="btn btn-warning">
                    <i class="bi bi-pencil"></i> Edit
                </asp:HyperLink>
                <asp:HyperLink ID="DeleteLink" runat="server" CssClass="btn btn-danger">
                    <i class="bi bi-trash"></i> Delete
                </asp:HyperLink>
            </asp:Panel>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-4">
                    <asp:Image ID="BookImage" runat="server" CssClass="img-fluid rounded" />
                </div>
                <div class="col-md-8">
                    <dl class="row">
                        <dt class="col-sm-3">Author</dt>
                        <dd class="col-sm-9"><asp:Literal ID="AuthorName" runat="server" /></dd>

                        <dt class="col-sm-3">Description</dt>
                        <dd class="col-sm-9"><asp:Literal ID="BookDescription" runat="server" /></dd>

                        <dt class="col-sm-3">Server</dt>
                        <dd class="col-sm-9"><asp:Literal ID="ServerType" runat="server" /></dd>
                    </dl>
                </div>
            </div>
        </div>
    </div>
</asp:Content>