<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_BookReturn.aspx.cs" Inherits="E_Granthalaya.Adm_BookReturn" %>

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
    <script type='text/javascript'>
        function ShowPopup() {
            $("#bookreturn").modal("show");
        }
        function closeModal() {
            $('#bookreturn').modal('hide');
            $('.modal-backdrop').hide();
        }

    </script>
    <style>
        #ContentPlaceHolder1_YourGridView,#ContentPlaceHolder1_GridView1 .justify-content-between {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

        #ContentPlaceHolder1_YourGridView,#ContentPlaceHolder1_GridView1 .justify-content-md-center {
            margin-top: 0px !important;
        }

        #ContentPlaceHolder1_YourGridView thead th {
            text-align: center;
            font-size: 14px !important;
        }

        #ContentPlaceHolder1_GridView1 thead th {
            text-align: center;
            font-size: 14px !important;
        }
        #ContentPlaceHolder1_YourGridView tbody td {
            text-align: center;
            font-size: 14px !important;
        }
        #ContentPlaceHolder1_GridView1 tbody td {
            text-align: center;
            font-size: 14px !important;
        }

        .custom {
            margin-left: 10px;
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
        <h3 class="mt-2"></h3>
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Book Return</li>
        </ol>
        <asp:Panel ID="panel_warning" runat="server" Visible="false">
            <div class="alert alert-danger text-center">
                <asp:Label ID="lbl_warning" runat="server" Text="Some error occurred" />
            </div>
        </asp:Panel>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="card" style="font-weight: 500">
                        <div class="card-body">
                            <div class="row form-group justify-content-between">
                                <div class="col-md-2">
                                    <h5 style="font-weight: bold">Book Return</h5>
                                </div>
                                <div class="col-md-auto">
                                    <asp:Label ID="lbl_note" runat="server" Style="font-size: 0.7rem; color: red; font-weight: 700" CssClass="text text-sm" Text="Note : Today's Date will be the returning date."></asp:Label>
                                </div>
                                <hr />
                            </div>

                            <div class="modal" tabindex="-1" id="bookreturn" role="dialog">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" style="font-weight: bold">Book Return</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="container-fluid">
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="lblstudent" runat="server" Font-Bold="true" Text="Roll No : "></asp:Label>
                                                    <asp:Label ID="lblrollno" runat="server" Text=""></asp:Label>
                                                </div>
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Book Name : "></asp:Label>
                                                    <asp:Label ID="lblbookname" runat="server" Text="" CssClass="text-capitalize form-label"></asp:Label>
                                                </div>
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Book ID : "></asp:Label>
                                                    <asp:Label ID="lblbookid" runat="server" Text="" CssClass="text-capitalize form-label"></asp:Label>
                                                </div>
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Publisher Name : "></asp:Label>
                                                    <asp:Label ID="lbl_publisher" runat="server" Text="" CssClass="text-capitalize form-label"></asp:Label>
                                                </div>

                                                <div class="form-group mb-1">
                                                    <asp:Label ID="Label3" runat="server" Font-Bold="true" Visible="false" Text="Book Requ ID: "></asp:Label>
                                                    <asp:Label ID="lbl_brid" runat="server" Text="" Visible="false" CssClass="text-capitalize form-label"></asp:Label>
                                                </div>
                                                <div class="form-group mb-1">
                                                    <asp:Label ID="lbl5" runat="server" Font-Bold="true" Visible="true" Text="Due Date: "></asp:Label>
                                                    <asp:Label ID="lbl_duedate" runat="server" Text="" Visible="true" CssClass="text-capitalize form-label"></asp:Label>
                                                </div>
                                                <div class="form-group mb-1">
                                                    <div class="alert alert-info d-flex align-items-center" role="alert">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-exclamation-triangle-fill flex-shrink-0 me-2" viewBox="0 0 16 16" role="img" aria-label="Warning:">
                                                            <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z" />
                                                        </svg>
                                                        <div style="margin-left: 10px;">
                                                            Fine : ₹ 
                                                            <asp:Label ID="lblfine" runat="server" Text="" CssClass="col-form-label" Font-Bold="true" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <%--                                            <asp:LinkButton ID="btnreturn" CssClass="btn btn-success" BackColor="Green" Font-Bold="true" runat="server" Text="Return Book" OnClick="btnreturn_Click"></asp:LinkButton>--%>
                                            <button type="button" class="btn btn-secondary" style="font-weight: bold" data-bs-dismiss="modal">Close</button>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row form-group mb-2">
                                <label class="col-md-auto col-form-label">Return Date : </label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_date" runat="server" CssClass="form-control" Style="font-weight: 500" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <section>
                                <div class="btn btn-group px-0 py-0">
                                    <asp:Button ID="btn_studreturn" CssClass="btn btn-sm btn-subs" Style="font-weight: 600; color: white" runat="server" OnClick="btn_studreturn_Click" Text="Student Details" />
                                    <asp:Button ID="btn_facreturn" CssClass="btn btn-sm btn-subs" Style="font-weight: 600; color: white" runat="server" OnClick="btn_facreturn_Click" Text="Teacher Details" />
                                </div>
                                <asp:Panel ID="pnl_return" runat="server" Visible="false">
                                    <div class="card p-2 mt-2" style="font-weight: 500">
                                        <asp:GridView ID="YourGridView" runat="server" Style="width: 100%" CssClass="table table-bordered table-responsive dataTable" DataKeyNames="BR_ID,BOOK_ID" OnSelectedIndexChanged="YourGridView_SelectedIndexChanged" AutoGenerateColumns="False">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BR_ID" HeaderText="BR_ID" ReadOnly="true" />
                                                <asp:BoundField DataField="ISSUED_TO" HeaderText="Issued To" ReadOnly="true" />
                                                <asp:BoundField DataField="BOOKNAME" HeaderText="Book Name" ReadOnly="true" />
                                                <asp:BoundField DataField="BOOK_ID" HeaderText="Book ID" ReadOnly="true" />
                                                <asp:BoundField DataField="FINE" HeaderText="Fine" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="View/Return">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnview" Style="color: green !important; text-decoration:none; font-weight: 600" CssClass="table-link text-success"  runat="server" ToolTip="View record" OnClick="btnview_Click" CausesValidation="false">
                                                        <span class="fa-stack">
                                                              <i class="fa fa-square fa-stack-2x"></i>
                                                              <i class="fa fa-eye fa-stack-1x fa-inverse"></i>
                                                         </span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnreturn" runat="server" CssClass="btn btn-sm small btn-success" Style="background-color: green; color: white; font-weight: 600"  Text="Return" OnClick="btnreturn_Click" CausesValidation="false">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnl_facrtrn" runat="server" Visible="false">
                                    <div class="mt-2 card p-2" style="font-weight: 500">
                                        <asp:GridView ID="GridView1" runat="server" Style="width: 100%" CssClass="table table-bordered table-responsive dataTable" DataKeyNames="BR_ID" OnSelectedIndexChanged="YourGridView_SelectedIndexChanged" AutoGenerateColumns="False">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BR_ID" HeaderText="BR_ID" ReadOnly="true" />
                                                <asp:BoundField DataField="ISSUED_TO" HeaderText="Issued To" ReadOnly="true" />
                                                <asp:BoundField DataField="BOOKNAME" HeaderText="Book Name" ReadOnly="true" />
                                                <asp:BoundField DataField="BOOK_ID" HeaderText="Book ID" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Return">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnreturn1" runat="server" CssClass="btn btn-sm small btn-success" Text="Return" Style="background-color: green; color: white; font-weight: 600"  OnClick="btnreturn1_Click" CausesValidation="false">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </section>
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
        </asp:Panel>
    </div>
</asp:Content>