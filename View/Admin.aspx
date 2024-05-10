<%@ Page Title="" Language="C#" MasterPageFile="~/View/Site1.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="aspharmony.View.Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <form id="form1" runat="server">
      <asp:GridView
        ID="UsersGrid"
        runat="server"
        AllowSorting="true"
        AllowPaging="true"
        PageSize="4"
        OnPageIndexChanging="UsersGrid_PageIndexChanging"
        OnSorting="UsersGrid_Sorting"
        OnRowEditing="UsersGrid_RowEditing"
        OnRowCancelingEdit="UsersGrid_RowCancelingEdit"
        OnRowUpdating="UsersGrid_RowUpdating"
        OnRowDeleting="UsersGrid_RowDeleting"
        AutoGenerateColumns="false"
      >
        <Columns>
          <asp:TemplateField>
            <ItemTemplate>
              <asp:CheckBox ID="Chk" AutoPostBack="false" runat="server" />
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField 
              DataField="id" 
              HeaderText="id"
              ReadOnly="true"
          />
          <asp:BoundField
            DataField="email"
            SortExpression="email"
            HeaderText="email"
          />
          <asp:BoundField
            DataField="password"
            SortExpression="password"
            HeaderText="password"
          />
          <asp:BoundField
            DataField="gender"
            SortExpression="gender"
            HeaderText="gender"
          />
          <asp:BoundField
            DataField="role"
            SortExpression="role"
            HeaderText="role"
          />

          <asp:ButtonField ButtonType="Button" Text="delete" CommandName="delete" HeaderText="delete" />
          <asp:CommandField ButtonType="Button" ShowEditButton="true" EditText="edit" HeaderText="edit" />
        </Columns>
      </asp:GridView>
    </form>
</asp:Content>
