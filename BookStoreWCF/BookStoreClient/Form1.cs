using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BookStoreClient
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private const string BaseUrl = "http://localhost:8733/BookStoreService/books";
        private bool _isLoadingData = false;

        public Form1()
        {
            InitializeComponent();
            LoadBooks();
        }

        private async void LoadBooks()
        {
            try
            {
                _isLoadingData = true;
                var response = await client.GetStringAsync(BaseUrl);
                var books = JsonConvert.DeserializeObject<List<Book>>(response);
                dataGridView1.DataSource = books;
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading books: " + ex.Message);
            }
            finally
            {
                _isLoadingData = false;
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields(requireISBN: true)) return;

            var book = new Book
            {
                Title = txtTitle.Text.Trim(),
                Author = txtAuthor.Text.Trim(),
                ISBN = txtISBN.Text.Trim(),
                Price = decimal.Parse(txtPrice.Text),
                Stock = int.Parse(txtStock.Text),
                PublishedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd")
            };

            try
            {
                var json = JsonConvert.SerializeObject(book);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(BaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Book added successfully!");
                    ClearFields();
                    LoadBooks();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Error: " + error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to update.");
                return;
            }

            var selectedBook = dataGridView1.SelectedRows[0].DataBoundItem as Book;
            if (selectedBook == null) return;

            if (!ValidateFields(requireISBN: false)) return;

            var book = new Book
            {
                Id = selectedBook.Id,
                Title = txtTitle.Text.Trim(),
                Author = txtAuthor.Text.Trim(),
                ISBN = txtISBN.Text.Trim(),
                Price = decimal.Parse(txtPrice.Text),
                Stock = int.Parse(txtStock.Text),
                PublishedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd")
            };

            try
            {
                var json = JsonConvert.SerializeObject(book);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(BaseUrl + "/" + selectedBook.Id, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Book updated successfully!");
                    ClearFields();
                    LoadBooks();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Error: " + error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to delete.");
                return;
            }

            var selectedBook = dataGridView1.SelectedRows[0].DataBoundItem as Book;
            if (selectedBook == null) return;

            var confirm = MessageBox.Show(
                "Are you sure you want to delete \"" + selectedBook.Title + "\"?",
                "Confirm Delete",
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes) return;

            try
            {
                var response = await client.DeleteAsync(BaseUrl + "/" + selectedBook.Id);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Book deleted successfully!");
                    ClearFields();
                    LoadBooks();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Error: " + error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadBooks();
                return;
            }

            try
            {
                var response = await client.GetStringAsync(
                    BaseUrl + "/search?query=" + Uri.EscapeDataString(txtSearch.Text));
                var books = JsonConvert.DeserializeObject<List<Book>>(response);
                dataGridView1.DataSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            LoadBooks();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            if (dataGridView1.SelectedRows.Count == 0) return;

            var book = dataGridView1.SelectedRows[0].DataBoundItem as Book;
            if (book == null) return;

            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            txtISBN.Text = book.ISBN;
            txtPrice.Text = book.Price.ToString();
            txtStock.Text = book.Stock.ToString();
            dateTimePicker1.Value = DateTime.TryParse(book.PublishedDate, out DateTime dt)
                ? dt
                : DateTime.Now;
        }

        private void ClearFields()
        {
            txtTitle.Text = "";
            txtAuthor.Text = "";
            txtISBN.Text = "";
            txtPrice.Text = "";
            txtStock.Text = "";
            txtSearch.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dataGridView1.ClearSelection();
        }

        private bool ValidateFields(bool requireISBN)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("Title and Author are required.");
                return false;
            }

            if (requireISBN && string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show("ISBN is required.");
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.");
                return false;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Please enter a valid stock number.");
                return false;
            }

            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}