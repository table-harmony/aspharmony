<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="aspharmony.View.Update" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="FormValid.js" defer></script>
    <style>
        input {
            outline: none;
        }
        input.invalid {
            border-color: rgb(255, 150, 150);
        }
        input.valid {
            border-color: rgb(150, 255, 150);
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="text" name="username" spellcheck="false" placeholder="Username" data-pattern="^[A-Za-z0-9]{2,15}$" autocomplete="off" value=<%=Session["username"] %> disabled="disabled" />
            <input type="password" name="password" spellcheck="false" data-pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{5,}$" placeholder="Password" required="required" value="<%=password %>" />
            <input type="email" name="gmail" data-pattern="^[^\s@]+@[^\s@]+\.[^\s@]+$" spellcheck="false" placeholder="Email" autocomplete="off" required="required" value="<%=gmail %>" />
            <input type="radio" name="gender" value="1" <% if (gender.ToString() == "1") Response.Write("checked"); %> />
            <input type="radio" name="gender" value="0" <% if (gender.ToString() == "0") Response.Write("checked"); %> />
            <input type="submit" name="submit" value="submit" />
        </div>
    </form>
</body>
</html>
