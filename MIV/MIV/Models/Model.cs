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
        INode Next { get; }
        INode Prev { get; }   
        INode FindRoot();
        bool IsDir { get; set; }
    }

    public abstract class AbstractNode : NotificationObject, INode
    {
        string name;
        public String Name
        {
            get { return this.name; }
            set
            {
                if (this.name == value) return;
                this.name = value;
                RaisePropertyChanged("Name");
            }
        }

        INode parent;
        public INode Parent
        {
            get { return this.parent; }
            set
            {
                if (this.parent == value) return;
                parent = value;
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

        string path;
        public String Path
        {
            get { return this.path; }
            set
            {
                if (this.path == value) return;
                this.path = value;
                RaisePropertyChanged("Path");
            }
        }

        INode next;
        public INode Next
        {
            get
            {
                var idx = this.parent.Children.IndexOf(this);
                if (idx == this.parent.Children.Count - 1) return this;
                return this.parent.Children[idx + 1 ];
            }

        }

        INode prev;
        public INode Prev
        {
            get
            {          
                var idx = this.parent.Children.IndexOf(this);
                if (idx == 0) return this;
                return this.parent.Children[idx - 1];
            }
        }      
                                           
        public virtual INode FindRoot()
        {
            if (this.parent == null) return this;
            return this.parent.FindRoot();
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

        ObservableSynchronizedCollection<INode> children;
        public override ObservableSynchronizedCollection<INode> Children
        {
            get { return this.children; } 
            set { this.children = value; }
        }

        public override void Remove(INode node)
        {
            this.children.Remove(node);
            RaisePropertyChanged("Children");
        }

        public override void Add(INode node, bool isDir)
        {
            if (this.children == null)
            {
                this.children = new ObservableSynchronizedCollection<INode> { };
            }       
            node.Parent = this; 
            this.children.Add(node);

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
                parent.Add(child, false);
            }                              
        }                                             
    }
                                
}
