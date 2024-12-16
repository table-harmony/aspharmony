<%@ Page Async="true" Title="Library Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LibraryDetails.aspx.cs" Inherits="AspClient.LibraryDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="mb-4">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="Libraries.aspx">Libraries</a></li>
                <li class="breadcrumb-item active"><asp:Literal ID="LibraryNameBreadcrumb" runat="server" /></li>
            </ol>
        </nav>
    </div>

    <form runat="server" class="row">
        <div class="col-md-8">
            <div class="card mb-4">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h3 class="mb-0">Books</h3>
                </div>
                <div class="card-body">
                        <asp:GridView ID="BooksGridView" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-hover" DataKeyNames="Id">
                            <Columns>
                                <asp:TemplateField HeaderText="Cover">
                                    <ItemTemplate>
                                        <asp:Image ID="BookImage" runat="server" 
                                            ImageUrl='<%# Eval("Book.Metadata.ImageUrl") %>'
                                            Width="50px" Height="70px" CssClass="rounded" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Book.Metadata.Title" HeaderText="Title" />
                                <asp:BoundField DataField="Book.Author.Username" HeaderText="Author" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="BookDetailsLink" runat="server" 
                                            NavigateUrl='<%# $"~/BookDetails.aspx?id={Eval("Book.Id")}" %>'
                                            CssClass="btn btn-info btn-sm">
                                            <i class="bi bi-info-circle"></i> Details
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card mb-4">
                <div class="card-header">
                    <h3 class="mb-0">Library Info</h3>
                </div>
                <div class="card-body">
                    <dl class="row mb-0">
                        <dt class="col-sm-5">Name</dt>
                        <dd class="col-sm-7"><asp:Literal ID="LibraryName" runat="server" /></dd>

                        <dt class="col-sm-5">Multiple Copies</dt>
                        <dd class="col-sm-7"><asp:Literal ID="AllowCopies" runat="server" /></dd>

                        <dt class="col-sm-5">Total Books</dt>
                        <dd class="col-sm-7"><asp:Literal ID="TotalBooks" runat="server" /></dd>
                    </dl>
                </div>
            </div>

            <div class="card mb-4">
                <div class="card-header">
                    <h3 class="mb-0">Members</h3>
                </div>
                <div class="card-body">
                    <asp:GridView ID="MembersGridView" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-sm" DataKeyNames="Id">
                        <Columns>
                            <asp:BoundField DataField="User.Username" HeaderText="Username" />
                            <asp:BoundField DataField="Role" HeaderText="Role" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <asp:Panel ID="ManagerPanel" runat="server" CssClass="d-flex gap-2" Visible="false">
                <asp:HyperLink ID="EditLink" runat="server" CssClass="btn btn-warning">
                    <i class="bi bi-pencil"></i> Edit
                </asp:HyperLink>
                <asp:HyperLink ID="DeleteLink" runat="server" CssClass="btn btn-danger">
                    <i class="bi bi-trash"></i> Delete
                </asp:HyperLink>
            </asp:Panel>
        </div>
    </form>
</asp:Content>