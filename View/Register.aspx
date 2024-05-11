<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="aspharmony.View.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title></title>
  </head>
  <body>
    <form id="form1" runat="server">
      <div>
        <input
          type="email"
          name="email"
          data-pattern="^[^\s@]+@[^\s@]+\.[^\s@]+$"
          spellcheck="false"
          placeholder="Email"
          autocomplete="off"
          required="required"
        />
        <input
          type="password"
          name="password"
          spellcheck="false"
          data-pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{5,}$"
          placeholder="Password"
          required="required"
        />
        <input
          type="text"
          name="name"
          spellcheck="false"
          placeholder="Name"
          autocomplete="off"
          required="required"
        />
        <input type="submit" name="submit" value="submit" />

        <p><%=msg %></p>
      </div>
    </form>

    <footer>
      have an account ? <a href="Login.aspx"><b>sign in now</b></a>
    </footer>
  </body>
</html>