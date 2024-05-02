<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="aspharmony.View.Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="UsersGrid" runat="server"
            AllowSorting="true" OnSorting="UsersGrid_Sorting"
            AllowPaging="true" PageSize="4" OnPageIndexChanging="UsersGrid_PageIndexChanging"
            AutoGenerateColumns="false"
        >
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="Chk" AutoPostBack="false" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="usernameField" SortExpression="usernameField" HeaderText="username"/>
                <asp:BoundField DataField="passwordField" SortExpression="passwordField" HeaderText="password"/>
                <asp:BoundField DataField="gmailField" SortExpression="gmailField" HeaderText="gmail"/>
                <asp:BoundField DataField="accesskeyField" SortExpression="accesskeyField" HeaderText="key"/>
            </Columns>
        </asp:GridView>  
    </form>
</body>
</html>
