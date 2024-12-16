<%@ Page Async="true" Title="Books" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Books.aspx.cs" Inherits="AspClient.Books" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="card">
        <div class="card-header">
            <h2>Books Collection</h2>
            <a class="btn btn-primary" href="CreateBook.aspx">
                <i class="bi bi-pencil"></i> Create
            </a>
        </div>
        <form runat="server" class="card-body">
            <asp:GridView ID="BooksGridView" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-hover" DataKeyNames="Id">
                <Columns>
                    <asp:TemplateField HeaderText="Cover">
                        <ItemTemplate>
                            <asp:Image ID="BookImage" runat="server" 
                                ImageUrl='<%# Eval("Metadata.ImageUrl") %>'
                                Width="50px" Height="70px" CssClass="rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Metadata.Title" HeaderText="Title" />
                    <asp:BoundField DataField="Author.Username" HeaderText="Author" />
                    <asp:BoundField DataField="Server" HeaderText="Server" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:HyperLink ID="InfoLink" runat="server" 
                                NavigateUrl='<%# $"~/BookDetails.aspx?id={Eval("Id")}" %>'
                                CssClass="btn btn-info btn-sm">
                                <i class="bi bi-info-circle"></i> Info
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </form>
    </div>
</asp:Content>