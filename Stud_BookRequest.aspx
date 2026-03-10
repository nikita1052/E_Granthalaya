<%@ Page Title="" Language="C#" MasterPageFile="~/Stud_Master.Master" AutoEventWireup="true" CodeBehind="Stud_BookRequest.aspx.cs" Inherits="E_Granthalaya.Stud_BookRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Book Request</title>
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
        #ContentPlaceHolder1_GridView1 thead th {
            text-align: center;
        }

        #ContentPlaceHolder1_GridView1 tbody td {
            text-align: center;
        }

        .scrollable-panel {
            height: 400px;
            overflow-y: scroll;
            overflow-x: hidden;
        }

        .scroll::-webkit-scrollbar {
            width: 0 !important
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type='text/javascript'>
        function ShowPopup(bookId) {
            var imagepath = 'ImageHandler.ashx?linkedwith=' + bookId;
            // Update the src attribute of the image with the imagePath
            $('#bookImage').attr('src', imagepath);
            $("#booktype").modal("show");
        }
        function showmodal() {
            $("#modalotp").modal("show");
        }
        function closemodal() {
            $('#modalotp').modal('hide');
            $('.modal-backdrop').hide();
        }
        function closeModal() {
            $('#booktype').modal('hide');
            $('.modal-backdrop').hide();
        }
        function checkboxClicked() {
            // Trigger postback to the server
            __doPostBack('chk_viewMode', '');
        }
    </script>
    <div class="container-fluid px-4">
        <h3 class="mt-2"></h3>
        <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><a href="Stud_Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Add Book Request</li>
        </ol>
        <asp:Panel ID="panel_content" runat="server" Visible="true">
            <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="update1" runat="server">
                <ContentTemplate>
                    <div class="modal" tabindex="-1" id="booktype" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Request Details</h5>
                                    <%--  <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>--%>
                                </div>
                                <div class="modal-body" style="font-weight: 500">
                                    <div class="container-fluid">
                                        <div style="text-align: center; margin-top: 0;">
                                            <img id="bookImage" src="assets/images/bgimg-library.jpg" alt="Book Image" style="width: 100px; height: 120px; border: 10px solid #87CEEB;" />
                                        </div>
                                        <div class="form-group mb-1">
                                            <asp:Label ID="lblstudent" runat="server" Font-Bold="true" Text="Book ID : "></asp:Label>
                                            <asp:Label ID="lblbookid" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="form-group mb-1">
                                            <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Book Name : "></asp:Label>
                                            <asp:Label ID="lblbookname" runat="server" Text="" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="form-group mb-1">
                                            <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Publisher Name : "></asp:Label>
                                            <asp:Label ID="lbl_publisher" runat="server" Text="" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="form-group mb-1">
                                            <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Course : "></asp:Label>
                                            <asp:Label ID="lbl_course" runat="server" Text="" CssClass="form-label"></asp:Label>
                                            - 
                                            <asp:Label ID="lbl_sem" runat="server" CssClass="form-label" Text=""></asp:Label>
                                        </div>
                                        <div class="form-group mb-1">
                                            <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Subject : "></asp:Label>
                                            <asp:Label ID="lbl_subject" runat="server" Text="" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="form-group mb-1">
                                            <asp:Label ID="Label6" runat="server" Font-Bold="true" Text="Time Slot : "></asp:Label>
                                            <asp:Label ID="lbl_timeslot" runat="server" Text="" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="form-group mb-1">
                                            <asp:Label ID="Label9" runat="server" Font-Bold="true" Text="Book Status : "></asp:Label>
                                            <asp:Label ID="lbl_status" runat="server" Text="" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="form-group mb-1">
                                            <div class="alert alert-info d-flex align-items-center" role="alert">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-exclamation-triangle-fill flex-shrink-0 me-2" viewBox="0 0 16 16" role="img" aria-label="Warning:">
                                                    <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z" />
                                                </svg>
                                                <div style="margin-left: 10px;">
                                                    <asp:Label Visible="false" ID="lbl_information" runat="server" Text="" CssClass="col-form-label" Font-Bold="true" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:LinkButton ID="btn_instant" CssClass="btn btn-primary" BackColor="#003da3" Font-Bold="true" ForeColor="White" runat="server" Visible="true" OnClick="btn_instantClick">Instant request</asp:LinkButton>
                                    <asp:LinkButton ID="btn_save" CssClass="btn btn-primary" BackColor="#003da3" Font-Bold="true" ForeColor="White" runat="server" Text="Request Book" OnClick="btn_saveClick"></asp:LinkButton>
                                    <button type="button" class="btn btn-secondary" font-bold="true" forecolor="White" data-bs-dismiss="modal">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal" id="modalotp" tabindex="-1">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Verify Code</h5>
                                    <%--<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>--%>
                                </div>
                                <div class="modal-body" style="font-weight: 500">
                                    <div class="form-group mb-2">
                                        <asp:Label ID="Label5" runat="server" Font-Bold="true" Text="Enter Code:- "></asp:Label>
                                        <asp:TextBox ID="txt_code" Style="font-weight: 500" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btn_close" class="btn btn-secondary" Font-Bold="true" ForeColor="White" OnClick="btn_closeClick" runat="server" Text="Close" />
                                    <asp:Button ID="btn_instantsave" class="btn btn-primary" BackColor="#003da3" Font-Bold="true" ForeColor="White" OnClick="btn_instantsaveClick" runat="server" Text="Verify And Request" />

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-body" style="font-weight: 500">
                            <div class="row form-group justify-content-between">
                                <div class="col-10">
                                    <h5>Book Request</h5>
                                </div>
                                <%-- <label class="col-md-auto col-form-label">View Mode : </label>--%>
                                <div class="col-md small">
                                    <div class="form-check form-switch">
                                        <input class="form-check-input" type="checkbox" runat="server" autopostback="true" checked="checked" id="chk_viewMode" onchange="checkboxClicked()" disabled>
                                        <label class="form-check-label" for="chk_viewMode">Card View</label>
                                    </div>
                                </div>
                                <hr style="border: 1px solid black" />
                            </div>
                            <div class="row form-group mb-2">
                                <label class="col-md-1 col-form-label">Course : <span class="text-danger">*</span></label>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddl_course" Style="font-weight: 500" CssClass="form-control form-select" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_courseSelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <asp:Label runat="server" ID="lable" class="col-md-1 col-form-label"  >Subject<span class="text-danger">*</span>: </asp:Label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddl_subject" Style="font-weight: 500;" CssClass="form-control form-select" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_subjectSelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <asp:Label ID="lable1" runat="server"  class="col-md-1 col-form-label" Visible="false">Category:<span class="text-danger">*</span></asp:Label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddl_category" Visible="false" Style="font-weight: 500;" CssClass="form-control form-select" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_category_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                            </div>
                            <div class="row form-group mb-2">
                                <asp:Label ID="Label7" class="col-md-1 col-form-label" runat="server">Time Slot : </asp:Label>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddl_timeslot" Style="font-weight: 500" CssClass="form-control form-select" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_timeslotSelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter Company Address" Display="Dynamic" ControlToValidate="ddl_timeslot" ForeColor="red"></asp:RequiredFieldValidator>
                                </div>
                                <label class="col-md-1 col-form-label">Publisher :<span class="text-danger">*</span></label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddl_publisher" Style="font-weight: 500" CssClass="form-control form-select" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_publisherSelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>

                            <asp:Panel ID="pnl_grid" runat="server" Visible="false">
                                <div class="card p-2">
                                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-responsive dataTable" DataKeyNames="Book_ID" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">
                                        <HeaderStyle BackColor="#0066FF" Font-Bold="true" ForeColor="White" />
                                        <FooterStyle BackColor="#3366CC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Book Icon">
                                                <ItemTemplate>
                                                    <i class="fa-solid fa-book" style="height: 30px; width: 30px; align-self: flex-start; color: #790000; margin-bottom: 0px;"></i>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Book Photo">
                                                <ItemTemplate>
                                                    <%--<asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageURL(Eval("BOOK_PHOTO")) %>' AlternateText='<%# Eval("BOOK_PHOTO") %>' Style="width: 30px; height: 30px; border-radius: 10px; border: 1px solid black;" />--%>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# "ImageHandler.ashx?linkedwith=" + Eval("linkedwith") %>' Style="width: 30px; height: 30px; border-radius: 10px; border: 1px solid black;" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BOOKNAME" HeaderText="Book Title" />
                                            <asp:BoundField DataField="PUBLISHERNAME" HeaderText="Publisher Name" />
                                            <asp:BoundField DataField="DESCRIPTION" HeaderText="Book Status" />
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="btn_save" CssClass="table-link text-danger" runat="server" Text="Request" OnClick="btn_saveClick"></asp:LinkButton>--%>
                                                    <asp:LinkButton ID="btn_view" CssClass="table-link text-primary" runat="server" Text="View" ToolTip="View record" CommandName="View" OnClick="btn_view1Click" CausesValidation="False"> </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnl_card" runat="server" Visible="false">
                                <div class="card p-2 scrollable-panel">
                                    <div class="row">
                                        <asp:Repeater ID="Repeater1" runat="server">
                                            <ItemTemplate>
                                                <div class="col-md-4 mb-2">
                                                    <div class="card" style='<%# GetBackgroundColor1(Eval("DESCRIPTION").ToString()) %>; <%# GetBackgroundColor2(Eval("DESCRIPTION").ToString()) %>; text-transform: capitalize'>
                                                        <div class="card-body small">
                                                            <div class="row">
                                                                <div class="col-4 col-md-3">
                                                                    <asp:Image ID="Image2" runat="server" ImageUrl='<%# "ImageHandler.ashx?linkedwith=" + Eval("linkedwith") %>' class="img-fluid rounded " Style="border: 3px solid #87CEEB" />
                                                                </div>
                                                                <div class="col-8 col-md-9">
                                                                    <div class="small" style="margin-bottom: 0.5rem"><b><%#Eval("BOOKNAME") %></b></div>
                                                                    <div class="small" style="margin-bottom: 0.5rem"><b>Publisher: <%#Eval("PUBLISHERNAME") %></b></div>
                                                                    <div class="text small" style="margin-bottom: 4px">
                                                                        <b><%#Eval("DESCRIPTION") %></b>
                                                                    </div>
                                                                    <asp:Button ID="btn_viewInfo" BackColor="#003da3" Font-Bold="true" ForeColor="White" runat="server" Text="View Details" CssClass="btn btn-sm" Style='<%# GetBackgroundColor3(Eval("DESCRIPTION").ToString()) %>' CommandArgument='<%# string.Concat(Eval("Book_ID"), "|", Eval("BOOKNAME"), "|", Eval("PUBLISHERNAME"), "|", Eval("DESCRIPTION")) %>' OnClick="btn_viewInfo_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
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
                                        <asp:Label ID="lbl_not" runat="server" class="d-flex justify-content-center" Style="font-weight: 700; font-size: 1.2rem" Text="No Books Available Now.."></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="chk_viewMode" EventName="click" />--%>
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>