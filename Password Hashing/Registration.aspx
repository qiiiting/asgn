<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Password_Hashing.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script src="https://www.google.com/recaptcha/api.js?render=6Lfyc24eAAAAAJ_ZN_WUOnICOH0eHs9857T2lgc-"></script>

    <style type="text/css">
        .auto-style1 {
            height: 46px;
        }
        .auto-style2 {
            height: 46px;
            margin-left: 80px;
        }
        .auto-style3 {
            height: 28px;
        }
        .auto-style4 {
            height: 46px;
            width: 30px;
            margin-left: 80px;
        }
        .auto-style5 {
            height: 28px;
            width: 30px;
        }
        .auto-style6 {
            width: 30px;
        }
        .auto-style7 {
            height: 25px;
        }
        .auto-style8 {
            width: 30px;
            height: 25px;
        }
        .auto-style9 {
            width: 30px;
            height: 46px;
        }
        .auto-style10 {
            height: 42px;
        }
        .auto-style11 {
            width: 30px;
            height: 42px;
        }
    </style>

    <script type="text/javascript">
        function validatepwd() {

            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;

            var errmsg = "Password must have at least:";
            var errcount = 0

            if (str.length < 12) {
                errmsg += " 12 characters";
                errcount += 1
            }

            if (str.search(/[0-9]/) == -1) {

                if (errcount > 0) {
                    errmsg += ","
                }
                errmsg += " 1 number";
                errcount += 1

            }

            if (str.search(/[A-Z]/) == -1) {
                if (errcount > 0) {
                    errmsg += ","
                }
                errmsg += " 1 uppercase letter";
                errcount += 1

            }

            if (str.search(/[a-z]/) == -1) {
                if (errcount > 0) {
                    errmsg += ","
                }
                errmsg += " 1 lowercase letter";
                errcount += 1

            }

            if (str.search(/[^A-Za-z0-9]/) == -1) {
                if (errcount > 0) {
                    errmsg += ","
                }
                errmsg += " 1 special character";
                errcount += 1

            }

            if (errcount > 0) {
                errmsg += "."
                document.getElementById("lb_pwdError").innerHTML = errmsg;
                document.getElementById("lb_pwdError").style.color = "Red";
            }
            else {
                document.getElementById("lb_pwdError").innerHTML = "Excellent!";
                document.getElementById("lb_pwdError").style.color = "Blue";

            }   
        }

        function validateccno() {
            var str = document.getElementById('<%=tb_ccNo.ClientID %>').value;

            if (str.search(/[0-9]{16,16}/) != 0 || str.length != 16) {
                
                document.getElementById("lb_ccerror").innerHTML = "Invalid credit card number,";
                document.getElementById("lb_ccerror").style.color = "Red";
            }
            else {
                document.getElementById("lb_ccerror").innerHTML = "";

            }
        }

        function validatecvv() {
            var str = document.getElementById('<%=tb_cvv.ClientID %>').value;

            if (str.search(/[0-9]{3,3}/) != 0 || str.length != 3) {
               
                    document.getElementById("lb_cvv").innerHTML = "Invalid cvv";
                    document.getElementById("lb_cvv").style.color = "Red";

                
            }
            else {
                document.getElementById("lb_cvv").innerHTML = "";

            }
        }

        function validateexp() {
            var str = document.getElementById('<%=tb_exp.ClientID %>').value;

            if (str.search(/[0-9]{4,4}/) != 0 || str.length != 4) {

                document.getElementById("lb_exp").innerHTML = "Invalid expiry date";
                document.getElementById("lb_exp").style.color = "Red"
                
            }
            else if (parseInt(str.substring(0,2) ) > 12) {
                document.getElementById("lb_exp").innerHTML = "Invalid expiry date ";
                document.getElementById("lb_exp").style.color = "Red"
            }

            else {
                document.getElementById("lb_exp").innerHTML = "";

            }
        }

        function validateemail() {
            var str = document.getElementById('<%=tb_email.ClientID %>').value;

            if (str.search(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i) != 0) {
                document.getElementById("lb_email").innerHTML = "Invalid email";
                document.getElementById("lb_email").style.color = "Red";

            }
            else {
                document.getElementById("lb_email").innerHTML = "";

            }
        }



    </script>
