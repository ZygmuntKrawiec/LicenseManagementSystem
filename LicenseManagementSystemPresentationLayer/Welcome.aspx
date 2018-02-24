﻿<%@ Page Language="C#" MasterPageFile="~/MainSite.Master" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="LicenseManagementSystemPresentationLayer.Welcome" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphLogoutContent" runat="server">
    <asp:Label ID="lblUserName" runat="server" Text="UserName"></asp:Label>
    (<asp:LinkButton ID="lbnLogout" runat="server" OnClick="lbnLogout_Click">Logout</asp:LinkButton>)    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Welcome
    </h1>
    <asp:DropDownList ID="ddlRowsPerPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRowsPerPage_SelectedIndexChanged">
        <asp:ListItem Selected="True" Text="10" Value="10"></asp:ListItem>
        <asp:ListItem Text="20" Value="20"></asp:ListItem>
        <asp:ListItem Text="50" Value="50"></asp:ListItem>
        <asp:ListItem Text="100" Value="100"></asp:ListItem>
    </asp:DropDownList>
    <div style="font-family: Arial;">
        <asp:GridView ID="gvLicenseData" runat="server"  AllowSorting="True" AllowPaging="True" AllowCustomPaging="True" OnSorting="grvLicenseData_Sorting">
        </asp:GridView>
        <asp:Repeater ID="rprPages" runat="server" ItemType="System.Web.UI.WebControls.ListItem">            
            <ItemTemplate>
                <asp:LinkButton ID="lbnPage" runat="server" Text='<%# Item.Text %>' 
                     CommandArgument='<%# Item.Value %>' Enabled='<%# Item.Enabled %>' Visible='<%# Item.Selected %>' OnClick="lbnPage_Click" ></asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
    </div>   
</asp:Content>
