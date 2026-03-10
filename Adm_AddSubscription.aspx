<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_AddSubscription.aspx.cs" Inherits="E_Granthalaya.Adm_AddSubscription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Add New Book</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
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

        .circlebtn {
            margin-top: 4px;
            font-weight: 900;
            border-radius: 100px;
            padding-top: 0px;
            padding-bottom: 2px;
            font-size: 1.1rem
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
        }

        #ContentPlaceHolder1_YourGridView tbody td {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <h3 class="mt-2"></h3>
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Subcription Page</li>
        </ol>
        <asp:Panel ID="panel_warning" runat="server" Visible="false">
            <div class="alert alert-danger text-center">
                <asp:Label ID="lbl_warning" runat="server" Text="Some error occured" />
            </div>
        </asp:Panel>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>

            <div class="modal" id="examplemodal" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" style="font-weight: bold">Add New Journal Name </h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body" style="font-weight: 500">
                            <label class="col-form-label">Journal Name <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txt_add_journal" Style="font-weight: 500" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btn_add_journal_name" BackColor="#003da3" Font-Bold="true" runat="server" class="btn btn-primary" OnClick="btn_journal_name_Click" Text="Save changes" data-bs-dismiss="modal" CausesValidation="False" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal" id="examplemodal1" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" style="font-weight: bold">Add New Category Name </h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body" style="font-weight: 500">
                            <label class="col-form-label">Category Name <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txt_category_name" Style="font-weight: 500" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btn_add_category_name" BackColor="#003da3" Font-Bold="true" runat="server" class="btn btn-primary" OnClick="btn_category_name_Click" Text="Save changes" data-bs-dismiss="modal" CausesValidation="False" />
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="update1" runat="server">
                <ContentTemplate>
                    <div class="card">
                        <div class="card-body" style="font-weight: 500">
                            <div class="row form-group">
                                <h5>Subscription Master</h5>
                                <hr style="border: 1px solid black" />
                            </div>

                            <div class="row form-group mb-2">
                                <label class="col-md-2 col-xl-2 col-form-label">Journal Name<span class="text-danger">*</span></label>
                                <div class="col-md-4 col-xl-4">
                                    <asp:DropDownList ID="ddl_journal_name" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-select"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Select journal name" Display="Dynamic" ControlToValidate="ddl_journal_name" ForeColor="Red" BorderColor="Black"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Button ID="btn_journal_name" BackColor="#003da3" align="center" Font-Bold="true" runat="server" Text="+" CssClass="btn circlebtn btn-sm btn-primary" data-bs-target="#examplemodal" data-bs-toggle="modal" CausesValidation="false" />
                                </div>

                                <label class="col-md-1 col-form-label">Category<span class="text-danger">*</span></label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddl_category_name" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-select" OnSelectedIndexChanged="ddl_category_name_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidate2" runat="server" ErrorMessage="Enter category name" Display="Dynamic" ControlToValidate="ddl_category_name" ForeColor="Red" BorderColor="Black"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Button ID="btn_category_name" BackColor="#003da3" align="center" Font-Bold="true" runat="server" Text="+" CssClass="btn circlebtn btn-sm btn-primary" data-bs-target="#examplemodal1" data-bs-toggle="modal" CausesValidation="false" />
                                </div>
                            </div>

                            <div class="row form-group mb-2">
                                <%--<label class="col-md-2 col-form-label">Subscription Id<span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_sub_id" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter Subscription ID" Display="Dynamic" ControlToValidate="txt_sub_id" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_sub_id" Display="Dynamic" ErrorMessage="Subscription ID must be a number" ValidationExpression="\d+" ForeColor="red"></asp:RegularExpressionValidator>
                                </div>--%>
                                <label class="col-md-2 col-form-label">Subs. No.<span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_sub_no" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidato3" runat="server" ErrorMessage="Enter Subscription No" Display="Dynamic" ControlToValidate="txt_sub_no" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_sub_no" Display="Dynamic" ErrorMessage="Subscription No. must be a number" ValidationExpression="\d+" ForeColor="red"></asp:RegularExpressionValidator>--%>
                                </div>
                                <label class="col-md-auto col-form-label">Contact Person<span class="text-danger">*</span></label>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txt_cont_per" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Enter contact person name" Display="Dynamic" ControlToValidate="txt_cont_per" ForeColor="Red" BorderColor="Black"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row form-group mb-2">
                                <label class="col-md-2 col-form-label">Telephone no.<span class="text-danger">*</span></label>
                                <div class="col-md-3 col-xl-2">
                                    <asp:TextBox ID="txt_tel_no" Style="font-weight: 500" runat="server" onkeypress="return isNumberKey(event)" MaxLength="10" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter telephone no." Display="Dynamic" ControlToValidate="txt_tel_no" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revTelephoneNumber" runat="server" ControlToValidate="txt_tel_no" Display="Dynamic" ErrorMessage="Invalid telephone number format." ValidationExpression="^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$" ForeColor="red"></asp:RegularExpressionValidator>
                                </div>
                                <label class="col-md-1 col-form-label">Mobile No</label>
                                <div class="col-xl-2">
                                    <asp:TextBox ID="txt_mob_no" Style="font-weight: 500" runat="server" onkeypress="return isNumberKey(event)" MaxLength="10" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter mobile no." Display="Dynamic" ControlToValidate="txt_mob_no" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revmobnumber" runat="server" ControlToValidate="txt_mob_no" Display="Dynamic" ErrorMessage="Invalid phone number format." ValidationExpression="^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$" ForeColor="red"></asp:RegularExpressionValidator>
                                </div>
                                <label class="col-md-1 col-form-label">Email ID<span class="text-danger">*</span></label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txt_email" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Enter email" Display="Dynamic" ControlToValidate="txt_email" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row form-group mb-2">
                                <label class="col-md-2 col-form-label">Address</label>
                                <div class="col-md-10">
                                    <textarea id="txt_address" style="font-weight: 500" runat="server" rows="1" autocomplete="off" class="form-control"></textarea>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Enter Address" Display="Dynamic" ControlToValidate="txt_address" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>

                            </div>

                            <div class="row form-group mb-2">
                                <label class="col-md-2 col-form-label">From-To Date<span class="text-danger">*</span></label>
                                <div class="col-md-2 ">
                                    <asp:TextBox ID="txt_sub_from" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" TextMode="date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Enter Subscription date" Display="Dynamic" ControlToValidate="txt_sub_from" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3 col-xl-2">
                                    <asp:TextBox ID="txt_sub_to" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" TextMode="date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter Subscription date" Display="Dynamic" ControlToValidate="txt_sub_to" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1"></div>
                                <label class="col-md-1 col-form-label">Periodicity</label>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddl_periodicity" Style="font-weight: 500" runat="server" AutoPostBack="true" AutoComplete="off" CssClass="form-select" OnSelectedIndexChanged="ddl_periodicity_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Select Periodicity " Display="Dynamic" ControlToValidate="ddl_periodicity" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row form-group mb-2">
                                <label class="col-md-2  col-form-label">Transaction ID<span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_trans_id" Style="font-weight: 500" CssClass="form-control" runat="server" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Enter transaction id" Display="Dynamic" ControlToValidate="txt_trans_id" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                                <label class="col-md-1 col-form-label">Trx. Date<span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_trans_date" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Enter Transaction date" Display="Dynamic" ControlToValidate="txt_trans_date" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                                <label class="col-md-1 col-form-label">Amount<span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_amount" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Enter Amount  " Display="Dynamic" ControlToValidate="txt_amount" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row form-group mb-2">
                                <div class="col-md-2">
                                    <asp:Button ID="btn_save" BackColor="Green" Font-Bold="true" runat="server" Text="Save Details" CssClass="btn btn-sm btn-success" OnClick="btn_save_Click" />
                                </div>
                            </div>

                            <asp:Panel ID="PNL_GRID" runat="server">
                                <div class="card p-2 table-responsive scroll">
                                    <asp:GridView ID="YourGridView" runat="server" DataKeyNames="subscription_id, subscription_no" CssClass="table table-bordered dataTable" OnSelectedIndexChanged="YourGridView_SelectedIndexChanged" AutoGeneratedColumns="false" ShowHeaderWhenEmpty="True" AutoGenerateSelectButton="false" AutoGenerateColumns="False">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:BoundField DataField="journal_name" HeaderText="Journal" />
                                            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                            <asp:BoundField DataField="subscription_from" HeaderText="From Date" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="subscription_to" HeaderText="To Date" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="periodicity" HeaderText="Periodicity" />
                                            <asp:BoundField DataField="amount" HeaderText="Amount" />
                                            <asp:BoundField DataField="transaction_date" HeaderText="Trx. Date" DataFormatString="{0:d}" />
                                            <asp:TemplateField HeaderText="Update">
                                                <ItemTemplate>
                                                    <asp:Button ID="SelectButton" runat="server" CommandName="Select" CssClass="btn btn-sm small btn-success" style="font-weight:600;" BackColor="Green" CausesValidation="false" Text="✎ Edit Details" />
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
                    <asp:PostBackTrigger ControlID="btn_save" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>