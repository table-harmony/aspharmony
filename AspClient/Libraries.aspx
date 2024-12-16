<%@ Page Async="true" Title="Libraries" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Libraries.aspx.cs" Inherits="AspClient.Libraries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
            <h2 class="mb-0">Libraries</h2>
            <a href="CreateLibrary.aspx" class="btn btn-primary">
                <i class="bi bi-plus-circle"></i> Create Library
            </a>
        </div>
        <div class="card-body">
            <form runat="server">
                <asp:GridView ID="LibrariesGridView" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover" DataKeyNames="Id">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Library Name" />
                        <asp:TemplateField HeaderText="Multiple Copies">
                            <ItemTemplate>
                                <i class="bi <%# (bool)Eval("AllowCopies") ? "bi-check text-success" : "bi-x text-danger" %>"></i>
                                <%# (bool)Eval("AllowCopies") ? "Yes" : "No" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:HyperLink ID="DetailsLink" runat="server" 
                                    NavigateUrl='<%# $"~/LibraryDetails.aspx?id={Eval("Id")}" %>'
                                    CssClass="btn btn-info btn-sm">
                                    <i class="bi bi-info-circle"></i> Details
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </form>
        </div>
    </div>
</asp:Content>