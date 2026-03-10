<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_ContentMaster.aspx.cs" Inherits="E_Granthalaya.Adm_ContentMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Content Master</title>
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
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function DeleteContent(rowIndex) {
            Swal.fire({
                title: "Delete this Content?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#006A05',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Delete',
                cancelButtonText: 'Close',
                customClass: {
                    popup: 'custom-swal-popup',
                    title: 'custom-swal-title',
                    icon: 'custom-swal-icon',
                    text: 'custom-swal-text',
                    confirmButton: 'custom-swal-confirm-button',
                    cancelButton: 'custom-swal-cancel-button'
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    __doPostBack('<%= GridView1.UniqueID %>', 'DeleteContent$' + rowIndex);
                }
            });
            return false;
        }

        document.getElementById('fileUpload').addEventListener('change', function (event) {
            var fileSize = this.files[0].size / 1024 / 1024;
            if (fileSize > 4) {
                toastr.error('File size is too large. Please upload a file smaller than 4MB.');
                event.preventDefault();
            }
        });

    </script>
    <style>
        .custom-swal-popup {
            width: 400px !important;
            height: 200px;
            margin-top: 20px !important;
        }

        .custom-swal-title {
            margin-top: -4px !important;
            font-size: 25px !important;
        }

        .custom-swal-icon {
            margin-top: -8px !important;
            font-size: 10px !important;
        }

        .custom-swal-confirm-button, .custom-swal-cancel-button {
            margin-top: -6px !important;
            padding: 5px 10px !important;
            font-size: 17px !important;
        }

        .custom-swal-text {
            font-size: 0px;
        }
    </style>
    <style>
        .scroll::-webkit-scrollbar {
            width: 0 !important
        }

        #ContentPlaceHolder1_GridView1 table th {
            color: white !important;
        }

        #ContentPlaceHolder1_GridView1_wrapper .justify-content-between {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

        #ContentPlaceHolder1_GridView1_wrapper .justify-content-md-center {
            margin-top: 0px !important;
        }

        #ContentPlaceHolder1_GridView1 thead th {
            text-align: center;
        }

        #ContentPlaceHolder1_GridView1 tbody td {
            text-align: center;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Add New Content</li>
        </ol>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
            <!--modal popup code-------------------------------------------------------------------------------------------------------------------->
            <div class="modal fade" id="exmodal" tabindex="-1" role="dialog" aria-labelledby="con_model" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="con_model">Add New Content Type</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <label for="recipient-name" class="col-form-label">Content Type : </label>
                                <asp:TextBox ID="txt_addcontent" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <asp:Button ID="btn_add" runat="server" CssClass="btn btn-primary" OnClick="btn_add_Click" Text="Save" />
                        </div>
                    </div>
                </div>
            </div>

            <!--main page code----------------------------------------------------------------------------------------------------------------------------->
            <asp:UpdatePanel ID="update1" runat="server">
                <ContentTemplate>
                    <div class="card" style="font-weight: 500">
                        <div class="card-body">
                            <div class="row form-group">
                                <h5>Content Details</h5>
                                <hr style="border: 1px solid black" />
                            </div>
                            <div class="row form-group mb-2">
                                <label class="col-md-1 col-form-label">
                                    Year :<span class="text-danger">*</span>
                                </label>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddl_year" CssClass="form-control form-select" AutoPostBack="true" Style="font-weight: 500" data-toggle="dropdown" runat="server" OnSelectedIndexChanged="ddl_year_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <label class="col-md-1 col-form-label">
                                    Course :<span class="text-danger">*</span>
                                </label>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddl_course" CssClass="form-control form-select" Style="font-weight: 500" data-toggle="dropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_course_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <label class="col-md-1 col-form-label">
                                    Subject :<span class="text-danger">*</span>
                                </label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddl_semsub" CssClass="form-control form-select" Style="font-weight: 500" data-toggle="dropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_sub_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group mb-2">
                                <label class="col-md-1 col-form-label">
                                    Content :<span class="text-danger">*</span>
                                </label>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddl_con" CssClass="form-control form-select" Style="font-weight: 500" data-toggle="dropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_con_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-auto">
                                    <asp:Button ID="btn_addcon" BackColor="#003da3" Font-Bold="true" CssClass="btn circlebtn btn-sm btn-primary" data-bs-toggle="modal" data-bs-target="#exmodal" align="center" runat="server" Text="+" />
                                </div>
                                <label class="col-md-auto col-form-label">
                                    Upload Content :<span class="text-danger">*</span>
                                </label>
                                <div class="col-md-6 d-flex">
                                    <asp:FileUpload ID="fileupload" Style="font-weight: 500" CssClass="form-control" OnClick="fileupload_click" runat="server" />&nbsp;
                                    <asp:TextBox ID="txtfile" runat="server" Style="font-weight: 500" CssClass="form-control" MaxLength="50" placeholder="Enter File Name < 50 Characters*"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-group mb-2">
                                <div class="col-md-10">
                                    <asp:Button ID="btn_submit" BackColor="#003da3" Font-Bold="true" runat="server" Text="Submit" Width="100px" CssClass="btn btn-primary" OnClick="btn_submit_Click" />
                                    <asp:Label class="col-md-auto col-form-label-sm" ID="lbl_warn" runat="server" Style="padding-inline-start: 10px; padding-inline-end: 10px; color: #084298; background-color: #cfe2ff; border-color: #b6d4fe; border-radius: 8px; font-weight: 600;" Text="Upload a File (PDF,PPT,DOC,JPG,PNG,XLS) : Maximum File Size - 500Kb"></asp:Label>
                                </div>
                            </div>
                            <!-- GRID VIEW ----------------------------------------------------------------------------------------------------->
                            <asp:Panel ID="pnl_grid" runat="server" Visible="false">
                                <div class="card p-2 table-responsive scroll" style="font-weight: 500">
                                    <asp:GridView ID="GridView1" runat="server" Style="width: 100%" CssClass="table table-bordered dataTable" AutoGenerateColumns="false" OnRowCommand="GridView1_RowCommand">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="content_id" HeaderText="Content Type" />
                                            <asp:BoundField DataField="document_path" HeaderText="File Name" />
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <a target="_blank" class="btn btn-sm btn-primary" Style="font-weight: 600;background-color:#003da3" runat="server" href='<%# ResolveUrl(GetImageURL(Eval("document_path"))) %>'>View / Download</a>
                                                    <asp:LinkButton ID="btn_delete" Style="font-weight: 600; color: white;background-color:#940000" CssClass="btn btn-sm btn-danger" runat="server" Text="Delete" CommandName="DeleteContent" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Delete Content" OnClientClick='<%# "return DeleteContent(" + Container.DataItemIndex + ");" %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                <Triggers>
                    <asp:PostBackTrigger ControlID="btn_submit" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>