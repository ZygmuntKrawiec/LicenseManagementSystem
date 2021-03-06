﻿<%@ Page Language="C#" MasterPageFile="~/MainSite.Master" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="LicenseManagementSystemPresentationLayer.Welcome" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphLogoutContent" runat="server">
    <asp:Label ID="lblUserName" runat="server" Text="UserName"></asp:Label>
    (<asp:LinkButton ID="lbnLogout" runat="server" OnClick="lbnLogout_Click">Logout</asp:LinkButton>)    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Welcome</h1>
    <asp:DropDownList ID="ddlRowsPerPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRowsPerPage_SelectedIndexChanged">
        <asp:ListItem Selected="True" Text="10" Value="10"></asp:ListItem>
        <asp:ListItem Text="20" Value="20"></asp:ListItem>
        <asp:ListItem Text="50" Value="50"></asp:ListItem>
        <asp:ListItem Text="100" Value="100"></asp:ListItem>
    </asp:DropDownList>
   
    <div style="font-family: Arial;">
        <asp:GridView ID="gvLicenseData" runat="server" AllowSorting="True" AllowPaging="True" AllowCustomPaging="True" OnSorting="grvLicenseData_Sorting" OnRowCommand="gvLicenseData_RowCommand" OnRowDataBound="gvLicenseData_RowDataBound">
        </asp:GridView>
        <asp:Repeater ID="rprPages" runat="server" ItemType="System.Web.UI.WebControls.ListItem">
            <ItemTemplate>
                <asp:LinkButton ID="lbnPage" runat="server" Text='<%# Item.Text %>'
                    CommandArgument='<%# Item.Value %>' Enabled='<%# Item.Enabled %>' Visible='<%# Item.Selected %>' OnClick="lbnPage_Click"></asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <asp:Label ID="lblMessages" runat="server" Text=""></asp:Label>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
     <asp:Panel ID="pnlButtons" runat="server">
        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" Width="40px"/>
        <asp:Button ID="btnModify" runat="server" Text="Modify" OnClick="btnModify_Click" Width="50px" />
        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" Width="50px" />
    </asp:Panel>
    <asp:Panel ID="pnlTextBoxes" runat="server" Visible="true">
        <asp:Label ID="lblUser" runat="server" Text="User name"></asp:Label>
        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
        <asp:Label ID="lblEmail" runat="server" Text="User email"></asp:Label>
        <asp:TextBox ID="txtUserEmail" runat="server"></asp:TextBox>
    </asp:Panel>
</asp:Content>


