<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_SubscriptionEntry.aspx.cs" Inherits="E_Granthalaya.Adm_SubscriptionEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Add Subscription Entry</title>
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
        function DeleteEntry(rowIndex) {
            Swal.fire({
                title: "Delete this Entry?",
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
                    __doPostBack('<%= gridview_subscription_details.UniqueID %>', 'Delete$' + rowIndex);
                }
            });
            return false;
        }
        function Showmodal(bookId) {
            var imagepath = 'ImageHandler1.ashx?SUBS_IMAGE=' + bookId;
            // Update the src attribute of the image with the imagePath
            $('#bookImage').attr('src', imagepath);
            $("#imageModal").modal("show");
        }
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

        #ContentPlaceHolder1_gridview_subscription_details table th {
            color: white !important;
        }

        #ContentPlaceHolder1_gridview_subscription_details_wrapper .justify-content-between {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

        #ContentPlaceHolder1_gridview_subscription_details_wrapper .justify-content-md-center {
            margin-top: 0px !important;
        }

        #ContentPlaceHolder1_gridview_subscription_details thead th {
            text-align: center;
        }

        #ContentPlaceHolder1_gridview_subscription_details tbody td {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <h3 class="mt-2"></h3>
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Add Subscription Entry</li>
        </ol>
        <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="update1" runat="server">

            <ContentTemplate>
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
                <div class="card" style="font-weight: 500">
                    <div class="card-body">
                        <div class="row form-group">
                            <h5>Subscription Details </h5>
                            <hr style="border: 1px solid black" />
                        </div>
                        <div class="row form-group mb-2">
                            <label class="col-md-auto col-form-label">
                                Subscription Name:<span class="text-danger">*</span>
                            </label>
                            <div class="col-md-auto">
                                <asp:DropDownList ID="ddl_jname" Style="font-weight: 500" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddl_jname_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <label class="col-md-auto col-form-label">Date:<span class="text-danger">*</span></label>
                            <div class="col-md-2">
                                <asp:TextBox ID="txt_subscription_date" Style="font-weight: 500" required="true" runat="server" AutoComplete="off" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <label class="col-md-auto col-form-label">Upload Book Image : </label>
                            <div class="col-md-3">
                                <asp:FileUpload Style="font-weight: 500" ID="fileupload" CssClass="form-control" runat="server" />
                            </div>

                        </div>
                        <div class="row form-group mb-2">
                            <div class="col-md-3">
                                <asp:Button ID="btn_subscription_entry" runat="server" Font-Bold="true" BackColor="#003da3" Text="Submit" Width="100px" CssClass="btn btn-primary" OnClick="btn_subscription_entry_click" />
                            </div>
                        </div>
                        <asp:Panel ID="pnl_bookdetails" runat="server" Visible="false">
                            <div class="card p-2">
                                <div class="form-group mb-1">
                                    <asp:Label ID="lblstudent" runat="server" Font-Bold="true" Text="Subscription Name : "></asp:Label>
                                    <asp:Label ID="lbl_jname" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="form-group mb-1">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Category : "></asp:Label>
                                    <asp:Label ID="lbl_Category" runat="server" Text="" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="form-group mb-1">
                                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Periodicity : "></asp:Label>
                                    <asp:Label ID="lbl_Periodicity" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="form-group mb-1">
                                    <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Period : "></asp:Label>
                                    <asp:Label ID="lbl_Period" runat="server" Text="" CssClass="form-label"></asp:Label>
                                </div>
                                <asp:Panel ID="pnl_books" runat="server" Visible="false">
                                    <div class=" table-responsive scroll">
                                        <asp:GridView ID="gridview_subscription_details" runat="server" DataKeyNames="SUBS_IMAGE" CssClass="table table-bordered dataTable" AutoGenerateColumns="false" OnRowDeleting="gridview_subscription_details_RowDeleting">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="SUBS_DATE" SortExpression="SUBS_DATE" HeaderText="Entry Dates" ReadOnly="true" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="SUBS_IMAGE" HeaderText="Image Name" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkdelete" CssClass="table-link text-danger" runat="server" Text="Delete" CommandName="Delete" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Delete record" OnClientClick='<%# "return DeleteEntry(" + Container.DataItemIndex + ");" %>'>
                                                  <span class="fa-stack">
                                                     <i class="fa fa-square fa-stack-2x"></i>
                                                     <i class="fa fa-trash fa-stack-1x fa-inverse"></i>
                                                  </span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnview" Style="color: green !important; text-decoration: none; font-weight: 600" CssClass="table-link text-success" runat="server" ToolTip="View record" OnClick="btnview_Click" CausesValidation="false">
                                                           <span class="fa-stack">
                                                                 <i class="fa fa-square fa-stack-2x"></i>
                                                                 <i class="fa fa-eye fa-stack-1x fa-inverse"></i>
                                                           </span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_nodata" runat="server" Visible="false">
                            <div class="container-fluid py-1 px-0">
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
                                    <asp:Label ID="lbl_not" runat="server" class="d-flex justify-content-center" Style="font-weight: 700; font-size: 1.2rem" Text="No Entries Found"></asp:Label>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_subscription_entry" />
                <%--<asp:AsyncPostBackTrigger ControlID="btn_showdetails" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
