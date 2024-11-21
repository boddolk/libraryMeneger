﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace libraryMeneger.book
{
    public class GenBook
    {
        private int article;
        private string title;
        private string author;
        private int year;

        public GenBook(int article, string title, string author, int year) 
        {
            this.article = article;
            this.title = title;
            this.author = author;
            this.year = year;
        }

        public int Article
        {
            get { return article; }
            set { article = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }
    }
}