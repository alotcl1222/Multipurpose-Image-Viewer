using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;   

using Livet;

namespace MIV.Models
{
    public interface INode
    {
        // 本(=本棚)、ページのインターフェイス
        string Name { get; set; }
        INode Parent { get; set; }
        ObservableSynchronizedCollection<INode> Children { get; set; }
        void Remove(INode node);
        void Add(INode node, bool isDir);
        string Path { get; set; }
        INode Next { get; set; }
        INode Prev { get; set; }   
        INode FindRoot();
        bool IsDir { get; set; }
    }

    public abstract class AbstractNode : NotificationObject, INode
    {
        string m_name;
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

        INode m_parent;
        public INode Parent
        {
            get { return m_parent; }
            set
            {
                if (m_parent == value) return;
                m_parent = value;
                RaisePropertyChanged("Parent");
            }
        }

        public virtual ObservableSynchronizedCollection<INode> Children
        {
            get { return null; }
            set { throw new InvalidOperationException(); }
        }

        public virtual void Remove(INode node)
        {
            throw new InvalidOperationException();
        }

        public virtual void Add(INode node, bool isDir)
        {
            throw new InvalidOperationException();
        }

        string m_path;
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

        INode m_next;
        public INode Next { get; set; }

        INode m_prev;
        public INode Prev { get; set; }      
                                           
        public virtual INode FindRoot()
        {
            if (this.m_parent == null) return this;
            return this.m_parent.FindRoot();
        }

        public bool IsDir { get; set; }
    }

    public class Page : AbstractNode
    {
        public Page(INode node)
        {
            this.Parent = node; // 自身を格納するBook
            this.IsDir = false;
        }
    }

    public class Book : AbstractNode
    {
        public string FolderPath { get; set; }

        public Book(string path="")
        {
            this.Name = "root";
            this.IsDir = true;
            this.FolderPath = path;
        }

        ObservableSynchronizedCollection<INode> m_children;
        public override ObservableSynchronizedCollection<INode> Children
        {
            get { return m_children; } 
            set { m_children = value; }
        }

        public override void Remove(INode node)
        {
            m_children.Remove(node);
            RaisePropertyChanged("Children");
        }

        public override void Add(INode node, bool isDir)
        {
            if (m_children == null)
            {
                m_children = new ObservableSynchronizedCollection<INode> { };
            }       
            node.Parent = this; 
            m_children.Add(node);

            if (isDir)
            {
                this.AddPages((Book)node);
                node.Path = System.Environment.CurrentDirectory + @"\media\folder.jpg";
            }

            RaisePropertyChanged("Children");
            RaisePropertyChanged("Path");
        } 
        
        private void AddPages(Book parent)
        {
            var files = System.IO.Directory.GetFiles(parent.FolderPath, "*.jpg", System.IO.SearchOption.TopDirectoryOnly);
            var pages = new List<INode> { };
            foreach (var file in files)
            {
                INode child = new Page(parent);
                child.Path = file;
                if (pages.Any())
                {
                    child.Prev = pages.Last();
                    pages.Last().Next = child; 
                }
                pages.Add(child);
            }    

            foreach(var page in pages)
            {                            
                parent.Add(page, false);
            }                                        
        }                                               
    }
                                
}
