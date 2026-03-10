<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_AddBook.aspx.cs" Inherits="E_Granthalaya.Adm_AddBook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    </script>
    <script>
        function showAlert() {
            Swal(
                'Good job!',
                'New Company Successfully Added!',
                'success'
            );
        }
        function closeModal(modalID) {
            $('[id*=' + modalID + ']').modal('hide');
            $('.modal-backdrop').hide();
        }
        function Showmodal(bookId) {
            var imagepath = 'ImageHandler.ashx?linkedwith=' + bookId;
            // Update the src attribute of the image with the imagePath
            $('#bookImage').attr('src', imagepath);
            $("#imageModal").modal("show");
        }
        function close_Modal() {
            $('#imageModal').modal('hide');
            $('.modal-backdrop').hide();
        }
        function closemodal() {
            $('#booktype').modal('hide');
            $('.modal-backdrop').hide();
        }
        function hideLoadingScreen() {
            document.getElementById("loadingScreen").style.display = "none";
        }
        function showLoadingScreen() {
            document.getElementById("loadingScreen").style.display = "block";
        }

    </script>
    <style>
        .scroll::-webkit-scrollbar {
            width: 0 !important
        }

        #ContentPlaceHolder1_YourGridView table th {
            color: white !important;
        }

        #ContentPlaceHolder1_YourGridView_wrapper .justify-content-between {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

        #ContentPlaceHolder1_YourGridView_wrapper .justify-content-md-center {
            margin-top: 0px !important;
        }

        #ContentPlaceHolder1_YourGridView thead th {
            text-align: center;
            font-size: 15px;
        }

        #ContentPlaceHolder1_YourGridView tbody td {
            text-align: center;
            font-size: 14px;
            font-weight: 500;
        }

        .circlebtn {
            margin-top: 4px;
            font-weight: 900;
            border-radius: 100px;
            padding-top: 0px;
            padding-bottom: 2px;
            font-size: 1.1rem
        }
    </style>
    <title>Add New Book</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid px-4">

        <h3 class="mt-2"></h3>
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Add New Book</li>
        </ol>
        <asp:Panel ID="panel_warning" runat="server" Visible="false">
            <div class="alert alert-danger text-center">
                <asp:Label ID="lbl_warning" runat="server" Text="Some error occured" />
            </div>
        </asp:Panel>

        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>

            <div class="modal fade" id="imageModal" tabindex="-1" role="dialog" aria-labelledby="imageModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" style="font-weight: bold" id="imageModalLabel">Book Image</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body d-flex justify-content-center align-items-center">
                            <img id="bookImage" src="" alt="Book Image" class="img-fluid" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="publisher" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" style="font-weight: bold" id="exampleModalLabel">Add New Publisher</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body" style="font-weight: 500">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <div class="row form-group">
                                        <label class="col-md-4 col-form-label">Publisher Name<span class="text-danger">*</span></label>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="txt_add_publisher" Style="font-weight: 500" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button3" BackColor="#003da3" Font-Bold="true" class="btn btn-primary" runat="server" Text="Save changes" OnClick="btn_add_publisher_Click" CausesValidation="False" />
                            <button type="button" style="font-weight: bold" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal" tabindex="-1" id="booktype" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" style="font-weight: bold">Add New Book Type</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <div class="row form-group" style="font-weight: 500">
                                        <label class="col-md-3 col-form-label">Book Type<span class="text-danger">*</span></label>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="txt_add_booktype" Style="font-weight: 500" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button2" BackColor="#003da3" Font-Bold="true" class="btn btn-primary" runat="server" Text="Save changes" OnClick="btn_add_booktype_Click" CausesValidation="False" />
                            <button type="button" style="font-weight: bold" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="exampleModal1" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h1 class="modal-title fs-5" style="font-weight: bold" id="exampleModalLabel1">Add New Author</h1>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body" style="font-weight: 500">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <div class="row form-group">
                                        <label class="col-md-4 col-form-label">Author Name<span class="text-danger">*</span></label>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="txt_add_author" Style="font-weight: 500" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btn_add_author" BackColor="#003da3" Font-Bold="true" class="btn btn-primary" runat="server" Text="Save changes" OnClick="btn_add_author_Click" CausesValidation="False" />
                            <button type="button" style="font-weight: bold" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal" id="exmodal" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" style="font-weight: bold">Add New Medium</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body" style="font-weight: 500">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="row form-group">
                                        <label class="col-md-2 col-form-label">Medium<span class="text-danger">*</span></label>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="txt_add_medium" Style="font-weight: 500" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button1" BackColor="#003da3" Font-Bold="true" class="btn btn-primary" runat="server" Text="Save changes" OnClick="btn_add_medium_Click" CausesValidation="False" />
                            <button type="button" style="font-weight: bold" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal" id="mdl_vendor" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" style="font-weight: bold">Add New Vendor</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body" style="font-weight: 500">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row form-group mb-2">
                                        <label class="col-md-5 col-form-label">Vendor Name<span class="text-danger">*</span></label>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txt_VendorName" Style="font-weight: 500" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Enter vendor name here" Display="Dynamic" ControlToValidate="txt_VendorName" ForeColor="red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row form-group mb-2">
                                        <label class="col-md-5 col-form-label">Name of Person<span class="text-danger">*</span></label>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txt_NameOfPerson" Style="font-weight: 500" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Enter Person Name here" Display="Dynamic" ControlToValidate="txt_NameOfPerson" ForeColor="red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row form-group mb-2">
                                        <label class="col-md-5 col-form-label">Contact No<span class="text-danger">*</span></label>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txt_ContactNo" Style="font-weight: 500" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Enter Contact Number here" Display="Dynamic" ControlToValidate="txt_ContactNo" ForeColor="red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row form-group mb-2">
                                        <label class="col-md-5 col-form-label">Mail Address<span class="text-danger">*</span></label>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txt_MailAddress" TextMode="Email" Style="font-weight: 500" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Enter Mail Address here" Display="Dynamic" ControlToValidate="txt_MailAddress" ForeColor="red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="ButtonSave" BackColor="#003da3" Font-Bold="true" CssClass="btn btn-primary" runat="server" Text="Save changes" OnClick="btn_add_vendor_Click" CausesValidation="False" />
                            <button type="button" style="font-weight: bold" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal" id="bookmoneydtl" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" style="font-weight: bold">Edit Book Details</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body" style="font-weight: 500; width: 70%;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="row form-group mb-2">
                                        <label class="col-md-2 col-form-label">Cost Per Book : <span class="text-danger">*</span></label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txt_mdl_cost" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" placeholder="Amount" TextMode="Number"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Enter Total Cost " Display="Dynamic" ControlToValidate="txt_mdl_cost" ForeColor="red"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txt_mdl_cost" MinimumValue="0" Type="Double" ErrorMessage="Cost must be a positive number." ForeColor="red" Display="Dynamic"></asp:RangeValidator>
                                        </div>

                                        <label class="col-md-2 col-form-label">Discount Per Book : <span class="text-danger">*</span></label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txt_mdl_disc" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" placeholder="Percentage %" TextMode="Number"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Enter Total Discount" Display="Dynamic" ControlToValidate="txt_mdl_disc" ForeColor="red"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txt_mdl_disc" MinimumValue="0" MaximumValue="100" Type="Double" ErrorMessage="Discount must be between 0 and 100 percent." ForeColor="red" Display="Dynamic"></asp:RangeValidator>
                                        </div>
                                    </div>
                                    <div class="row form-group mb-2">
                                        <label class="col-md-2 col-form-label">No of Copies: <span class="text-danger">*</span></label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txt_mdl_nocpy" Style="font-weight: 500" runat="server" AutoComplete="off" placeholder="Copies" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="No of Copies" Display="Dynamic" ControlToValidate="txt_mdl_nocpy" ForeColor="red"></asp:RequiredFieldValidator>
                                        </div>
                                        <label class="col-md-2 col-form-label">Upload Book Image : </label>
                                        <div class="col-md-3">
                                            <asp:FileUpload Style="font-weight: 500" ID="fileupload1" CssClass="form-control" runat="server" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btn_editbook" class="btn btn-primary" BackColor="#003da3" Font-Bold="true" runat="server" Text="Save" CausesValidation="False" />
                            <%--<asp:Button ID="btn_SaveMoneyDetails" runat="server" Text="Save" CausesValidation="false" CssClass="btn btn-primary" OnClick="btn_SaveMoneyDetails_Click" />--%>
                            <button type="button" class="btn btn-secondary" style="font-weight: bold" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="update1" runat="server">

                <ContentTemplate>

                    <!-- Modal -->
                    <div class="card">
                        <div class="card-body" style="font-weight: 500">
                            <div class="row form-group">
                                <h5>Book Details</h5>
                                <hr style="border: 1px solid black" />
                            </div>
                            <div class="row form-group mb-2">
                                <label class="col-md-2 col-form-label">Book Type : <span class="text-danger">*</span></label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddl_booktype" Style="font-weight: 500" CssClass="form-control form-select" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_booktype_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Book Type" Display="Dynamic" ControlToValidate="ddl_booktype" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-1">
                                    <asp:Button ID="btn_add_book_type" BackColor="#003da3" align="center" Font-Bold="true" runat="server" Text="+" CssClass="btn circlebtn btn-sm  btn-primary" type="button" data-bs-toggle="modal" data-bs-target="#booktype" CausesValidation="False" />
                                </div>

                                <label class="col-md-2 col-form-label">Book Source : <span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddl_book_source" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-select"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Select Book source" Display="Dynamic" ControlToValidate="ddl_book_source" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row form-group mb-2">
                                <label class="col-md-2 col-form-label">Book Name : <span class="text-danger">*</span></label>
                                <div class="col-md-5">
                                    <asp:TextBox ID="txt_book_name" Style="font-weight: 500" placeholder="Enter Book Name" runat="server" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter Book Name" Display="Dynamic" ControlToValidate="txt_book_name" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                                <label class="col-md-2 col-form-label">Select Medium : <span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddl_medium" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-select"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Select Medium" Display="Dynamic" ControlToValidate="ddl_medium" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Button ID="add_btn_ddl_medium" BackColor="#003da3" align="center" Font-Bold="true" runat="server" Text="+" CssClass="btn circlebtn btn-sm  btn-primary" data-bs-toggle="modal" data-bs-target="#exmodal" CausesValidation="false" />
                                </div>
                            </div>

                            <asp:Panel ID="coursesem" runat="server" Visible="false">
                                <div class="row form-group mb-2">
                                    <asp:Label ID="Label2" runat="server" class="col-md-2 col-form-label" Text="Label">Select Course : <span class="text-danger">*</span></asp:Label>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddl_course" Style="font-weight: 500" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddl_course_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <asp:Label ID="Label3" runat="server" class="col-md-auto col-form-label" Text="Label">Sem : <span class="text-danger">*</span></asp:Label>
                                    <div class="col-md-auto">
                                        <asp:DropDownList ID="ddl_Semester" Style="font-weight: 500" runat="server" CssClass="form-select" AutoPostBack="true" TextMode="Email" OnSelectedIndexChanged="ddl_Semester_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <asp:Label ID="Label4" runat="server" class="col-md-1 col-form-label" Text="Label">Subject : <span class="text-danger">*</span></asp:Label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddl_subject" Style="font-weight: 500" runat="server" CssClass="form-select"></asp:DropDownList>
                                    </div>
                                </div>
                            </asp:Panel>

                            <div class="row form-group mb-2">
                                <label class="col-md-2 col-form-label">Select Author : <span class="text-danger">*</span></label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddl_author" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-select"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Select Author" Display="Dynamic" ControlToValidate="ddl_author" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Button ID="add_btn_ddl_author" align="center" BackColor="#003da3" Font-Bold="true" data-bs-toggle="modal" data-bs-target="#exampleModal1" runat="server" Text="+" CssClass="btn  circlebtn btn-sm btn-primary" CausesValidation="false" />
                                </div>
                                <label class="col-md-1 col-form-label">Publisher:<span class="text-danger">*</span></label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddl_publisher" Style="font-weight: 500" CssClass="form-select" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Select Publisher" Display="Dynamic" ControlToValidate="ddl_publisher" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Button class="btn circlebtn btn-sm btn-primary" align="center" BackColor="#003da3" Font-Bold="true" Text="+" ID="add_btn_ddl_publisher" type="button" runat="server" data-bs-toggle="modal" data-bs-target="#publisher" CausesValidation="false" />
                                </div>
                            </div>

                            <div class="row form-group mb-2">
                                <label class="col-md-2 col-form-label">No of Copies: <span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_no_cpy" Style="font-weight: 500" runat="server" AutoComplete="off" placeholder="Copies" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="No of Copies" Display="Dynamic" ControlToValidate="txt_no_cpy" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                                <label class="col-md-2 col-form-label">Upload Book Image : </label>
                                <div class="col-md-3">
                                    <asp:FileUpload Style="font-weight: 500" ID="fileupload" CssClass="form-control" runat="server" />
                                </div>

                                <%--<div class="col-md-2">
                                    <asp:Button ID="btn_showdetails" runat="server" BackColor="#003da3" ForeColor="White" Font-Bold="true" Text="Add Invoice Details" data-bs-toggle="modal" data-bs-target="#bookmoneydtl" CausesValidation="false" CssClass="btn btn-sm btn-primary" Style="margin-top: 4px" OnClick="btn_showdetails_Click" />
                                </div>--%>
                            </div>
                            <div class="row form-group mb-2">
                                <label class="col-md-2 col-form-label">Cost Per Book : <span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_Ttl_cost" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" placeholder="Amount" TextMode="Number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Enter Total Cost " Display="Dynamic" ControlToValidate="txt_Ttl_cost" ForeColor="red"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidatorCost" runat="server" ControlToValidate="txt_Ttl_cost" MinimumValue="0" Type="Double" ErrorMessage="Cost must be a positive number." ForeColor="red" Display="Dynamic"></asp:RangeValidator>
                                </div>

                                <label class="col-md-2 col-form-label">Discount Per Book : <span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_Ttl_discount" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" placeholder="Percentage %" TextMode="Number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Enter Total Discount" Display="Dynamic" ControlToValidate="txt_Ttl_discount" ForeColor="red"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidatorDiscount" runat="server" ControlToValidate="txt_Ttl_discount" MinimumValue="0" MaximumValue="100" Type="Double" ErrorMessage="Discount must be between 0 and 100 percent." ForeColor="red" Display="Dynamic"></asp:RangeValidator>
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btn_add" runat="server" BackColor="#003da3" ForeColor="White" Font-Bold="true" Text="Add Books" CausesValidation="false" CssClass="btn btn-sm btn-primary" Style="margin-top: 4px" OnClick="btn_add_Click" />
                                </div>
                            </div>
                            <asp:Panel ID="PNL_GRID" runat="server">
                                <div class="card p-2 table-responsive scroll">
                                    <asp:GridView ID="YourGridView" runat="server" DataKeyNames="BOOK_ID,LINKEDWITH,BOOKNAME,BILL_ID,SUBJECT_ID" CssClass="table table-bordered dataTable" AutoGeneratedColumns="false" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:BoundField DataField="BOOK_ID" HeaderText="Book ID" />--%>
                                            <asp:BoundField DataField="BOOKNAME" HeaderText="Book Name" />
                                            <asp:BoundField DataField="PUBLISHERNAME" HeaderText="Publisher Name" />
                                            <asp:BoundField DataField="ENTRYDATE" HeaderText=" Entry Date" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="LANGUAGE" HeaderText="Medium" />
                                            <asp:BoundField DataField="COSTRS" HeaderText="Cost " />
                                            <asp:BoundField DataField="DISCOUNT" HeaderText="Disc " />
                                            <asp:BoundField DataField="COURSE_NAME" HeaderText="Course Name" />
                                            <asp:BoundField DataField="NO_OF_COPY" HeaderText="Copy No." />
                                            <asp:BoundField DataField="BILL_ID" HeaderText="Bill ID" />
                                            <asp:BoundField DataField="SUBJECT_ID" HeaderText="SUB ID" />
                                            <asp:TemplateField HeaderText="Book Image">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="btn small btn-sm" BackColor="#2e2d2d" ForeColor="White" Font-Bold="true" ID="btn_view" OnClick="btn_view_Click" runat="server" CausesValidation="false" Text="View Image" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="SelectButton" runat="server" OnClick="SelectButton_Click" CssClass="table-link text-danger" CausesValidation="false" Text="Delete">
                                                        <span class="fa-stack">
                                                           <i class="fa fa-square fa-stack-2x"></i>
                                                           <i class="fa fa-trash fa-stack-1x fa-inverse"></i>
                                                        </span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="mb-2"></div>
                                <%--data-bs-toggle="modal" data-bs-target="#bookmoneydtl"--%>
                            </asp:Panel>
                            <asp:Panel ID="pnl_invoice" runat="server" Visible="false">
                                <div class="row form-group mb-2">
                                    <asp:Label ID="Label5" class="col-md-2 col-form-label" runat="server" Text="Vendor Name : "></asp:Label>
                                    <div class="col-md-3">
                                        <%--<asp:TextBox ID="txt_vendor" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddl_vendorname" Style="font-weight: 500" CssClass="form-select" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-auto">
                                        <asp:Button ID="Button5" runat="server" class="btn circlebtn btn-sm btn-primary" align="center" BackColor="#003da3" Font-Bold="true" Text="+" data-bs-toggle="modal" data-bs-target="#mdl_vendor" CausesValidation="false" />
                                    </div>
                                    <label class="col-md-auto col-form-label">Voucher No. </label>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txt_invoice_no" Style="font-weight: 500" runat="server" placeholder="Voucher Number" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <label class="col-md-auto col-form-label">Voucher Date </label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txt_date" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Enter Invoice date here" Display="Dynamic" ControlToValidate="txt_date" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row form-group mb-2">
                                    <label class="col-md-2 col-form-label">Total Book Cost : <span class="text-danger">*</span></label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txt_act_cst" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" placeholder="Total Cost" TextMode="Number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Enter Totl Amount" Display="Dynamic" ControlToValidate="txt_act_cst" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>

                                    <label class="col-md-auto col-form-label">Total Discounted (Amt) :<span class="text-danger">*</span></label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txt_act_disc" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" placeholder="Discounted Amount" TextMode="Number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Enter Totl Amount" Display="Dynamic" ControlToValidate="txt_act_disc" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>

                                    <label class="col-md-auto col-form-label">Net Amount :<span class="text-danger">*</span></label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txt_Ttl_amount" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" placeholder="Total Amount" TextMode="Number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Enter Totl Amount" Display="Dynamic" ControlToValidate="txt_Ttl_amount" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="row form-group mb-2">
                                <div class="col-md-6">
                                    <asp:Button ID="btn_savet" BackColor="Green" Font-Bold="true" runat="server" Text="Save Book Details" OnClick="Button1_Click" CausesValidation="false" CssClass="btn btn-sm btn-success" />
                                </div>
                            </div>
                            <asp:Panel ID="Panel1" runat="server">
                                <div class="card p-2 table-responsive scroll">
                                    <asp:GridView ID="GridView1" runat="server" DataKeyNames="BOOK_ID,LINKEDWITH,BOOKNAME,BILL_ID,SUBJECT_ID" CssClass="table table-bordered dataTable" AutoGeneratedColumns="false" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:BoundField DataField="BOOK_ID" HeaderText="Book ID" />--%>
                                            <asp:BoundField DataField="BILL_ID" HeaderText="Bill ID" />
                                            <asp:BoundField DataField="BOOK_ID_RANGE" HeaderText="Book Ids" />
                                            <asp:BoundField DataField="OV_ID_RANGE" HeaderText=" Accession Ids"  />
                                            <asp:BoundField DataField="BOOKNAME" HeaderText="Book Name" />
                                            <asp:BoundField DataField="COURSE_NAME" HeaderText="Stream" />
                                            <asp:BoundField DataField="CATEGORYNAME" HeaderText="Category" />
                                           
                                           
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="mb-2"></div>
                                <%--data-bs-toggle="modal" data-bs-target="#bookmoneydtl"--%>
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
                <Triggers>
                    <asp:PostBackTrigger ControlID="btn_savet" />
                    <asp:PostBackTrigger ControlID="btn_editbook" />
                    <asp:PostBackTrigger ControlID="btn_add" />
                    <asp:AsyncPostBackTrigger ControlID="btn_add_author" />
                    <asp:AsyncPostBackTrigger ControlID="btn_add_book_type" />
                    <%-- <asp:AsyncPostBackTrigger ControlID="SelectButton" />--%>
                    <asp:AsyncPostBackTrigger ControlID="Button1" />
                    <asp:AsyncPostBackTrigger ControlID="Button2" />
                    <asp:AsyncPostBackTrigger ControlID="Button3" />
                    <%--<asp:AsyncPostBackTrigger ControlID="Button4" />--%>
                    <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
                    <%--<asp:AsyncPostBackTrigger ControlID="btn_showdetails" />--%>
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>


