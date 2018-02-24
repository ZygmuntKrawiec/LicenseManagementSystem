<%@ Page Language="C#" MasterPageFile="~/MainSite.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LicenseManagementSystemPresentationLayer.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Login Page</h1>
    <div>
        <table style="border: 1px solid black">
            <tr>
                <td colspan="2">
                    <b>LOGIN</b>
                </td>
            </tr>
            <tr>
                <td>Email:
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr> 
                <td>Password:
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbxRememberMe" runat="server" Text="Remember me." />
                </td>
                <td>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                </td>
            </tr>
            <tr>
                <td><a href="/Registration/Register.aspx">Registration</a></td>
            </tr>
        </table>
        <asp:Label runat="server" Visible="false" ID="lblMessage" ForeColor="red"></asp:Label>
    </div>

</asp:Content>
