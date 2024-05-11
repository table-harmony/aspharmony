<%@ Page Title="" Language="C#" MasterPageFile="~/View/Site1.Master" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="aspharmony.View.Update" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
      <div>
        <input
          type="email"
          name="email"
          spellcheck="false"
          placeholder="Email"
          required="required"
          value=<%=Session["email"] %>
        />
        <input
          type="password"
          name="password"
          spellcheck="false"
          data-pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{5,}$"
          placeholder="Password"
          required="required"
          value="<%=password %>"
        />
        <input
          type="text"
          name="name"
          spellcheck="false"
          placeholder="Name"
          required="required"
          value=<%=Session["name"] %>
        />
        <input type="submit" name="submit" value="submit" />
    
        <p><%=msg %></p>
      </div>
    </form>
</asp:Content>
