<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
Inherits="aspharmony.View.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title></title>
  </head>
  <body>
    <div id="access-window">
      <header>Login</header>
      <form id="form1" runat="server">
        <input
          type="text"
          id="username"
          name="username"
          spellcheck="false"
          placeholder="Username"
          autocomplete="off"
          required="required"
        />
        <input
          type="password"
          id="password"
          name="password"
          spellcheck="false"
          placeholder="Password"
          required="required"
        />
        <input type="submit" name="submit" id="submit" value="submit" />

        <%=msg %>
      </form>
      <footer>
        don't have an account ? <a href="Register.aspx"><b>sign up now</b></a>
      </footer>
    </div>
  </body>
</html>
