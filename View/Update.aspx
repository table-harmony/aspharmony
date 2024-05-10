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
          type="radio" 
          name="gender" 
          value="1" 
          <% if (gender.ToString() == "1") Response.Write("checked"); %> 
        />
        <input 
          type="radio" 
          name="gender" 
          value="0" 
          <% if (gender.ToString() == "0") Response.Write("checked"); %> 
        /> 

        <input type="submit" name="submit" value="submit" />
    
        <p><%=msg %></p>
      </div>
    </form>
</asp:Content>
