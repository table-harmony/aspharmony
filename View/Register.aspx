<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="aspharmony.View.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="text" name="username" spellcheck="false" placeholder="Username" data-pattern="^[A-Za-z0-9]{2,15}$" autocomplete="off" required="required" />
            <input type="password" name="password" spellcheck="false" data-pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{5,}$" placeholder="Password" required="required" />
            <input type="email" name="gmail" data-pattern="^[^\s@]+@[^\s@]+\.[^\s@]+$" spellcheck="false" placeholder="Email" autocomplete="off" required="required" />
            <input type="radio" name="gender" value="1" checked="checked" />
            <input type="radio" name="gender" value="0" />
            <input type="submit" name="submit" value="submit" />

            <%=msg %>
        </div>
    </form>
</body>
</html>
