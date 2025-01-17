﻿using libraryMeneger.book;
using libraryMeneger.Data.BookRepository;
using libraryMeneger.Data.BorrowHistory;
using libraryMeneger.Data.StatusRepository;
using libraryMeneger.Data.UserRepository;
using libraryMeneger.user;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace libraryMeneger
{
    public partial class ReturnBookForm : Form
    {
        public AdminUser adminUser { get; private set; }
        public bool IsChanged { get; private set; }
        BooksRepository repository = new BooksRepository();
        StatusRepository statRepository = new StatusRepository();
        BorrowHistoryRepository borrowRepository = new BorrowHistoryRepository();

        public ReturnBookForm(AdminUser user)
        {
            InitializeComponent();
            adminUser = user;
            IsChanged = false;

            this.userEmailLabel.Visible = false;
            this.currEmailLabel.Visible = false;
            this.currEmailLabel.ForeColor = Color.Red;

            string Title;
            string UserLogin;
            List<BookStatManager> issuedBooks = statRepository.getIssuedBookInfo();

            if (issuedBooks != null)
            {
                this.issuedComboBox.Items.Add("Select the issued:");
                foreach (BookStatManager issuedBook in issuedBooks)
                {
                    Title = repository.getBookTitle(issuedBook.Article);
                    UserLogin = statRepository.getUserIDByArticle(issuedBook.Article);
                    this.issuedComboBox.Items.Add(issuedBook.GetToStringStatus(Title, UserLogin));
                }
            }
            else
            {
                this.issuedComboBox.Items.Add("The library has no issued books!");
            }
            this.issuedComboBox.SelectedIndex = 0;
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            string Title;
            string UserLogin = this.filterTextBox.Text;
            this.issuedComboBox.Items.Clear(); // очистка поточного комбача
            if (UserLogin.Length != 0) // Якщо логін не пустий то перейте до видобування орендованих за юзером
            {
                List<BookStatManager> issuBooksForUser = statRepository.getStatManagersByUser(UserLogin);
                if (issuBooksForUser != null)
                {
                    this.issuedComboBox.Items.Add("Select the reserve:");
                    foreach (BookStatManager issuUserBook in issuBooksForUser)
                    {
                        if (issuUserBook.IssueStatus == true)
                        {
                            Title = repository.getBookTitle(issuUserBook.Article);
                            UserLogin = statRepository.getUserIDByArticle(issuUserBook.Article);
                            this.issuedComboBox.Items.Add(issuUserBook.GetToStringStatus(Title, UserLogin));
                        }
                    }
                }
                else
                {
                    this.issuedComboBox.Items.Add("The library has no reserved books!");
                }
            }
            else
            {
                List<BookStatManager> issuedBooks = statRepository.getIssuedBookInfo();
                if (issuedBooks != null)
                {
                    this.issuedComboBox.Items.Add("Select the issued:");
                    foreach (BookStatManager issuedBook in issuedBooks)
                    {
                        Title = repository.getBookTitle(issuedBook.Article);
                        UserLogin = statRepository.getUserIDByArticle(issuedBook.Article);
                        this.issuedComboBox.Items.Add(issuedBook.GetToStringStatus(Title, UserLogin));
                    }
                }
                else
                {
                    this.issuedComboBox.Items.Add("The library has no issued books!");
                }
            }
            this.issuedComboBox.SelectedIndex = 0;
        }

        private void issuedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.issuedComboBox.SelectedIndex != 0)
            {
                UserRepository userRepository = new UserRepository();
                int article = int.Parse(new string(this.issuedComboBox.SelectedItem.ToString().TakeWhile(char.IsDigit).ToArray()));
                BookStatManager currentIssuedBook = statRepository.GetBookStatManager(article);
                DateTime startDate = currentIssuedBook.StartDate;
                DateTime endDate = currentIssuedBook.EndDate;
                this.cStartDatelabel.Text = startDate.ToString("dd.MM.yyyy");
                this.cEndDatelabel.Text = endDate.ToString("dd.MM.yyyy");
                
                this.cStartDatelabel.ForeColor = Color.RoyalBlue;
                if (currentIssuedBook.EndDate > DateTime.Now.Date)
                {
                    this.cEndDatelabel.ForeColor = Color.Green;
                    this.userEmailLabel.Visible = false;
                    this.currEmailLabel.Visible = false;
                }
                else
                {
                    this.cEndDatelabel.ForeColor = Color.Red;
                    this.userEmailLabel.Visible = true;
                    this.currEmailLabel.Visible = true;
                    this.currEmailLabel.Text = userRepository.getEmail(statRepository.getUserIDByArticle(article));
                }
            }
            else
            {
                this.cStartDatelabel.Text = "Start date is empty";
                this.cEndDatelabel.Text = "End date is empty";
                this.cStartDatelabel.ForeColor = Color.LightBlue;
                this.cEndDatelabel.ForeColor = Color.LightBlue;
                this.userEmailLabel.Visible = false;
                this.currEmailLabel.Visible = false;
            }
        }

        private void returnMenuButton_Click(object sender, EventArgs e)
        {
            if (this.issuedComboBox.SelectedIndex != 0)
            {
                int article = int.Parse(new string(this.issuedComboBox.SelectedItem.ToString().TakeWhile(char.IsDigit).ToArray()));
                BookStatManager currentIssuedBook = statRepository.GetBookStatManager(article);
                string Login = statRepository.getUserIDByArticle(article);
                DateTime startDate = currentIssuedBook.StartDate;
                DateTime endDate = currentIssuedBook.EndDate;
                if (borrowRepository.addHistory(Login, article, currentIssuedBook.StartDate, DateTime.Now.Date))
                {
                    if (statRepository.removeBookWithItsStatus(article))
                    {
                        IsChanged = true;
                        this.issuedComboBox.Items.RemoveAt(this.issuedComboBox.SelectedIndex);
                        this.issuedComboBox.SelectedIndex = 0;
                        MessageBox.Show("Book successfully returned to the library!", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Choose book!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReturnBookForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsChanged)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
