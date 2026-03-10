<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_Report.aspx.cs" Inherits="E_Granthalaya.Adm_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Report</title>
    <script type="text/javascript">
        $(document).ready(function () {
            reinitDataTable(); reinitDataTable2();
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function () { reinitDataTable(); })
                prm.add_endRequest(function () { reinitDataTable2(); })
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
        function reinitDataTable2() {
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
                toastr.error('No grid visible to print.');
            }
        }

        function exportGridToCSV() {
            var grids = document.querySelectorAll('.dataTable');
            var visibleGrid = null;
            for (var i = 0; i < grids.length; i++) {
                if (grids[i].offsetParent !== null) {
                    visibleGrid = grids[i];
                    break;
                }
            }

            if (visibleGrid !== null) {
                var rows = visibleGrid.querySelectorAll('tr');
                var csvData = [];

                rows.forEach(function (row) {
                    var cells = row.querySelectorAll('th, td');
                    var rowData = [];
                    cells.forEach(function (cell) {
                        rowData.push(cell.innerText);
                    });
                    csvData.push(rowData.join(','));
                });

                var csvString = csvData.join('\n');
                var encodedUri = 'data:text/csv;charset=utf-8,' + encodeURIComponent(csvString);

                var link = document.createElement("a");
                link.setAttribute("href", encodedUri);
                link.setAttribute("download", "visible_grid.csv");
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            } else {
                toastr.error('No grid visible to print.');
            }
        }
    </script>

    <style>
        /*
        #ContentPlaceHolder1_gridview,*/
        #ContentPlaceHolder1_gridview8,
        #ContentPlaceHolder1_gridview7,
        #ContentPlaceHolder1_gridview9,
        #ContentPlaceHolder1_gridview1,
        #ContentPlaceHolder1_gridview4,
        #ContentPlaceHolder1_gridview5,
        #ContentPlaceHolder1_gridview6,
        #ContentPlaceHolder1_gridview10,
        #ContentPlaceHolder1_gridview11,
        #ContentPlaceHolder1_gridview2,
        #ContentPlaceHolder1_gridview3,
        #ContentPlaceHolder1_finepaid,
        table th {
            color: white !important;
        }

        #ContentPlaceHolder1_gridview_wrapper,
        #ContentPlaceHolder1_gridview8_wrapper,
        #ContentPlaceHolder1_gridview7_wrapper,
        #ContentPlaceHolder1_gridview9_wrapper,
        #ContentPlaceHolder1_gridview1_wrapper,
        #ContentPlaceHolder1_gridview4_wrapper,
        #ContentPlaceHolder1_gridview5_wrapper,
        #ContentPlaceHolder1_gridview6_wrapper,
        #ContentPlaceHolder1_gridview10_wrapper,
        #ContentPlaceHolder1_gridview11_wrapper,
        #ContentPlaceHolder1_gridview2_wrapper,
        #ContentPlaceHolder1_gridview3_wrapper,
        #ContentPlaceHolder1_finepaid_wrapper {
            padding: 0rem 0rem 0.4rem 0rem !important;
        }

            #ContentPlaceHolder1_gridview_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview8_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview7_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview9_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview1_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview4_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview5_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview6_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview10_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview11_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview2_wrapper .justify-content-between,
            #ContentPlaceHolder1_gridview3_wrapper .justify-content-between,
            #ContentPlaceHolder1_finepaid_wrapper .justify-content-between {
                padding: 0rem 0rem 0.4rem 0rem !important;
            }

            #ContentPlaceHolder1_gridview_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview8_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview7_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview9_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview1_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview4_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview5_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview6_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview10_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview11_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview2_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_gridview3_wrapper .justify-content-md-center,
            #ContentPlaceHolder1_finepaid_wrapper .justify-content-md-center {
                margin-top: 0px !important;
            }

        #ContentPlaceHolder1_gridview thead th,
        #ContentPlaceHolder1_gridview8 thead th,
        #ContentPlaceHolder1_gridview7 thead th,
        #ContentPlaceHolder1_gridview9 thead th,
        #ContentPlaceHolder1_gridview1 thead th,
        #ContentPlaceHolder1_gridview4 thead th,
        #ContentPlaceHolder1_gridview5 thead th,
        #ContentPlaceHolder1_gridview6 thead th,
        #ContentPlaceHolder1_gridview10 thead th,
        #ContentPlaceHolder1_gridview11 thead th,
        #ContentPlaceHolder1_gridview2 thead th,
        #ContentPlaceHolder1_gridview3 thead th,
        #ContentPlaceHolder1_finepaid thead th {
            text-align: center;
        }

        #ContentPlaceHolder1_gridview tbody td,
        #ContentPlaceHolder1_gridview8 tbody td,
        #ContentPlaceHolder1_gridview7 tbody td,
        #ContentPlaceHolder1_gridview9 tbody td,
        #ContentPlaceHolder1_gridview1 tbody td,
        #ContentPlaceHolder1_gridview4 tbody td,
        #ContentPlaceHolder1_gridview5 tbody td,
        #ContentPlaceHolder1_gridview6 tbody td,
        #ContentPlaceHolder1_gridview10 tbody td,
        #ContentPlaceHolder1_gridview11 tbody td,
        #ContentPlaceHolder1_gridview2 tbody td,
        #ContentPlaceHolder1_gridview3 tbody td,
        #ContentPlaceHolder1_finepaid tbody td {
            text-align: center;
            color: black;
        }
        /*
        .customgrid table th {
            color: white !important;
        }

        .customgrid thead th {
            text-align: center;
        }

        .customgrid tbody td {
            text-align: center;
        }*/

        .scroll::-webkit-scrollbar {
            width: 0 !important
        }


        .gridview-row {
            cursor: pointer;
        }

            .gridview-row:hover {
                font-weight: 800;
            }

        .btn-primary-clicked {
            background-color: #0062cc;
            border-color: #0056b3;
            color: #fff;
        }

        .btn-custom {
            color: #CCFFFF;
            height: 44px;
            /* width: 93px;*/
            border-color: #0099FF;
            background-color: #0066FF;
            border-radius: 3px;
            font-weight: bold;
        }

            .btn-custom:hover {
                background-color: #004bb5; /* Darker shade for hover */
                border-color: #007bff;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <h3 class="mt-2"></h3>
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Generate Report</li>
        </ol>
        <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="update1" runat="server">
            <ContentTemplate>
                <div class="card" style="font-weight: 500">
                    <div class="card-body">
                        <div class="row form-group">
                            <h5>Report Details</h5>
                            <hr style="border: 1px solid black" />
                        </div>
                        <div class="row form-group mb-2">
                            <label class="col-md-auto col-form-label">From Date:<span class="text-danger">*</span></label>
                            <div class="col-md-2">
                                <asp:TextBox ID="txt_fromdate" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <label class="col-md-auto col-form-label">To Date:<span class="text-danger">*</span></label>
                            <div class="col-md-2">
                                <asp:TextBox ID="txt_todate" Style="font-weight: 500" runat="server" AutoComplete="off" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>

                            <label class="col-md-auto col-form-label">Report Name:<span class="text-danger">*</span></label>
                            <div class="col-md-5">
                                <asp:DropDownList ID="ddl_report" Style="font-weight: 500" CssClass="form-control form-select" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_report_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="container">

                                <div class="row form-group mb-2 mt-4">
                                    <div class="col d-flex justify-content-center">
                                        <asp:Label ID="lbl_ddl" runat="server" Style="margin-left: auto; font-size: 20px" Font-Bold="true" Text=""></asp:Label>
                                    </div>
                                    <div class="col d-flex justify-content-end">
                                        <asp:Button runat="server" OnClientClick="printGrid(); return false;" CssClass="btn btn-sm" ForeColor="White" BackColor="#460000" Font-Bold="true" Text="🖶 Print (PDF)" />&nbsp;
                                        <asp:Button runat="server" OnClientClick="exportGridToCSV(); return false;" CssClass="btn btn-sm" ForeColor="White" BackColor="#460000" Font-Bold="true" Text="🖶 Export (xls)" />
                                    </div>
                                </div>
                            </div>


                        </div>
                        <asp:Panel ID="pnl_shrink" runat="server" Visible="false">
                            <div class="container-fluid py-0 px-0">
                                <div class="card p-2 scroll">
                                    <asp:Button runat="server" ID="btn_open" Font-Bold="true" OnClick="btn_open_Click" Text="Open Previous" CssClass="btn btn-sm btn-light" />
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_shrink2" runat="server" Visible="false">
                            <div class="container-fluid py-0 px-0">
                                <div class="card p-2 scroll">
                                    <asp:Button runat="server" ID="btn_open2" OnClick="btn_open2_Click" Font-Bold="true" Text="Open Previous" CssClass="btn btn-sm btn-light" />
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_shrink3" runat="server" Visible="false">
                            <div class="container-fluid py-0 px-0">
                                <div class="card p-2 scroll">
                                    <asp:Button runat="server" ID="btn_open3" OnClick="btn_open3_Click" Font-Bold="true" Text="Open Previous" CssClass="btn btn-sm btn-light" />
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="col"></div>
                        <asp:Panel ID="pnl_books" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="False" OnRowDataBound="gridview_RowDataBound" OnSelectedIndexChanged="gridview_SelectedIndexChanged">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="COURSE_NAME" HeaderText="COURSE NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="TOTAL_BOOKS" HeaderText="TOTAL BOOKS" ReadOnly="true" />
                                        <asp:BoundField DataField="ISSUED_BOOKS" HeaderText="ISSUED BOOKS" ReadOnly="true" />
                                        <asp:BoundField DataField="AVAILABLE_BOOKS" HeaderText="AVAILABLE BOOKS" ReadOnly="true" />
                                        <asp:BoundField DataField="DISCARDED_BOOKS" HeaderText="DISCARDED BOOKS" ReadOnly="true" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnl_finepaid" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <div class="row form-group mb-2">
                                    <asp:Label ID="lbl_f1" Font-Bold="true" runat="server"></asp:Label>
                                </div>
                                <asp:GridView ID="finepaid" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="False" Visible="true">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="OV_ID" HeaderText="ACCESSION ID" ReadOnly="true" />
                                        <asp:BoundField DataField="BOOKNAME" HeaderText="BOOK NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="STUDENTNAME" HeaderText="STUDENT NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="COURSENAME" HeaderText="COURSE" ReadOnly="true" />
                                        <asp:BoundField DataField="ISSUEDATE" HeaderText="ISSUEDATE" ReadOnly="true" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="DUEDATE" HeaderText="DUEDATE" ReadOnly="true" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="RETURNDATE" HeaderText="RETURNDATE" ReadOnly="true" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="FINE" HeaderText="FINE" ReadOnly="true" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnl_bookdetails" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview8" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="False">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SEMESTER_SUBJECT" HeaderText="SUBJECT" ReadOnly="true" />
                                        <asp:BoundField DataField="PUBLISHER_NAME" HeaderText="PUBLISHER NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="BOOK_COUNT" HeaderText=" BOOKS COUNT" ReadOnly="true" />

                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_bookscost" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview7" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable1" AutoGenerateColumns="false" OnRowDataBound="gridview7_RowDataBound" OnSelectedIndexChanged="gridview7_SelectedIndexChanged">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="COURSE_NAME" HeaderText="COURSE NAME " ReadOnly="true" />
                                        <asp:BoundField DataField="ACTIVE_COST" HeaderText="ACTIVE BOOK COST" ReadOnly="true" />
                                        <asp:BoundField DataField="DISCARDED_COST" HeaderText="DISCARDED BOOK COST" ReadOnly="true" />

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_bookdetails2" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview9" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable1" AutoGenerateColumns="False">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SEMESTER_SUBJECT" HeaderText="SUBJECT" ReadOnly="true" />
                                        <asp:BoundField DataField="PUBLISHER_NAME" HeaderText="PUB NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="TOTAL_COST" HeaderText=" TOTAL COUNT" ReadOnly="true" />

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <%--     <asp:Panel ID="pnl_issue" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview1" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="STUDENTNAME" HeaderText="STUDENT NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="ISSUEDBOOKSCOUNT" HeaderText="ISSUED BOOK COUNT" ReadOnly="true" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>--%>
                        <asp:Panel ID="pnl_return" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview4" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="STUDENTNAME" HeaderText="STUDENT NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="RETURNEDBOOKSCOUNT" HeaderText="RETURNED BOOK COUNT" ReadOnly="true" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_cancel" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview5" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="STUDENTNAME" HeaderText="STUDENT NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="CANCELEDBOOKSCOUNT" HeaderText="CANCELED BOOK COUNT" ReadOnly="true" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_fine" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview6" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="STUDENTNAME" HeaderText="STUDENT NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="TOTALFINE" HeaderText="TOTAL FINE  " ReadOnly="true" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_subs" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview10" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid  dataTable " AutoGenerateColumns="False" OnRowDataBound="gridview10_RowDataBound" OnSelectedIndexChanged="gridview10_SelectedIndexChanged">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="JOURNAL_NAME" HeaderText="JOURNAL NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="PERIODICITY" HeaderText="PERIODICITY" ReadOnly="true" />
                                        <asp:BoundField DataField="TOTAL_ENTRY" HeaderText=" TOTAL ENTRY" ReadOnly="true" />
                                        <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="AMOUNT" ReadOnly="true" />

                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_details3" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview1" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable1" AutoGenerateColumns="False">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField DataField="JOURNAL_ID" HeaderText="JOURNAL ID" ReadOnly="true" />--%>
                                        <asp:BoundField DataField="JOURNAL_NAME" HeaderText="JOURNAL NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="PERIODICITY" HeaderText="PERIODICITY " ReadOnly="true" />
                                        <asp:BoundField DataField="CATEGORYNAME" HeaderText=" CATEGORY " ReadOnly="true" />
                                        <asp:BoundField DataField="PERIOD" HeaderText=" PERIOD " ReadOnly="true" />
                                        <asp:BoundField DataField="SUBS_DATE" HeaderText=" ENTRY DATE" DataFormatString="{0:d}" ReadOnly="true" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_stream" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview11" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="False">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BOOKID" HeaderText="BOOK ID" ReadOnly="true" />
                                        <asp:BoundField DataField="OV_ID" HeaderText="ACCESSION ID" ReadOnly="true" />
                                        <asp:BoundField DataField="ROLLNO" HeaderText="ROLL NO" ReadOnly="true" />
                                        <asp:BoundField DataField="NAME" HeaderText="STUDENT NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="BOOKNAME" HeaderText="BOOK NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="STREAM" HeaderText=" STREAM " ReadOnly="true" />

                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_discarded" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview12" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="False">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BOOKID" HeaderText="BOOK ID" ReadOnly="true" />
                                        <asp:BoundField DataField="OV_ID" HeaderText="ACCESSION ID" ReadOnly="true" />
                                        <asp:BoundField DataField="BOOKNAME" HeaderText="  BOOK NAME  " ReadOnly="true" />
                                        <asp:BoundField DataField="STREAM" HeaderText="  STREAM  " ReadOnly="true" />

                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_coursecount" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview13" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="False">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="COURSE_NAME" HeaderText="COURSE NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="RETURNED_BOOKS" HeaderText="RETURNED BOOKS" ReadOnly="true" />
                                        <asp:BoundField DataField="CANCEL_BOOKS" HeaderText="CANCEL_BOOKS" ReadOnly="true" />
                                        <asp:BoundField DataField="FINE" HeaderText="FINE" ReadOnly="true" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_purchase" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <div class="row form-group mb-2">
                                    <asp:Label ID="Label1" Font-Bold="true" runat="server"></asp:Label>
                                </div>
                                <asp:GridView ID="GridView14" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="False" Visible="true">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BILL_ID" HeaderText="BILL ID" ReadOnly="true" />
                                        <asp:BoundField DataField="BOOK_ID" HeaderText="BOOK ID" ReadOnly="true" />
                                        <asp:BoundField DataField="OV_ID" HeaderText="ACCESSION ID" ReadOnly="true" />
                                        <asp:BoundField DataField="BOOKNAME" HeaderText="BOOK NAME" ReadOnly="true" />
                                        <asp:BoundField DataField="NO_OFCOPIES" HeaderText="BOOK COPIES" ReadOnly="true" />
                                        <asp:BoundField DataField="DISCOUNT" HeaderText="DISCOUNTED AMT" ReadOnly="true" />
                                        <asp:BoundField DataField="COST" HeaderText="TOTAL COST" ReadOnly="true" />
                                        <asp:BoundField DataField="ACTUALAMOUNT" HeaderText="NET AMOUNT" ReadOnly="true" />
                                        <asp:BoundField DataField="COSTRS" HeaderText="BOOK COST(PER)" ReadOnly="true" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_subsdate" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview2" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SUBS_DATE" HeaderText="Entry Dates" ReadOnly="true" DataFormatString="{0:d}" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_entries" runat="server" Visible="false">
                            <div class="card p-2 scroll table-responsive" style="font-weight: 500">
                                <asp:GridView ID="gridview3" Style="width: 100%" runat="server" CssClass="table table-bordered customgrid dataTable" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#3366CC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SUBS_DATE" HeaderText="Entry Dates" ReadOnly="true" DataFormatString="{0:d}" />
                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnl_nodata" runat="server" Visible="false">
                            <div class="container-fluid py-0 px-0">
                                <div class="card p-2 scroll">
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
    </div>
</asp:Content>
