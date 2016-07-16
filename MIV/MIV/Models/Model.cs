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
        void Add(INode node);
        string Path { get; set; }
        INode Next { get; set; }
        INode Prev { get; set; }
        bool HasNext { get; }
        bool HasPrev { get; }
        void GoNext();
        void GoPrev();
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

        public virtual void Add(INode node)
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

        public virtual bool HasNext
        {
            get { return m_next != default(INode); }
        }

        public virtual bool HasPrev
        {
            get { return m_prev != default(INode); }
        }

        public virtual void GoNext()
        {
            if (!this.HasNext) throw new InvalidOperationException();
            this.Name = this.Next.Name;
            this.Next = this.Next.Next;
            this.Path = this.Next.Path;
            this.Prev = this.Next.Prev;
        }

        public virtual void GoPrev()
        {
            if (!this.HasPrev) throw new InvalidOperationException();
            this.Name = this.Prev.Name;
            this.Next = this.Prev.Next;
            this.Path = this.Prev.Path;
            this.Prev = this.Prev.Prev;
        }

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
        public Book(string path="")
        {
            this.Name = "root";
            this.IsDir = true;
            this.Path = path;
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
        }

        public override void Add(INode node)
        {
            if (m_children == null)
            {
                m_children = new ObservableSynchronizedCollection<INode> { };
            }
            node.Parent = this;
            m_children.Add(node);
        }  

        public override void GoNext()
        {
            base.GoNext();
            this.Children = this.Next.Children;
        }

        public override void GoPrev()
        {
            base.GoPrev();
            this.Children = this.Prev.Children;
        }
                 
    }
                                
}
