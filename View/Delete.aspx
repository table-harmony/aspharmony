<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="aspharmony.View.Delete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title></title>
  </head>
  <body>
    <div id="access-window">
      <header>Delete</header>
      <form id="form1" runat="server">
        <input type="submit" name="submit" id="submit" value="delete" />

        <p><%=msg %></p>
      </form>
    </div>
  </body>
</html>