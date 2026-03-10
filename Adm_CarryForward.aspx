<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_CarryForward.aspx.cs" Inherits="E_Granthalaya.Adm_CarryForward" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
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
    <style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">carry forward</li>
        </ol>
        <div class="card">
            <div class="card-body">
                <div class="row form-group">
                    <h5>Data Carry Forward</h5>
                    <hr style="border: 1px solid black" />
                </div>
                <div class="row form-group mb-3">
                    <div class="col-md-3">
                        <asp:Button ID="btn_update" runat="server" OnClick="btn_update_Click" Font-Bold="true" Text="♻ Sync Now" BackColor="Green" ForeColor="White" CssClass="btn btn-sm" />
                    </div>
                </div>

                <asp:Panel ID="panel_content" runat="server" Visible="false">
                        <div class="card p-2" Style="font-weight: 500">
                            <asp:GridView ID="GridView1" Style="width: 100%" CssClass="table table-bordered table-responsive dataTable" runat="server" AutoGenerateColumns="false">
                                <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                <FooterStyle BackColor="#3366CC" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TYPE" HeaderText="Type" />
                                    <asp:BoundField DataField="ESANCHALAN_CT" HeaderText="Esanchalan" />
                                    <asp:BoundField DataField="EGRANTHALAYA_CT" HeaderText="Egranthalaya" />
                                    <asp:BoundField DataField="CARRYFORWARD_CT" HeaderText="Carry Forward" />
                                    <asp:BoundField DataField="CREATED_ON" HeaderText="Date" DataFormatString="{0:d}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                </asp:Panel>
            </div>

        </div>
    </div>

</asp:Content>
