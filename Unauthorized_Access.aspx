<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Unauthorized_Access.aspx.cs" Inherits="E_Granthalaya.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="assets/css/styles.css" rel="stylesheet" />
    <script src="assets/js/bootstrap-5.2.3.bundle.min.js"></script>
    <script src="assets/js/fontawesome-all.js"></script>
    <script src="assets/js/scripts.js"></script>
    <title>Unauthorized Access</title>
</head>
<body class="sb-nav-fixed">
        <nav class="sb-topnav navbar navbar-expand navbar-dark col-md" style="background-color: #000c1f">
            <button class="btn btn-link btn-sm order-1 order-lg-0 me-4 me-lg-0" id="sidebarToggle" href="#!"><i class="fas fa-bars"></i></button>
            <a class="navbar-brand ps-3" href="Adm_Home.aspx" style="font-weight:bold">E-Granthalaya</a>
            <div class="d-none d-md-inline-block form-inline ms-auto me-0 me-md-3 my-2 my-md-0">
            </div>
            <div class="navbar-nav ms-auto ms-md-0 me-3 me-lg-4" style="font-weight:700">
                    <a class="nav-link" id="navbarDropdown" href="Login.aspx" role="button" aria-expanded="false">
                        <i class="fa fa-user fa-fw me-1"></i>Login
                    </a>
            </div>
        </nav>
        <div class=" card p-2 container d-flex justify-content-center align-items-center" style="margin-top:100px; font-weight:bold; font-size:2rem">
            <div class="mb-2">Sorry, Unauthorized Access or the Page might be searching for is invalid...!
            </div>
            <a class="btn btn-primary" style="font-weight:bold;background-color:#003da3" href="Login.aspx">Back To Login ➲</a>
    </div>
</body>
</html>