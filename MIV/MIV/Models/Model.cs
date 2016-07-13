using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace MIV.Models
{                   
    public class Book : NotificationObject
    {
        BookShelf m_bookShelf;

        String m_name;
        public String Name
        {
            get { return m_name; }
            set
            {
                if (m_name == value) return;
                m_name = value;
                RaisePropertyChanged("Name");
            }
        }

        String m_path;
        public String Path
        {
            get { return m_path; }
            set
            {
                if (m_path == value) return;
                m_path = value;
                RaisePropertyChanged("Path");
            }
        }

        String m_currentPage;
        public String CurrentPage
        {
            get { return m_currentPage; }
            set
            {
                if (m_currentPage == value) return;
                m_currentPage = value;
                RaisePropertyChanged("CurrentPage");
            }
        }

        public Book(BookShelf bookShelf)
        {
            m_bookShelf = bookShelf;
        }

        public void NextPage()
        {
            string str = this.CurrentPage;
            this.CurrentPage = str.Replace("0001", "0002");
            Console.Write(str.Replace("0001", "0002")); 
        }

        public bool IsIncludedInMainCollection()
        {
            return m_bookShelf.Books.Contains(this);
        }

        public void AddThisToMainCollection()
        {
            m_bookShelf.Books.Add(this);
        }

        public void RemoveThisFromMainCollection()
        {
            m_bookShelf.Books.Remove(this);
        }
    }

    public class BookShelf : Livet.NotificationObject
    {
        ObservableSynchronizedCollection<Book> m_books;
        public ObservableSynchronizedCollection<Book> Books
        {
            get
            {
                if(m_books == null)
                {
                    m_books = new ObservableSynchronizedCollection<Book>
                    {
                        new Book(this) {Name = "hoge", Path="./", CurrentPage=System.Environment.CurrentDirectory+@"\media\0001.jpg" }
                    };
                }
                return m_books;
            }
        }
    }
}