</head>
<body>
        <form id="form1" runat="server">
    <div>
    
    <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Account Registration"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">
            <tr>
                <td class="auto-style1">
        <asp:Label ID="Label2" runat="server" Text="First Name"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="tb_fname" runat="server" Height="36px" Width="280px"></asp:TextBox>
                </td>
                <td class="auto-style4">
                    &nbsp;</td>
                <td class="auto-style2">
        <asp:Label ID="Label3" runat="server" Text="Last Name"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="tb_lname" runat="server" Height="36px" Width="280px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">
        <asp:Label ID="Label5" runat="server" Text="Email"></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_email" runat="server" Height="36px" Width="280px" onkeyup="javascript:validateemail()"></asp:TextBox>
                </td>
                <td class="auto-style5">
                    &nbsp;</td>
                <td class="auto-style3">
                    &nbsp;</td>
                <td class="auto-style3">
        <asp:Label ID="lb_email" runat="server"></asp:Label>
                            </td>
            </tr>
            <tr>
                <td class="auto-style3">
                    </td>
                <td class="auto-style3">
                    </td>
                <td class="auto-style5">
                    </td>
                <td class="auto-style3">
                </td>
                <td class="auto-style3">
                    </td>
            </tr>
                        <tr>
                <td class="auto-style1">
        <asp:Label ID="Label4" runat="server" Text="Credit Card No. "></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="tb_ccNo" runat="server" Height="36px" Width="280px" onkeyup="javascript:validateccno()"  ></asp:TextBox>
                </td>
                <td class="auto-style9">
                </td>
                <td class="auto-style1">
                </td>
                <td class="auto-style1">
        <asp:Label ID="lb_ccerror" runat="server"></asp:Label>
                            </td>
            </tr>
                        <tr>
                <td class="auto-style7">
        <asp:Label ID="Label10" runat="server" Text="Expiry Date"></asp:Label>
                            &nbsp;(MMYY)</td>
                <td class="auto-style7">
                    <asp:TextBox ID="tb_exp" runat="server" Height="36px" Width="280px" ToolTip="MM/YY"  onkeyup="javascript:validateexp()"></asp:TextBox>
                            </td>
                <td class="auto-style8">
                            </td>
                <td class="auto-style7">
        <asp:Label ID="Label9" runat="server" Text="CVV"></asp:Label>
                            </td>
                <td class="auto-style7">
                    <asp:TextBox ID="tb_cvv" runat="server" Height="36px" Width="280px"  onkeyup="javascript:validatecvv()"></asp:TextBox>
                            </td>
            </tr>
                        <tr>
                <td class="style3">
                    &nbsp;</td>
                <td class="style2">
        <asp:Label ID="lb_exp" runat="server"></asp:Label>
                            </td>
                <td class="auto-style6">
                    &nbsp;</td>
                <td class="style2">
                    &nbsp;</td>
                <td class="style2">
        <asp:Label ID="lb_cvv" runat="server"></asp:Label>
                            </td>
            </tr>
                        <tr>
                <td class="style3">
        <asp:Label ID="Label8" runat="server" Text="Password"></asp:Label>
                    &nbsp;&nbsp;</td>
                <td class="style2">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="36px" Width="280px" onkeyup="javascript:validatepwd()" TextMode="Password" ></asp:TextBox>
                </td>
                <td class="auto-style6">
                    &nbsp;</td>
                <td class="style2">
                    &nbsp;</td>
                <td class="style2">
        <asp:Label ID="lb_pwdError" runat="server"></asp:Label>
                            </td>
            </tr>
                        <tr>
                <td class="auto-style10">
       
        <asp:Label ID="DoB" runat="server" Text="Date of Birth"></asp:Label>
       
                </td>
                <td class="auto-style10">
                    <asp:TextBox ID="tb_dob" runat="server" Height="32px" Width="281px" type='date'></asp:TextBox>
                            </td>
                <td class="auto-style11">
                            </td>
                <td class="auto-style10">
                            </td>
                <td class="auto-style10">
                            </td>
            </tr>
                        <tr>
                <td class="style4">
       
        <asp:Label ID="DoB0" runat="server" Text="Photo"></asp:Label>
       
                            </td>
                <td class="style5">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
                <td class="style5">
                    &nbsp;</td>
                <td class="style5">
                    &nbsp;</td>
            </tr>
                        <tr>
                <td class="style4">
       
                    &nbsp;</td>
                <td class="style5">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
                <td class="style5">
                    &nbsp;</td>
                <td class="style5">
    <asp:Button ID="btn_Submit" runat="server" Height="48px" 
        onclick="btn_Submit_Click" Text="Submit" Width="288px" />
                </td>
            </tr>
    </table>
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
&nbsp;<br />
        <br />
        <asp:Label ID="lb_error" runat="server"></asp:Label>
    <br />
        <br />
    
    </div>
    </form>

    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Lfyc24eAAAAAJ_ZN_WUOnICOH0eHs9857T2lgc-', { action: 'Register' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
