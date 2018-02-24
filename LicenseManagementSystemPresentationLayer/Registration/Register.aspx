<%@ Page Language="C#" MasterPageFile="~/MainSite.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LicenseManagementSystemPresentationLayer.Registration.Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Registration Page
    </h1>
    <div>
        <table style="border: 1px solid black">
            <tr>
                <td colspan="2">
                    <b>Registration</b>
                </td>
            </tr>
            <tr>
                <td>Email:
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqEmail" ControlToValidate="txtEmail" runat="server" ErrorMessage="Email is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Password:
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqPassword" ControlToValidate="txtPassword" runat="server" ErrorMessage="Password is required." Display="Dynamic"></asp:RequiredFieldValidator>

                </td>
            </tr>
            <tr>
                <td>Repeat password:
                </td>
                <td>
                    <asp:TextBox ID="txtRepeatPassword" TextMode="Password" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqRepeatPassword" ControlToValidate="txtRepeatPassword" runat="server" ErrorMessage="Password is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cmpPassword" ControlToValidate="txtRepeatPassword" ControlToCompare="txtPassword" Type="String" Operator="Equal" runat="server" ErrorMessage="Passwords are not equal." Display="Dynamic"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
                </td>
            </tr>
        </table>
        <asp:Label runat="server" Visible="false" ID="lblMessage" ForeColor="red"></asp:Label>
    </div>
</asp:Content>
