<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_EditBookMaster.aspx.cs" Inherits="E_Granthalaya.Adm_EditBookMaster" %>

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
        function closeModal(modalID) {
            $('[id*=' + modalID + ']').modal('hide');
            $('.modal-backdrop').hide();
        }
        function checkboxClicked() {
            __doPostBack('chk_updateall', '');
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
    </style>
    <title>Update Book</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <h3 class="mt-2"></h3>
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Edit Book</li>
        </ol>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
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
                                <label class="col-md-2 col-form-label">Book Type/Category : <span class="text-danger">*</span></label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddl_booktype" Style="font-weight: 500" CssClass="form-control form-select" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_booktype_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Book Type" Display="Dynamic" ControlToValidate="ddl_booktype" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <asp:Panel ID="pnl_coursem" runat="server" Visible="false">
                                <div class="row form-group mb-2">
                                    <asp:Label ID="Label2" runat="server" class="col-md-2 col-form-label" Text="Label">Select Course : <span class="text-danger">*</span></asp:Label>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddl_course" Style="font-weight: 500" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddl_course_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                    <asp:Label ID="Label3" runat="server" class="col-md-1 col-form-label" Text="Label">Semester:<span class="text-danger">*</span></asp:Label>
                                    <div class="col-md-1">
                                        <asp:DropDownList ID="ddl_Semester" Style="font-weight: 500" runat="server" CssClass="form-select" AutoPostBack="true" TextMode="Email" OnSelectedIndexChanged="ddl_Semester_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                    <asp:Label ID="Label4" runat="server" class="col-md-1 col-form-label" Text="Label">Subject : <span class="text-danger">*</span></asp:Label>
                                    <div class="col-md-5">
                                        <asp:DropDownList ID="ddl_subject" Style="font-weight: 500" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddl_subject_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnl_bookedit" runat="server">
                                <div class="row form-group mb-2">
                                    <label class="col-md-2 col-form-label">Book Name : <span class="text-danger">*</span></label>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txt_book_name" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter Book Name" Display="Dynamic" ControlToValidate="txt_book_name" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row form-group mb-2">
                                    <label class="col-md-2 col-form-label">Book Source : <span class="text-danger">*</span></label>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddl_book_source" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-select"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Select Book source" Display="Dynamic" ControlToValidate="ddl_book_source" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>
                                    <label class="col-md-1 col-form-label">Medium :<span class="text-danger">*</span></label>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddl_medium" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-select"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Select Medium" Display="Dynamic" ControlToValidate="ddl_medium" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>
                                    <label class="col-md-1 col-form-label">Book ID :<span class="text-danger">*</span></label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txt_bookid" Style="font-weight: 500" CssClass="form-control " runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Select Book id" Display="Dynamic" ControlToValidate="ddl_booktype" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row form-group mb-2">
                                    <label class="col-md-2 col-form-label">Select Author : <span class="text-danger">*</span></label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddl_author" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-select"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Select Author" Display="Dynamic" ControlToValidate="ddl_author" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>

                                    <label class="col-md-1 col-form-label">Publisher:<span class="text-danger">*</span></label>
                                    <div class="col-md-5">
                                        <asp:DropDownList ID="ddl_publisher" Style="font-weight: 500" CssClass="form-select" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Select publidher" Display="Dynamic" ControlToValidate="ddl_publisher" InitialValue="0" ForeColor="red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row form-group mb-2">
                                    <div class="col-md-10 d-flex">
                                        <asp:Button ID="btn_savet" BackColor="Green" Font-Bold="true" runat="server" Text="Update Book Details" OnClick="Button1_Click" CausesValidation="false" CssClass="btn btn-sm btn-success" />&nbsp;&nbsp;
                                            <div class="form-check form-switch small" style="margin-top: 4px;">
                                                <input class="form-check-input" type="checkbox" runat="server" autopostback="true" id="chk_updateall" onchange="checkboxClicked()">
                                                <label class="form-check-label" for="chk_updateall">Update All</label>
                                            </div>
                                    </div>
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
                            <asp:Panel ID="pnl_grid" runat="server">
                                <div class="card p-2 table-responsive scroll">
                                    <asp:GridView ID="YourGridView" runat="server" DataKeyNames="BOOK_ID,LINKEDWITH" CssClass="table table-bordered dataTable" OnSelectedIndexChanged="YourGridView_SelectedIndexChanged" AutoGenerateSelectButton="false" AutoGeneratedColumns="false" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:BoundField DataField="BOOK_ID" HeaderText="BOOK ID" />
                                            <asp:BoundField DataField="BOOKNAME" HeaderText="BOOK NAME" />
                                            <asp:BoundField DataField="PUBLISHERNAME" HeaderText="PUBLISHER NAME" />
                                            <asp:BoundField DataField="AUTHORNAME" HeaderText="AUTHOR NAME" />
                                            <asp:BoundField DataField="CATEGORYNAME" HeaderText="CATEGORY NAME" />
                                            <asp:BoundField DataField="LANGUAGE" HeaderText="MEDIUM" />
                                            <asp:BoundField DataField="COURSE_NAME" HeaderText="COURSE NAME" />
                                            <asp:BoundField DataField="SEM" HeaderText="SEMESTER" />
                                            <asp:TemplateField HeaderText="Update">
                                                <ItemTemplate>
                                                    <asp:Button ID="SelectButton" runat="server" CommandName="Select" BackColor="Green" CssClass="btn small btn-sm btn-success" Style="font-weight: 600" CausesValidation="false" Text="✎ Edit" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>
