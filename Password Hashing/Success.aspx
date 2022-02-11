<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="Password_Hashing.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>User Profile</h2>
        <p>email: <asp:Label ID="lb_email" runat="server"></asp:Label>
        </p>
        <p>ccNum : <asp:Label ID="lb_ccNum" runat="server"></asp:Label>
        </p>
        <p>cvv: <asp:Label ID="lb_cvv" runat="server"></asp:Label>
        </p>
        <p>expiry date: <asp:Label ID="lb_exp" runat="server"></asp:Label>
        </p>
        <p>(page is to demonstrate ccinfo can be unencrypted by server</p>
        <p>&nbsp;</p>
    </div>
        <asp:Button ID="btn_logout" runat="server" OnClick="btn_logout_click" Text="Logout" />
    </form>
</body>

</html>
