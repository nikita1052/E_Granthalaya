<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_BookIssue.aspx.cs" Inherits="E_Granthalaya.Adm_BookIssue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Book Issue</title>
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
            $('.dataTable1').prepend($("<thead></thead>").append($('.dataTable1').find("tr:first"))).DataTable({
                bLengthChange: true,
                lengthMenu: [[10, 20, -1], [10, 20, "All"]],
                bFilter: true,
                bSort: true,
                bPaginate: true
            });
        }
        function printGrid() {
            var grids = document.querySelectorAll('.dataTable');
            var visibleGrid = null;
            for (var i = 0; i < grids.length; i++) {
                if (grids[i].offsetParent !== null) {
                    visibleGrid = grids[i];
                    break;
                }
            }
            if (visibleGrid !== null) {
                var printWindow = window.open('', '', 'width=800,height=600');
                printWindow.document.write('<html><head><title>Print Grid</title></head><body>');
                printWindow.document.write(visibleGrid.outerHTML);
                printWindow.document.write('</body></html>');
                printWindow.print();
                printWindow.close();
            } else {
                //    alert('No grid is visible to print.');
                toastr.error('No grid visible to print.');
            }
        }

    </script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function IssueBook(type, rowIndex) {
            let gridViewID;
            switch (type) {
                case 'student':
                    gridViewID = '<%= GridView1.UniqueID %>';
                    break;
                case 'faculty':
                    gridViewID = '<%= GridView3.UniqueID %>';
                    break;
                case 'instant':
                    gridViewID = '<%= GridView4.UniqueID %>';
                    break;
                default:
                    console.error('Invalid type');
                    return false;
            }

            Swal.fire({
                title: "Issue this request?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#006A05',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Issue',
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
                    __doPostBack(gridViewID, 'Issue$' + rowIndex);
                }
            });

            return false;
        }

        function CancelRequest(type, rowIndex) {
            let gridViewID;
            switch (type) {
                case 'student':
                    gridViewID = '<%= GridView1.UniqueID %>';
                    break;
                case 'faculty':
                    gridViewID = '<%= GridView3.UniqueID %>';
                    break;
                case 'instant':
                    gridViewID = '<%= GridView4.UniqueID %>';
                    break;
                default:
                    console.error('Invalid type');
                    return false;
            }

            Swal.fire({
                title: "Cancel this request?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#006A05',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Cancel',
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
                    __doPostBack(gridViewID, 'CancelIssue$' + rowIndex);
                }
            });

            return false;
        }

        function showPanel(panelId) {
            $('#<%= pnl_code.ClientID %>').hide();
            $('#' + panelId).show();
            $('#exmodal').modal('show');
        }
        function ShowPopup() {
            $('#exmodal').modal('show');
        }

        function showPanel1(panelId) {
            $('#<%= pnl_stud.ClientID %>').hide();
            $('#<%= pnl_fac.ClientID %>').hide();
            $('#' + panelId).show();
            $('#exmodal1').modal('show');
        }
        function ShowPopup1() {
            $('#exmodal1').modal('show');
        }
        function closePopup1() {
            $('#exmodal1').modal('hide');
        }

        var x;
        function timecount() {
            clearInterval(x);
            var countDownDate = new Date().getTime() + (5 * 60 * 1000);
            x = setInterval(function () {
                var now = new Date().getTime();
                var distance = countDownDate - now;
                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);
                document.getElementById("timer").innerHTML = minutes + "m " + seconds + "s ";
                if (distance < 0) {
                    clearInterval(x);
                    document.getElementById("timer").innerHTML = "EXPIRED";
                }
            }, 1000);
        }
        timecount();

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

        .btn-dashboard {
            color: white;
            background-color: #2e2d2d;
        }

            .btn-dashboard:hover {
                background-color: #2e2d2d;
            }

        #ContentPlaceHolder1_GridView1, #ContentPlaceHolder1_GridView3, #ContentPlaceHolder1_GridView4, #ContentPlaceHolder1_GridView5 table th {
            color: white !important;
        }

        #ContentPlaceHolder1_GridView1_wrapper, #ContentPlaceHolder1_GridView3_wrapper, #ContentPlaceHolder1_GridView4_wrapper, #ContentPlaceHolder1_GridView5_wrapper .justify-content-between {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

        #ContentPlaceHolder1_GridView1_wrapper, #ContentPlaceHolder1_GridView3_wrapper, #ContentPlaceHolder1_GridView4_wrapper, #ContentPlaceHolder1_GridView5_wrapper .justify-content-md-center {
            margin-top: 0px !important;
        }

        #ContentPlaceHolder1_GridView1 thead th, #ContentPlaceHolder1_GridView3 thead th, #ContentPlaceHolder1_GridView4, #ContentPlaceHolder1_GridView5 thead th {
            text-align: center;
            font-size: 15px;
        }

        #ContentPlaceHolder1_GridView1 tbody td, #ContentPlaceHolder1_GridView3 tbody td, #ContentPlaceHolder1_GridView4, #ContentPlaceHolder1_GridView5 tbody td {
            text-align: center;
            color: black;
            font-size: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Issue Book</li>
        </ol>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
            <div class="modal" tabindex="-1" id="exmodal" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <div class="container-fluid">
                                <h5 class="modal-title mb-1" style="font-weight: bold">Instant Request</h5>
                            </div>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="container-fluid">
                                <asp:UpdatePanel Style="font-weight: 700" ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="row form-group mb-2">
                                            <label id="lblf1" runat="server" class="col-md-3 col-form-label">Roll No : </label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txt_id" Style="font-weight: 700; text-transform: uppercase" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-auto justify-content-center">
                                                <asp:Button ID="btn_code" Style="background-color: #006A05; color: white; font-weight: 600" runat="server" Text="Generate Code" CssClass="btn btn-sm" OnClick="btn_code_Click" />
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnl_code" Style="font-weight: 700" runat="server" CssClass="panel" Visible="false">
                                            <div class="row form-group mb-2">
                                                <label id="lblf2" runat="server" class="col-md-3 col-form-label">Code : <span class="text-danger">*</span></label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txt_code" Style="font-weight: 700" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4" style="margin-top: 4px;">
                                                    <i class="fas fa-clock"></i>
                                                    <span id="timer" style="padding-inline-start: 10px; background-color: darkred; color: white; padding-inline-end: 10px; border-radius: 8px; font-weight: 600; font-size: 0.8rem"></span>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button Style="background-color: #003da3; color: white; font-weight: 600" CssClass="btn btn-sm" runat="server" ID="btn_new" OnClick="btn_new_Click" Text="New User?" />
                            <button type="button" style="font-weight: 600" class="btn btn-sm btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal" tabindex="-1" id="exmodal1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <div class="container-fluid">
                                <h5 class="modal-title mb-1" style="font-weight: bold">Manual Issue</h5>
                            </div>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="container-fluid">
                                <asp:UpdatePanel Style="font-weight: 700" ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="row form-group mb-2">
                                            <div class="btn btn-group px-0 py-0 col-md-4">
                                                <asp:Button ID="btn_stud" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600; color: white" runat="server" OnClick="btn_stud_Click" Text="Students" />
                                                <asp:Button ID="btn_fac" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600; color: white" runat="server" OnClick="btn_fac_Click" Text="Faculty" />
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnl_stud" Style="font-weight: 600" runat="server" CssClass="panel" Visible="false">
                                            <div class="row form-group mb-2">
                                                <label runat="server" class="col-md-3 col-form-label">Roll No : <span class="text-danger">*</span></label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txt_roll" Style="font-weight: 500" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row form-group mb-2">
                                                <label runat="server" class="col-md-3 col-form-label">Book ID : <span class="text-danger">*</span></label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txt_bid" Style="font-weight: 500" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnl_fac" Style="font-weight: 600" runat="server" CssClass="panel" Visible="false">
                                            <div class="row form-group mb-2">
                                                <label runat="server" class="col-md-3 col-form-label">Faculty ID : <span class="text-danger">*</span></label>
                                                <div class="col-md-5">
                                                    <asp:DropDownList ID="ddl_fid" Style="font-weight: 500" CssClass="form-select" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row form-group mb-2">
                                                <label runat="server" class="col-md-3 col-form-label">Book ID : <span class="text-danger">*</span></label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txt_bkid" Style="font-weight: 500" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Button Style="background-color: #003da3; color: white; font-weight: 600" CssClass="btn btn-sm" runat="server" ID="btn_missue" OnClick="btn_missue_Click" Text="Issue Book" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" style="font-weight: 600" class="btn btn-sm btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!------------ FIELD ----------------------------------------------------------------------------------------------------->
            <asp:UpdatePanel ID="update1" runat="server">
                <ContentTemplate>
                    <div class="card" style="font-weight: 600">
                        <div class="card-body">
                            <div class="row form-group mb-2">
                                <h5>Book Issue
                                </h5>
                                <hr style="border: 1px solid black" />
                            </div>
                            <div class="row form-group mb-2">
                                <label class="col-md-auto col-form-label">
                                    Date :
                                </label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_date" Style="font-weight: 600" CssClass="form-control " runat="server" ReadOnly="true"></asp:TextBox>
                                </div>
                                <label class="col-md-auto col-form-label">
                                    Time Slot :
                                </label>
                                <div class="col-md-auto">
                                    <asp:DropDownList ID="ddl_slot" Style="font-weight: 600" CssClass="form-control form-select" data-toggle="dropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_slot_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button runat="server" OnClick="btn_instan_Click" Style="background-color: #003da3; color: white; font-weight: 600" CssClass="btn btn-sm" ID="btn_instan" CauseValidation="false" Text="Instant Request" />&nbsp;
                                    <asp:Button runat="server" OnClick="btn_manual_Click" Style="background-color: #003da3; color: white; font-weight: 600" CssClass="btn btn-sm" ID="btn_manual" CauseValidation="false" Text="Manual Issue" />&nbsp;
                                </div>
                            </div>
                            <section>
                                <div class="col-md-12 d-flex justify-content-between">
                                    <div class="btn btn-group px-0 py-0">
                                        <asp:Button ID="btn_student" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600; border-right: 3px solid white; border-radius: 3px; color: white" runat="server" OnClick="btn_student_Click" Text="Students" />
                                        <asp:Button ID="btn_teacher" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600;border-right: 3px solid white; border-radius: 3px; color: white" runat="server" OnClick="btn_teacher_Click" Text="Teachers" />
                                        <asp:Button ID="btn_instant" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600; border-right: 3px solid white; border-radius: 3px;color: white" runat="server" OnClick="btn_instant_Click" Text="Instant" />
                                        <asp:Button ID="btn_manually" CssClass="btn btn-sm btn-dashboard" Style="font-weight: 600; color: white" runat="server" OnClick="btn_manually_Click" Text="Manually Issued" />
                                    </div>
                                    <div>
                                        <asp:Button runat="server" OnClientClick="printGrid(); return false;" CssClass="btn btn-sm" ForeColor="White" BackColor="#460000" Font-Bold="true" Text="🖶 Print (PDF)" />
                                    </div>
                                </div>
                                <asp:Panel ID="pnl_student" runat="server" Visible="false">
                                    <div class="card p-2 table-responsive scroll" style="font-weight: 500">
                                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered dataTable1" OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="false">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RollNo" HeaderText="Roll No" ReadOnly="true" />
                                                <asp:BoundField DataField="StudentName" HeaderText="Student Name" ReadOnly="true" />
                                                <asp:BoundField DataField="BookName" HeaderText="Book Name" ReadOnly="true" />
                                                <asp:BoundField DataField="BookId" HeaderText="Book Id" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btn_Issue" Style="font-weight: 600; color: white" CssClass="btn btn-sm btn-success" runat="server" Text="Issue" CommandName="Issue" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Issue Book" OnClientClick='<%# "return IssueBook(\"student\"," + Container.DataItemIndex + ");" %>'></asp:LinkButton>
                                                        <asp:LinkButton ID="btn_Cancel" Style="font-weight: 600; color: white" CssClass="btn btn-sm btn-danger" runat="server" Text="Cancel" CommandName="CancelIssue" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Cancel Book" OnClientClick='<%# "return CancelRequest(\"student\"," + Container.DataItemIndex + ");" %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnl_teacher" runat="server" Visible="false">
                                    <div class="card p-2 table-responsive scroll" style="font-weight: 500">
                                        <asp:GridView ID="GridView3" runat="server" CssClass="table table-bordered dataTable1" OnRowCommand="GridView3_RowCommand" AutoGenerateColumns="false">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="facid" HeaderText="Faculty ID" ReadOnly="true" />
                                                <asp:BoundField DataField="FacultyName" HeaderText="Faculty Name" ReadOnly="true" />
                                                <asp:BoundField DataField="BookName" HeaderText="Book Name" ReadOnly="true" />
                                                <asp:BoundField DataField="BookId" HeaderText="Book Id" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btn_FIssue" Style="font-weight: 600; color: white" CssClass="btn btn-sm btn-success" runat="server" Text="Issue" CommandName="Issue" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Issue Book" OnClientClick='<%# "return IssueBook(\"faculty\"," + Container.DataItemIndex + ");" %>'></asp:LinkButton>
                                                        <asp:LinkButton ID="btn_FCancel" Style="font-weight: 600; color: white" CssClass="btn btn-sm btn-danger" runat="server" Text="Cancel" CommandName="CancelIssue" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Cancel Book" OnClientClick='<%# "return CancelRequest(\"faculty\"," + Container.DataItemIndex + ");" %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnl_instant" runat="server" Visible="false">
                                    <div class="card p-2 table-responsive scroll" style="font-weight: 500">
                                        <asp:GridView ID="GridView4" runat="server" CssClass="table table-bordered dataTable1" OnRowCommand="GridView4_RowCommand" AutoGenerateColumns="false">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RollNo" HeaderText="Roll No" ReadOnly="true" />
                                                <asp:BoundField DataField="StudentName" HeaderText="Student Name" ReadOnly="true" />
                                                <asp:BoundField DataField="BookName" HeaderText="Book Name" ReadOnly="true" />
                                                <asp:BoundField DataField="BookId" HeaderText="Book Id" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btn_Issue" Style="font-weight: 600; color: white" CssClass="btn btn-sm btn-success" runat="server" Text="Issue" CommandName="Issue" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Issue Book" OnClientClick='<%# "return IssueBook(\"instant\"," + Container.DataItemIndex + ");" %>'></asp:LinkButton>
                                                        <asp:LinkButton ID="btn_Cancel" Style="font-weight: 600; color: white" CssClass="btn btn-sm btn-danger" runat="server" Text="Cancel" CommandName="CancelIssue" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Cancel Book" OnClientClick='<%# "return CancelRequest(\"instant\"," + Container.DataItemIndex + ");" %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnl_manually" runat="server" Visible="false">
                                    <div class="card p-2 table-responsive scroll" style="font-weight: 500">
                                        <asp:GridView ID="GridView5" runat="server" CssClass="table table-bordered dataTable1" OnRowCommand="GridView5_RowCommand" AutoGenerateColumns="false">
                                            <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                            <FooterStyle BackColor="#3366CC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ISSUEDTO" HeaderText="Issued To" ReadOnly="true" />
                                                <asp:BoundField DataField="BookName" HeaderText="Book Name" ReadOnly="true" />
                                                <asp:BoundField DataField="BookId" HeaderText="Book Id" ReadOnly="true" />
                                                <%--<asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btn_delete" Style="font-weight: 600; color: white" CssClass="btn btn-sm btn-danger" runat="server" Text="Cancel Issue" CommandName="CancelIssue" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Cancel Book" OnClientClick='<%# "return CancelRequest(\"instant\"," + Container.DataItemIndex + ");" %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
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