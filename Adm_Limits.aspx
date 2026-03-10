<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_Limits.aspx.cs" Inherits="E_Granthalaya.Book_Limit" %>
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
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
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
    </style>
    <title>Add User Limits</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_DashBoard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Add Book Limit</li>
        </ol>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="update1" runat="server">
                <ContentTemplate>
                    <div class="card" style="font-weight: 500">
                        <div class="card-body">
                            <div class="row form-group">
                                <h5>Add Limits</h5>
                                <hr style="border: 1px solid black" />
                            </div>
                            <div class="row form-group mb-2">
                                <label class="col-md-auto col-form-label">No. of Days : <span class="text-danger">*</span></label>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txt_Days" Style="font-weight: 500" runat="server" onkeypress="return isNumberKey(event)" MaxLength="2" required="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label class="col-md-auto col-form-label">Max Book Limit : <span class="text-danger">*</span></label>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txt_Maxbook" Style="font-weight: 500" runat="server" onkeypress="return isNumberKey(event)" MaxLength="2" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label class="col-md-auto col-form-label">Fine Amount : <span class="text-danger">*</span></label>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txt_Fineamount" Style="font-weight: 500" runat="server" onkeypress="return isNumberKey(event)" CssClass="form-control"></asp:TextBox>
                                </div>
                                 <label class="col-md-auto col-form-label">Waiting Queue : <span class="text-danger">*</span></label>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txt_waiting" Style="font-weight: 500" runat="server" onkeypress="return isNumberKey(event)" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-auto">
                                    <asp:Button ID="btn_save" BackColor="#003da3" Font-Bold="true" runat="server" Text="Submit" OnClick="btn_save_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                            <asp:Panel ID="pnl_grid" runat="server" Visible="false">
                                <div class="card p-2 table-responsive scroll" style="font-weight: 500">
                                    <asp:GridView ID="GridView1" runat="server" Style="width: 100%" CssClass="table table-bordered dataTable" AutoGenerateColumns="false" >
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="YEAR_ID" HeaderText="YEAR" ReadOnly="true" />
                                            <asp:BoundField DataField="NO_OF_DAYS" HeaderText="NO OF DAYS" ReadOnly="true" />
                                            <asp:BoundField DataField="BOOKS_LIMIT" HeaderText="BOOK LIMIT" ReadOnly="true" />
                                            <asp:BoundField DataField="FINE_AMOUNT" HeaderText="FINE AMOUNT" ReadOnly="true" />
                                            <asp:BoundField DataField="WAITING_QUEUE" HeaderText="Waiting Queue" ReadOnly="true" />
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
        </asp:Panel>
    </div>
</asp:Content>