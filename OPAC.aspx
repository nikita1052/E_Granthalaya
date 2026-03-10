<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OPAC.aspx.cs" Inherits="E_Granthalaya.OPAC" %>

<DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OPAC</title>
    <link href="assets/css/styles.css" rel="stylesheet" />
    <script src="assets/js/bootstrap-5.2.3.bundle.min.js"></script>
    <script src="assets/js/fontawesome-all.js"></script>
    <script src="assets/js/scripts.js"></script>
    <script src="assets/js/sweetalert.js"></script>
    <%--<script src="assets/js/jquery.numeric.js"></script>--%>

    <%--Toaster Notification--%>
    <link href="assets/css/toastr.min.css" rel="stylesheet" />
    <script src="assets/js/jquery-2.1.1.min.js"></script>
    <script src="assets/js/toastr-2.0.2.min.js"></script>
    <%--End Toaster Notification--%>
    <%--DataTable--%>
    <link href="assets/css/dataTables-2.0.5.bootstrap5.css" rel="stylesheet" />
    <script src="assets/js/dataTables-2.0.5.js"></script>
    <script src="assets/js/dataTables.bootstrap5-2.0.5.js"></script>
    <%--DataTable End--%>
    <script type="text/javascript">
        $(document).ready(function () {
            reinitDataTable();
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function () { reinitDataTable(); })
            }
            else {
                console.log("prm is null");
            }
        });

        function reinitDataTable() {
            $('.dataTable').prepend($("<thead></thead>").append($('.dataTable').find("tr:first"))).DataTable({
                bLengthChange: true,
                lengthMenu: [[10, 20, -1], [10, 20, "All"]],
                bFilter: true,
                bSort: true,
                bPaginate: true
            });
        }
        const searchInput = document.getElementById('searchInput');
        const checkboxes = document.querySelectorAll('.checkbox-item'); // Replace with your actual selector

        searchInput.addEventListener('input', () => {
            const query = searchInput.value.toLowerCase();
            checkboxes.forEach((checkbox) => {
                const label = checkbox.textContent.toLowerCase();
                checkbox.style.display = label.includes(query) ? 'block' : 'none';
            });
        });

    </script>
    <script type='text/javascript'>
        function ShowPopup() {
            $("#bookdets").modal("show");
        }
        function closeModal() {
            $('#bookdets').modal('hide');
            $('.modal-backdrop').hide();
        }
        function openModal() {
            $('#filter').modal('show');
        }
    </script>
    <style>
        .scroll::-webkit-scrollbar {
            width: 0 !important
        }

        #gridview_books table th {
            color: white !important;
        }

        #gridview_books_wrapper .justify-content-between {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

        #gridview_books_wrapper .justify-content-md-center {
            margin-top: 0px !important;
        }

        #gridview_books thead th {
            text-align: center;
        }

        #gridview_books tbody td {
            text-align: center;
            font-size: 15px;
            font-weight: 500;
        }

        .checkbox-container {
            border: 1px solid #ccc;
            padding: 10px;
            background-color: #f9f9f9;
            border-radius: 5px
        }

        .checkbox-item {
            display: flex;
            align-items: center;
            margin-bottom: 8px;
        }

        label {
            font-size: 16px;
            color: #333;
            margin-left: 10px;
        }

        .btn-opac {
            color: #00235e;
            background-color: white;
        }

            .btn-opac:hover {
                background-color: white;
                color: #00235e;
            }

        .horizontal-list {
            display: flex;
            flex-wrap: wrap;
            list-style: none;
            padding: 0;
        }

            .horizontal-list li {
                list-style: none;
                margin-right: 10px;
            }
    </style>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="sb-topnav navbar navbar-expand navbar-dark" style="background-color: #000c1f">
            <div class="col-md-11 d-flex justify-content-between">
                <div class="offset-1" style="font-weight: bold; margin-top: 4px; color: white; font-size: 1.3rem">SIA E-Granthalaya</div>
                <div class="small" style="margin-top: 4px">
                    <asp:Button ID="btn_login" CssClass="btn btn-sm btn-success" Font-Bold="true" OnClick="btn_login_Click" BackColor="Green" runat="server" Text="Back to Main Page" />
                </div>
            </div>
        </nav>
        <main>
            <div class="container-fluid py-2 px-5">
                <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
                <div class="modal fade" tabindex="-1" id="filter" role="dialog">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <div class="col-md" style="align-items: center; justify-content: center; display: flex; font-weight: bold">Search By Filter</div>
                                <button type="button" class="btn-close" style="font-weight: bold" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <div class="container-fluid">
                                    <asp:UpdatePanel Style="font-weight: 700" ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <div class="card">
                                                <div class="card-body">
                                                    <div class="btn btn-group px-0 py-0" style="align-items: center; justify-content: center; display: flex; border: 1px solid #00235e">
                                                        <asp:Button ID="btn_category" CssClass="btn btn-sm btn-opac" Style="font-weight: 600;" runat="server" OnClick="btn_category_Click" Text="Category" />
                                                        <asp:Button ID="btn_course" CssClass="btn btn-sm btn-opac" Style="font-weight: 600;" runat="server" OnClick="btn_course_Click" Text="Stream" />
                                                        <asp:Button ID="btn_subject" CssClass="btn btn-sm btn-opac" Style="font-weight: 600;" runat="server" OnClick="btn_subject_Click" Text="Subject" />
                                                        <asp:Button ID="btn_publisher" CssClass="btn btn-sm btn-opac" Style="font-weight: 600;" runat="server" OnClick="btn_publisher_Click" Text="Publisher" />
                                                        <asp:Button ID="btn_author" CssClass="btn btn-sm btn-opac" Style="font-weight: 600;" runat="server" OnClick="btn_author_Click" Text="Author" />
                                                    </div>
                                                    <div class="mb-2"></div>
                                                    <asp:Panel ID="pnl_category" runat="server" Visible="false">
                                                        <input type="text" id="searchInput" placeholder="Search Categort">
                                                        <div class="horizontal-list checkbox-container" style="max-height: 300px; overflow-y: auto;">
                                                            <asp:CheckBoxList ID="cbl_category" CssClass="form-check" runat="server"></asp:CheckBoxList>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnl_course" runat="server" Visible="false">
                                                        <div class="horizontal-list checkbox-container" style="max-height: 300px; overflow-y: auto;">
                                                            <asp:CheckBoxList ID="cbl_course" CssClass="form-check" runat="server"></asp:CheckBoxList>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnl_subject" runat="server" Visible="false">
                                                        <div class="horizontal-list checkbox-container" style="max-height: 300px; overflow-y: auto;">
                                                            <asp:CheckBoxList ID="cbl_subject" CssClass="form-check" runat="server"></asp:CheckBoxList>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnl_publisher" runat="server" Visible="false">
                                                        <div class="horizontal-list checkbox-container" style="max-height: 300px; overflow-y: auto;">
                                                            <asp:CheckBoxList ID="cbl_publisher" CssClass="form-check" runat="server"></asp:CheckBoxList>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnl_author" runat="server" Visible="false">
                                                        <div class="horizontal-list checkbox-container" style="max-height: 300px; overflow-y: auto;">
                                                            <asp:CheckBoxList ID="cbl_author" CssClass="form-check" runat="server"></asp:CheckBoxList>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btn_savefilters" BackColor="#003da3" Font-Bold="true" runat="server" Text="Apply Filter" OnClick="btn_savefilters_Click" CausesValidation="false" CssClass="btn btn-sm btn-primary" />
                                <button type="button" class="btn btn-sm btn-dark" style="font-weight: bold" data-bs-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal" tabindex="-1" id="bookdets" role="dialog">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">Book Details</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <div class="container-fluid">
                                    <%--<div style="text-align: center; margin-top: 0;">
                                                <img id="bookImage" src="assets/images/bgimg-library.jpg" alt="Book Image" style="width: 100px; height: 120px; border: 10px solid #87CEEB;" />
                                            </div>--%>
                                    <%--<div class="form-group mb-1">
                                                <asp:Label ID="lblstudent" runat="server" Font-Bold="true" Text="Book ID :- "></asp:Label>
                                                <asp:Label ID="lbl_bookid" runat="server" Text=""></asp:Label>
                                            </div>--%>
                                    <div class="form-group mb-1">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Book Name : "></asp:Label>
                                        <asp:Label ID="lbl_bookname" runat="server" Text="" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="form-group mb-1">
                                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Publisher Name : "></asp:Label>
                                        <asp:Label ID="lbl_publisher" runat="server" Text="" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="form-group mb-1">
                                        <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Author : "></asp:Label>
                                        <asp:Label ID="lbl_author" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="form-group mb-1">
                                        <asp:Label ID="Label5" runat="server" Font-Bold="true" Text="Course : "></asp:Label>
                                        <asp:Label ID="lbl_course" runat="server" Text="" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="form-group mb-1">
                                        <asp:Label ID="Label6" runat="server" Font-Bold="true" Text="Subject : "></asp:Label>
                                        <asp:Label ID="lbl_subject" runat="server" Text="" CssClass="form-label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
                <%-------------------- MAIN PAGE ----------------------%>
                <asp:UpdatePanel ID="update1" runat="server">
                    <ContentTemplate>
                        <div class="card">
                            <div class="card-body">
                                <div class="row form-group justify-content-between">
                                    <div class="col-md-4">
                                        <h5 style="font-weight: bold">Opac - Book Details</h5>
                                    </div>
                                    <div class="col-md-auto">
                                        <%--                                        <asp:Button ID="btn_filters" runat="server" Text="Search By Filter" CssClass="btn btn-sm btn-primary" Style="margin-right: 20px; margin-top: -4px" Font-Bold="true" BackColor="#ccffff" ForeColor="black" OnClick="btn_filters_Click" />--%>
                                    </div>
                                    <hr />
                                </div>
                                <%--  ---------------- GRIDVIEW ---------------------%>
                                <asp:Panel ID="pnl_gridview" runat="server">
                                    <div class="card p-2 scroll table-responsive">
                                        <asp:GridView ID="gridview_books" runat="server" CssClass="table table-bordered dataTable" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Book Photo">
                                                    <ItemTemplate>
                                                        <%--<asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageURL(Eval("BOOK_PHOTO")) %>' AlternateText='<%# Eval("BOOK_PHOTO") %>' Style="width: 30px; height: 30px; border-radius: 10px; border: 1px solid black;" />--%>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/assets/images/defaultbook.jpg" Style="width: 30px; height: 30px; " />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Book Photo">
                                                     <ItemTemplate>
                                                       <%--<asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageURL(Eval("BOOK_PHOTO")) %>' AlternateText='<%# Eval("BOOK_PHOTO") %>' Style="width: 30px; height: 30px; border-radius: 10px; border: 1px solid black;" />--%>
                                                <%--<asp:Image ID="Image1" runat="server" ImageUrl='<%# "ImageHandler.ashx?linkedwith=" + Eval("linkedwith") %>' Style="width: 30px; height: 30px; border-radius: 10px; border: 1px solid black;" />
                                                    </ItemTemplate>--%>
                                                <%--  </asp:TemplateField>--%>
                                                <asp:BoundField DataField="BOOKNAME" HeaderText="Book Title" />
                                                <asp:BoundField DataField="PUBLISHERNAME" HeaderText="Publisher Name" />
                                                <asp:BoundField DataField="AUTHOR" HeaderText="Author" />
                                                <asp:BoundField DataField="COURSENAME" HeaderText="Course" />
                                                <%--<asp:BoundField DataField="SUBJECT_NAME" HeaderText="Subject" />--%>
                                                <%--<asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btn_view" CssClass="btn btn-sm small" Style="background-color: #003da3; color: white; font-weight: 600" runat="server" Text="View" ToolTip="View record" CommandName="View" OnClick="btn_view_Click" CausesValidation="False"> </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnl_nodata" runat="server" Visible="false">
                                    <div class="container-fluid py-0 px-0">
                                        <div class="card p-2">
                                            <div class="d-flex justify-content-center">
                                                <svg width="64" height="41" viewBox="0 0 64 41" xmlns="http://www.w3.org/2000/svg">
                                                    <g transform="translate(0 1)" fill="none" fill-rule="evenodd">
                                                        <ellipse fill="#f5f5f5" cx="32" cy="33" rx="32" ry="7"></ellipse>
                                                        <g fill-rule="nonzero" stroke="#d9d9d9">
                                                            <path d="M55 12.76L44.854 1.258C44.367.474 43.656 0 42.907 0H21.093c-.749 0-1.46.474-1.947 1.257L9 12.761V22h46v-9.24z"></path>
                                                            <path d="M41.613 15.931c0-1.605.994-2.93 2.227-2.931H55v18.137C55 33.26 53.68 35 52.05 35h-40.1C10.32 35 9 33.259 9 31.137V13h11.16c1.233 0 2.227 1.323 2.227 2.928v.022c0 1.605 1.005 2.901 2.237 2.901h14.752c1.232 0 2.237-1.308 2.237-2.913v-.007z" fill="#fafafa"></path>
                                                        </g>
                                                    </g>
                                                </svg>
                                            </div>
                                            <asp:Label ID="lbl_not" runat="server" class="d-flex justify-content-center" Style="font-weight: 700; font-size: 1.2rem" Text="No Data Found"></asp:Label>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </main>
        <footer class="py-2 mt-auto" style="background-color: #bcc8de; font-weight: 700">
            <div class="container-fluid px-4">
                <div class="d-flex align-items-center justify-content-between small">
                    <div class="text text-muted">Copyright &copy; E-Granthalaya 2023</div>
                    <div class="text text-muted">
                        <a href="#">Privacy Policy</a>
                        &middot;<a href="#">Terms &amp; Conditions</a>
                    </div>
                </div>
            </div>
        </footer>
    </form>
</body>
</html>
