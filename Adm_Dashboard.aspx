<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_Dashboard.aspx.cs" Inherits="E_Granthalaya.Adm_BooksCount" %>
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
    <script type='text/javascript'>
        function ShowPopup() {
            $("#subs_details").modal("show");
        }
        function closeModal() {
            $('#subs_details').modal('hide');
            $('.modal-backdrop').hide();
        }

    </script>
    <style>
        .scroll::-webkit-scrollbar {
            width: 0 !important
        }

        #ContentPlaceHolder1_GridView1 table th {
            color: white !important;
        }

        #ContentPlaceHolder1_GridView2 table th {
            color: white !important;
        }

        #ContentPlaceHolder1_gridview_subscription_wrapper .justify-content-between {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

        #ContentPlaceHolder1_gridview_subscription_wrapper .justify-content-md-center {
            margin-top: 0px !important;
        }

        #ContentPlaceHolder1_gridview_subscription thead th {
            text-align: center;
        }

        #ContentPlaceHolder1_gridview_subscription tbody td {
            text-align: center;
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

        #ContentPlaceHolder1_GridView2_wrapper .justify-content-between {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

        #ContentPlaceHolder1_GridView2_wrapper .justify-content-md-center {
            margin-top: 0px !important;
        }

        #ContentPlaceHolder1_GridView2 thead th {
            text-align: center;
        }

        #ContentPlaceHolder1_GridView2 tbody td {
            text-align: center;
        }

        .btn-subs {
            color: white;
            background-color: #2e2d2d;
        }

            .btn-subs:hover {
                background-color: #2e2d2d;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item active">Dashboard</li>
        </ol>
        <asp:Panel ID="panel_warning" runat="server" Visible="false">
            <div class="alert alert-danger text-center">
                <asp:Label ID="lbl_warning" runat="server" Text="Some error occured" />
            </div>
        </asp:Panel>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
            <!------------ FIELD ----------------------------------------------------------------------------------------------------->
            <asp:UpdatePanel ID="update1" runat="server">
                <ContentTemplate>
                    <div class="card">
                        <div class="card-body">
                            <div class="modal" tabindex="-1" id="subs_details" role="dialog">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <div class="container-fluid">
                                                <h5 class="modal-title" style="font-weight: bold">Journal Details</h5>
                                            </div>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="container-fluid">
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Journal Name : "></asp:Label>
                                                    <asp:Label ID="lbl_journal" runat="server" Font-Bold="true" Text="" CssClass="form-label"></asp:Label>
                                                </div>
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Category : "></asp:Label>
                                                    <asp:Label ID="lbl_category" runat="server" Font-Bold="true" Text="" CssClass="form-label"></asp:Label>
                                                </div>
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Periodicity : "></asp:Label>
                                                    <asp:Label ID="lbl_period" runat="server" Font-Bold="true" Text="" CssClass="form-label"></asp:Label>
                                                </div>
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="lbl_datep" runat="server" Font-Bold="true" Text="" CssClass="form-label"></asp:Label>
                                                </div>
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="No. Of Entries : "></asp:Label>
                                                    <asp:Label ID="lbl_ecount" runat="server" Font-Bold="true" Text="" CssClass="form-label"></asp:Label>
                                                </div>
                                                <asp:Panel ID="pnl_entries" runat="server" Visible="false">
                                                    <div class="card p-2" style="font-weight: 500">
                                                        <asp:GridView ID="GridView2" runat="server" HorizontalAlign="Center" CssClass="table table-bordered col-md-auto table-responsive dataTable4" AutoGenerateColumns="false">
                                                            <HeaderStyle BackColor="#003da3" HorizontalAlign="Center" Font-Bold="true" ForeColor="White" />
                                                            <FooterStyle BackColor="#3366CC" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sr No">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="entries" HeaderText="Entry Dates" DataFormatString="{0:d}" ReadOnly="true" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="pnl_noentries" runat="server" Visible="false">
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
                                                            <asp:Label ID="Label5" runat="server" class="d-flex justify-content-center" Style="font-weight: 700; font-size: 1.2rem" Text="No Entries Found"></asp:Label>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <section class="content">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-xl-3 col-md-6 mb-3">
                                            <div class="card shadow h-100" style="border-left: .50rem solid #003da3 !important;">
                                                <div class="card-body">
                                                    <div class="row no-gutters align-items-center">
                                                        <div class="col mr-2">
                                                            <div class="text-uppercase mb-2 d-inline" style="font-size: 1.3rem; font-weight: 700; color: #003da3">
                                                                Total Books
                                                            </div>
                                                            <div style="font-weight: 700; font-size: 0.9rem">
                                                                All :
                                                                <asp:Label runat="server" ID="lbl_countBooks" ForeColor="#003da3" Text="" />
                                                            </div>
                                                            <div style="font-weight: 700; font-size: 0.9rem">
                                                                Unique :
                                                                <asp:Label runat="server" ID="lbl_countUnique" ForeColor="#003da3" Text="" />
                                                            </div>
                                                        </div>
                                                        <div class="col-auto">
                                                            <i class="fa-solid fa-book" style="font-size: 2.6rem; color: #003da3; opacity: 0.5; transform: rotate(5deg)"></i>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-footer mb-auto d-flex align-items-center justify-content-between">
                                                    <a class="small text-black stretched-link text-decoration-none" style="font-weight: 500" href="Adm_Report.aspx">More Info</a>
                                                    <div class="small text-black"><i class="fas fa-angle-right"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xl-3 col-md-6 mb-3">
                                            <div class="card shadow h-100" style="border-left: .50rem solid #006A05 !important;">
                                                <div class="card-body">
                                                    <div class="row no-gutters align-items-center">
                                                        <div class="col mr-2">
                                                            <div class="text-uppercase mb-2 d-inline" style="font-size: 1.3rem; font-weight: 700; color: #006A05">
                                                                Issued Books
                                                            </div>
                                                            <div style="font-weight: 700; font-size: 0.9rem">
                                                                Stud - <asp:Label runat="server" ID="lbl_i1" ForeColor="#006A05" Text="" />
                                                            </div>
                                                            <div style="font-weight: 700; font-size: 0.9rem">
                                                                Fac - <asp:Label runat="server" ID="lbl_i2" ForeColor="#006A05" Text="" />
                                                            </div>
                                                        </div>
                                                        <div class="col-auto">
                                                            <i class="fa-brands fa-stack-overflow" style="font-size: 2.6rem; color: #006A05; opacity: 0.5"></i>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-footer mb-auto d-flex align-items-center justify-content-between">
                                                    <a class="small text-black stretched-link text-decoration-none" style="font-weight: 500" href="Adm_Report.aspx">More Info</a>
                                                    <div class="small text-black"><i class="fas fa-angle-right"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xl-3 col-md-6 mb-3">
                                            <div class="card shadow h-100" style="border-left: .50rem solid #813C00 !important;">
                                                <div class="card-body">
                                                    <div class="row no-gutters align-items-center">
                                                        <div class="col mr-2">
                                                            <div class="text-uppercase mb-2 d-inline" style="font-size: 1.3rem; font-weight: 700; color: #813C00">
                                                                Returned Books
                                                            </div>
                                                            <div style="font-weight: 700; font-size: 0.9rem">
                                                                Stud - <asp:Label runat="server" ID="lbl_p1" ForeColor="#813C00" Text="" />
                                                            </div>
                                                            <div style="font-weight: 700; font-size: 0.9rem">
                                                                Fac - <asp:Label runat="server" ID="lbl_p2" ForeColor="#813C00" Text="" />
                                                            </div>
                                                        </div>
                                                        <div class="col-auto">
                                                            <i class="fa-solid fa-list" style="font-size: 2.6rem; color: #813C00; opacity: 0.5"></i>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-footer mb-auto d-flex align-items-center justify-content-between" style="font-weight: 400; color: #A70000">
                                                    <a class="small text-black stretched-link text-decoration-none" style="font-weight: 500" href="Adm_Report.aspx">More Info</a>
                                                    <div class="small text-black"><i class="fas fa-angle-right"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xl-3 col-md-6 mb-3">
                                            <div class="card shadow h-100" style="border-left: .50rem solid #A70000 !important;">
                                                <div class="card-body">
                                                    <div class="row no-gutters align-items-center">
                                                        <div class="col mr-2">
                                                            <div class="text-uppercase mb-2 d-inline" style="font-size: 1.3rem; font-weight: 700; color: #A70000">
                                                                Fine Collected
                                                            </div>
                                                            <div style="font-weight: 700; font-size: 1rem">
                                                                <asp:Label runat="server" ID="lbl_f1" ForeColor="#A70000" Text="" />
                                                            </div>
                                                        </div>
                                                        <div class="col-auto">
                                                            <i class="fa-solid fa-inr" style="font-size: 2.6rem; color: #A70000; opacity: 0.5"></i>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-footer mb-auto d-flex align-items-center justify-content-between" style="font-weight: 400; color: #A70000">
                                                    <a class="small text-black stretched-link text-decoration-none" style="font-weight: 500" href="Adm_Report.aspx">More Info</a>
                                                    <div class="small text-black"><i class="fas fa-angle-right"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                            <section>
                                <div class="btn btn-group px-0 py-0">
                                    <asp:Button ID="btn_subsr" CssClass="btn btn-sm btn-subs" Style="font-weight: 600; color: white" runat="server" OnClick="btn_subsr_Click" Text="Subscription Reminder" />
                                    <asp:Button ID="btn_subsd" CssClass="btn btn-sm btn-subs" Style="font-weight: 600; color: white" runat="server" OnClick="btn_subsd_Click" Text="Subscription Details" />
                                </div>
                                <asp:Panel ID="pnl_grid" runat="server" Visible="false">
                                    <div class="card p-2 table-responsive scroll" style="font-weight: 500">
                                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered  dataTable" Style="width: 100%" AutoGenerateColumns="false">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="journal_name" HeaderText="Journal Name" ReadOnly="true" />
                                                <asp:BoundField DataField="SUBS" HeaderText="Subscription Till" ReadOnly="true" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="DAYSLEFT" HeaderText="Days Left" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Extend Subscription">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="Btn_Extend" Style="background-color: #006A05; color: white; font-weight: 600" CssClass="btn btn-sm" runat="server" Text="Subscribe Now !" ToolTip="Extend Subscription" OnClick="Btn_Extend_Click">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnl_subs" runat="server" Visible="false">
                                    <div class="card p-2  table-responsive scroll " style="font-weight: 500">
                                        <asp:GridView ID="gridview_subscription" runat="server" CssClass="table table-bordered dataTable" Style="width: 100%" OnRowCommand="gridview_subscription_RowCommand" AutoGenerateColumns="false">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="journal_name" HeaderText="Journal Name" ReadOnly="true" />
                                                <asp:BoundField DataField="CategoryName" HeaderText="Category" ReadOnly="true" />
                                                <asp:BoundField DataField="period" HeaderText="Periodicity" ReadOnly="true" />
                                                <asp:BoundField DataField="entries" HeaderText="Entries" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Check Details">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnview" Style="background-color: #006A05; color: white; font-weight: 600" CssClass="btn btn-sm" runat="server" Text="View Details !" ToolTip="View record" OnClick="btnview_Click" CausesValidation="false"></asp:LinkButton>
                                                        <asp:LinkButton ID="btnenter" Style="background-color: #003da3; color: white; font-weight: 600" CssClass="btn btn-sm" runat="server" Text="Entry" ToolTip="Enter Record" CommandName="EnterRecord" CommandArgument='<%# Eval("journal_name") %>' CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </section>
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
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>
