<%@ Page Title="" Language="C#" MasterPageFile="~/Stud_Master.Master" AutoEventWireup="true" CodeBehind="Stud_Econtents.aspx.cs" Inherits="E_Granthalaya.Stud_Econtents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Student Contents</title>
    <style>
        
        .scroll::-webkit-scrollbar {
            width: 0 !important
        }
        .color-new {
            color: #003da3 !important
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-4">
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Stud_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Explore Contents</li>
        </ol>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="update1" runat="server">
                <ContentTemplate>
                    <div class="card" style="font-weight:500">
                        <div class="card-body">
                            <div class="row form-group">
                                <h5>Explore Contents</h5>
                                <hr style="border: 1px solid black" />
                            </div>
                            <%-- <div class="row form-group mb-2">
                                <a href="assets/images/defaultbook.jpg">Image</a>
                            </div>--%>
                            <div class="row form-group mb-2">
                                <label class="col-md-auto col-form-label">Course : <span class="text-danger">*</span></label>
                                <div class="col-md-auto">
                                    <asp:DropDownList CssClass="form-control form-select" Style="font-weight: 500" ID="ddl_course" runat="server" OnSelectedIndexChanged="ddl_course_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                </div>
                                <label class="col-md-auto col-form-label">Subject: <span class="text-danger">*</span></label>
                                <div class="col-md-auto ">
                                    <asp:DropDownList CssClass="form-control form-select" Style="font-weight: 500" ID="ddl_subject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_subject_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <label class="col-md-auto col-form-label" visible="false">Content Type: <span class="text-danger">*</span></label>
                                <div class="col-md-auto">
                                    <asp:DropDownList CssClass="form-control form-select" Style="font-weight: 500" ID="ddl_contenttype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_contenttype_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <asp:Panel ID="pnl_grid" runat="server">
                                <div class="card p-2 table-responsive scroll" style="font-weight:500">
                                    <asp:GridView ID="GridView1" style="width:100%" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#003da3" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" />
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
                                            <asp:BoundField DataField="FILENAME" HeaderText="File" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btn_view" CssClass="table-link color-new" runat="server" Text="view" ToolTip="View record" OnClick="btn_view_Click">
                                                            <span class="fa-stack">
                                                                <i class="fa fa-square fa-stack-2x"></i>
                                                                <i class="fa fa-eye fa-stack-1x fa-inverse"></i>
                                                            </span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle HorizontalAlign="Center" />
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
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>