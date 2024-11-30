﻿using libraryMeneger.book;
using libraryMeneger.Data.BookRepository;
using libraryMeneger.Data.StatusRepository;
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
    public partial class ReservePlus : Form
    {
        public RegularUser regularUser { get; private set; }
        BooksRepository repository = new BooksRepository();
        StatusRepository statusRepository = new StatusRepository();

        public ReservePlus(RegularUser user)
        {
            InitializeComponent();
            regularUser = user;
            this.DialogResult = DialogResult.Cancel;

            CurrentDateTimePicker.Value = DateTime.Today;
            CurrentDateTimePicker.CustomFormat = "dd.MM.yyyy";
            CurrentDateTimePicker.Format = DateTimePickerFormat.Custom;

            EndDateTimePicker.Value = CurrentDateTimePicker.Value.AddDays(3);
            EndDateTimePicker.CustomFormat = "dd.MM.yyyy";
            EndDateTimePicker.Format = DateTimePickerFormat.Custom;

            List<GenBook> allBooks = repository.getAvailableBooksForBooking();

            if (allBooks.Count > 0)
            {
                BookComboBox.Items.Add("Select the book:");
                foreach (GenBook book in allBooks) 
                {
                    BookComboBox.Items.Add(book.BookToString());
                }
            }
            else
            {
                BookComboBox.Items.Add("No books available:(");
            }
            BookComboBox.SelectedIndex = 0;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (BookComboBox.SelectedIndex != 0)
            {
                int article = int.Parse(new string(this.BookComboBox.SelectedItem.ToString().TakeWhile(char.IsDigit).ToArray()));
                DateTime current = CurrentDateTimePicker.Value;
                DateTime end = EndDateTimePicker.Value;
                BookStatManager manager = new BookStatManager(article, current, end, true, false);

                if (statusRepository.addBookWithItsStatus(regularUser.Login, manager))
                {
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("Book successfully reserved!", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else 
            {
                MessageBox.Show("The book is not selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
