<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="E_Granthalaya.Login" %>

<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link rel="shortcut icon" href="assets/images/logo.jpg" type="image/png" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>Login - E-Granthalaya v.1.0</title>
    <!-- Bootstrap core CSS-->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/2.0.2/js/toastr.min.js"></script>
    <style>
        body {
            background-image: url(assets/images/bgimg-library.jpg);
            height: 100vh;
            background-attachment: fixed;
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
        }

        .g-book-counts {
            position: absolute;
            bottom: 0;
            z-index: 1;
            background: rgba(0, 0, 0, .4);
            color: #FFF;
            right: 0;
            padding: 10px;
            text-align: right;
        }
    </style>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
    <!-- custom fonts for login-->
    <link href="assets/css/custom.css" rel="stylesheet" />
    <script src="assets/js/sweetalert.js"></script>
</head>
<body>
    <nav class="sb-topnav navbar justify-content-between navbar-expand navbar-dark col-md">
        <a class="navbar-brand ps-3" href="#" style="font-weight: bold">SIA E-Granthalaya</a>
        <div class="d-none d-md-inline-block form-inline ms-auto me-0 me-md-3 my-2 my-md-0">
        </div>
        <div class="navbar-nav ms-auto ms-md-0 me-3 me-lg-4" style="font-weight: 700">
            <a class="btn btn-sm" href="OPAC.aspx" style="font-weight: bold; background-color: #003da3; color: white" role="button" aria-expanded="false">View All Books : OPAC
            </a>
        </div>
    </nav>
    <div class="container" style="font-weight: 600">
        <div class="card card-login mx-auto mt-3">
            <div class="card-header d-flex align-items-center justify-content-center" style="font-weight: 700">Member Login</div>
            <div class="card-body">
                <form runat="server" id="formlogin">
                    <asp:Panel ID="panel_warning" runat="server" Visible="false">
                        <div class="form-group card-header text-center">
                            <div class="alert-danger">
                                <asp:Label ID="lbl_warning" runat="server" Text="Label" CssClass="col-form-label text-center"></asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="form-group">
                        <label for="exampleInputEmail1">Enter Username : <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txt_user" runat="server" CssClass="form-control" AutoComplete="off" Style="font-weight: 600" placeholder="RollNo@Dept"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqr_user" runat="server" ErrorMessage="Enter Username" ControlToValidate="txt_user" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Button ID="btn_send" OnClick="btn_send_Click" runat="server" BackColor="#003da3" Font-Bold="true" Text="Send OTP" CssClass="btn btn-primary btn-block" />
                    <div style="display: flex; justify-content: center; align-items: center; font-weight: bold; color: black; margin-top: 4px">EGranthalaya v.1.0</div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </form>
            </div>
        </div>
    </div>
    <div class="g-book-counts" style="font-weight: 600">
        <span>Granthalaya</span>
        <br />
        Total Books :
        <asp:Label ID="lbl_books" runat="server" Text="0"></asp:Label>
        | 
        <span class="">Total Unique Books:
            <asp:Label ID="lbl_unique" runat="server" Text="0"></asp:Label></span>
        <br />
    </div>
</body>
</html>
