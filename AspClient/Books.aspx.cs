using AspClient.Services;
using System;

namespace AspClient {
    public partial class Books : System.Web.UI.Page {
        private readonly BookService _bookService = new BookService();

        protected async void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                var books = await _bookService.GetAllAsync();
                BooksGridView.DataSource = books;
                BooksGridView.DataBind();
            }
        }
    }
}