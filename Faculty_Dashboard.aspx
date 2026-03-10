<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Faculty_Dashboard.aspx.cs" Inherits="E_Granthalaya.Faculty_Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Dashboard</title>
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
            Swal.fire
                ({
                    title: "Delete this Content?",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#006A05',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, Delete',
                    cancelButtonText: 'Close',
                    customClass:
                    {
                        popup: 'custom-swal-popup',
                        title: 'custom-swal-title',
                        icon: 'custom-swal-icon',
                        text: 'custom-swal-text',
                        confirmButton: 'custom-swal-confirm-button',
                        cancelButton: 'custom-swal-cancel-button'
                    }
                }).then((result) => {
                    if (result.isConfirmed) {
                        __doPostBack('<%= GridView3.UniqueID %>', 'DeleteContent$' + rowIndex);
                    }
                });
            return false;
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

        .scroll::-webkit-scrollbar {
            width: 0 !important
        }

        #ContentPlaceHolder1_GridView1, #ContentPlaceHolder1_GridView2, #ContentPlaceHolder1_GridView3, #ContentPlaceHolder1_GridView4 table th {
            color: white !important;
        }

        #ContentPlaceHolder1_GridView1_wrapper, #ContentPlaceHolder1_GridView2_wrapper, #ContentPlaceHolder1_GridView3_wrapper, #ContentPlaceHolder1_GridView4_wrapper .justify-content-between {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

        #ContentPlaceHolder1_GridView1_wrapper, #ContentPlaceHolder1_GridView2_wrapper, #ContentPlaceHolder1_GridView3_wrapper, #ContentPlaceHolder1_GridView4_wrapper .justify-content-md-center {
            margin-top: 0px !important;
        }

        #ContentPlaceHolder1_GridView1 thead th, #ContentPlaceHolder1_GridView2 thead th, #ContentPlaceHolder1_GridView3 thead th, #ContentPlaceHolder1_GridView4 thead th {
            text-align: center;
            font-size: 15px;
        }

        #ContentPlaceHolder1_GridView1 tbody td, #ContentPlaceHolder1_GridView2 tbody td, #ContentPlaceHolder1_GridView3 tbody td, #ContentPlaceHolder1_GridView4 tbody td {
            text-align: center;
            color: black;
            font-size: 15px;
        }

        .btn-dashboard {
            color: #fff;
            background-color: #250063;
        }

            .btn-dashboard:hover {
                background-color: #250063;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item active">Dashboard</li>
        </ol>
        <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="update1" runat="server">
            <ContentTemplate>
                <div class="card">
                    <div class="card-body">
                        <section class="content">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-xl-4 col-md-6 mb-4">
                                        <div class="card shadow h-100" style="border-left: .50rem solid #003da3 !important;">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-uppercase mb-2" style="font-size: 1.3rem; font-weight: 700; color: #003da3">
                                                            books status 
                                                            <div style="font-weight: 700; font-size: 1rem">
                                                                <asp:Label ID="lbl_book" runat="server" Text=""></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <i class="fa-solid fa-book" style="font-size: 3.2rem; color: #003da3; opacity: 0.5; transform: rotate(5deg)"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-footer mb-auto d-flex align-items-center justify-content-between">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-4 col-md-6 mb-4">
                                        <div class="card shadow h-100" style="border-left: .50rem solid #006A05 !important;">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-uppercase mb-2" style="font-size: 1.3rem; font-weight: 700; color: #006A05">
                                                            Books History 
                                                            <div style="font-weight: 700; font-size: 1rem">
                                                                <asp:Label ID="lbl_history" runat="server" Text=""></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <i class="fa-brands fa-stack-overflow" style="font-size: 3.3rem; color: #006A05; opacity: 0.5"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-footer mb-auto d-flex align-items-center justify-content-between">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-4 col-md-6 mb-4">
                                        <div class="card shadow h-100" style="border-left: .50rem solid #A70000 !important;">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-uppercase mb-2" style="font-size: 1.3rem; font-weight: 700; color: #A70000">
                                                            e-contents 
                                                            <div style="font-weight: 700; font-size: 1rem">
                                                                <asp:Label ID="lbl_count" runat="server" Text=""></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <i class="fa-solid fa-list" style="font-size: 3.2rem; color: #A70000; opacity: 0.5"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-footer mb-auto d-flex align-items-center justify-content-between" style="font-weight: 400; color: #813C00">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                        <section>
                            <div class="btn btn-group px-0 py-0">
                                <asp:Button ID="btn_request" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600; border-right: 3px solid white; border-radius: 3px; color: white" runat="server" OnClick="btn_request_Click" Text="Request Details" />
                                <asp:Button ID="btn_issue" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600; border-right: 3px solid white; border-radius: 3px; color: white" runat="server" OnClick="btn_issue_Click" Text="Issue Details" />
                                <asp:Button ID="btn_count" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600; border-right: 3px solid white; border-radius: 3px; color: white" runat="server" OnClick="btn_count_Click" Text="Uploaded Contents" />
                                <asp:Button ID="btn_history" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600; color: white" runat="server" OnClick="btn_history_Click" Text="History" />
                            </div>
                            <asp:Panel ID="pnl_bookrequest" runat="server" Visible="false">
                                <div class="card p-2 table-responsive scroll" style="font-weight: 500;">
                                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered dataTable" AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="bookid" HeaderText="Book ID" ReadOnly="true" />
                                            <asp:BoundField DataField="BookName" HeaderText="Book Name" ReadOnly="true" />
                                            <asp:BoundField DataField="PUBLISHER" HeaderText="Publisher" ReadOnly="true" />
                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" ReadOnly="true" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" ReadOnly="true" DataFormatString="{0:d}" />
                                            <asp:TemplateField HeaderText="Cancel">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete" CssClass="table-link text-danger" runat="server" Text="Delete" ToolTip="cancel Request" OnClick="lnkdelete_Click">
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
                            </asp:Panel>
                            <asp:Panel ID="pnl_bookissue" runat="server" Visible="false">
                                <div class="card p-2 table-responsive scroll" style="font-weight: 500;">
                                    <asp:GridView ID="GridView2" runat="server" CssClass="table table-bordered dataTable" AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BookName" HeaderText="Book Name" ReadOnly="true" />
                                            <asp:BoundField DataField="PUBLISHER" HeaderText="Publisher" ReadOnly="true" />
                                            <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" ReadOnly="true" DataFormatString="{0:d}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnl_bookhistory" runat="server" Visible="false">
                                <div class="card p-2 table-responsive scroll" style="font-weight: 500;">
                                    <asp:GridView ID="GridView4" runat="server" CssClass="table table-bordered dataTable" AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BookName" HeaderText="Book Name" ReadOnly="true" />
                                            <asp:BoundField DataField="PUBLISHER" HeaderText="Publisher" ReadOnly="true" />
                                            <asp:BoundField DataField="IssueDate" HeaderText="Issued On" ReadOnly="true" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="ReturnDate" HeaderText="Returned" ReadOnly="true" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="BSTATUS" HeaderText="Status" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnl_count" runat="server" Visible="false">
                                <div class="card p-2 table-responsive scroll" style="font-weight: 500;">
                                    <asp:GridView ID="GridView3" runat="server" CssClass="table table-bordered dataTable" AutoGenerateColumns="false" OnRowCommand="GridView3_RowCommand">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="COURSE_NAME" HeaderText="Course" ReadOnly="true" />
                                            <asp:BoundField DataField="SEM" HeaderText="Semester" ReadOnly="true" />
                                            <asp:BoundField DataField="SUBJECT_NAME" HeaderText="Subject" ReadOnly="true" />
                                            <asp:BoundField DataField="CONTENT_TYPE" HeaderText="Content Type" ReadOnly="true" />
                                            <asp:BoundField DataField="FILENAME" HeaderText="File Name" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <a target="_blank" class="btn btn-sm btn-primary" style="font-weight: 600; background-color: #003da3" runat="server" href='<%# ResolveUrl(GetImageURL(Eval("FILENAME"))) %>'>View / Download</a>
                                                    <asp:LinkButton ID="btn_delete" Style="font-weight: 600; color: white; background-color: #940000" CssClass="btn btn-sm btn-danger" runat="server" Text="Delete" CommandName="DeleteContent" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Delete Content" OnClientClick='<%# "return DeleteContent(" + Container.DataItemIndex + ");" %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnl_nodata" runat="server" Visible="false">
                                <div class="container-fluid py-2 px-0">
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
                                        <asp:Label ID="lbl_not" runat="server" class="d-flex justify-content-center" Style="font-weight: 700; font-size: 1.2rem" Text=""></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                        </section>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
