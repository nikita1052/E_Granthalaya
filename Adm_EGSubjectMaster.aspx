<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Adm_EGSubjectMaster.aspx.cs" Inherits="E_Granthalaya.Adm_EGSubjectMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>EgSubject</title>
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

        .circlebtn {
            margin-top: 4px;
            font-weight: 900;
            border-radius: 100px;
            padding-top: 0px;
            padding-bottom: 2px;
            font-size: 1.1rem
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Adm_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">EG Subject</li>
        </ol>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
            <div class="modal" id="exmodal" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Add Subject Name</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <h5>Subject Name : </h5>
                            <asp:TextBox ID="txt_addsubject" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btn_addsubject" runat="server" class="btn btn-success" Text="Submit" OnClick="btn_addsubject_Click" data-bs-dismiss="modal" CausesValidation="False" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel ID="update1" runat="server">
                <ContentTemplate>
                    <div class="card">
                        <div class="card-body">
                            <div class="row form-group">
                                <h5>EG Subject Details</h5>
                                <hr style="border: 1px solid black" />
                            </div>
                            <asp:Panel ID="pnl_grid" runat="server" Visible="false">
                                <div class="card p-2 table-responsive scroll" style="font-weight: 500">
                                    <asp:GridView ID="GridView1" Style="width: 100%" runat="server" CssClass="table table-bordered " OnRowDataBound="GridView1_RowDataBound" AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="COURSE_NAME" HeaderText="Course Name" ReadOnly="true" />
                                            <asp:BoundField DataField="SEMESTER" HeaderText="Semester" ReadOnly="true" />
                                            <asp:BoundField DataField="SUBJECT_NAME" HeaderText="Subject Name" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Link With">
                                                <ItemTemplate>
                                                    <div class="d-flex">
                                                        <asp:DropDownList ID="ddl_select" DataTextField="SUBJECT_NAME" Style="font-weight: 500" DataValueField="SUBEJECT_ID" runat="server" CssClass="d-flex form-select"></asp:DropDownList>&nbsp;
                                                        <asp:Button ID="btn_addcon" BackColor="#003da3" Font-Bold="true" CssClass="btn circlebtn btn-sm btn-primary" data-bs-toggle="modal" data-bs-target="#exmodal" align="center" runat="server" Text="+" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Save Data">
                                                <ItemTemplate>
                                                    <asp:Button OnClick="btn_save_Click1" ID="btn_save" BackColor="Green" Font-Bold="true" runat="server" Text="Save" CssClass="btn btn-success" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                    <hr />
                                    <div class="d-flex justify-content-center">
                                        <asp:Button ID="btn_save" OnClick="btn_save_Click" BackColor="Green" Font-Bold="true" runat="server" Text="Save Data" CssClass="btn btn-success" />
                                    </div>
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
                                        <asp:Label ID="lbl_not" runat="server" class="d-flex justify-content-center" Style="font-weight: 700; font-size: 1.2rem" Text="No Subjects Found!"></asp:Label>
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
