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
        ObservableSynchronizedCollection<INode> Children { get; }
        void Remove(INode node);
        void Add(INode node);
        string Path { get; set; }
        INode Next { get; set; }
        INode Prev { get; set; }
        bool HasNext { get; }
        bool HasPrev { get; }
        INode FindRoot();
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
            get { throw new InvalidOperationException(); }
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

        public virtual INode FindRoot()
        {
            if (this.m_parent == null) return this;
            return this.m_parent.FindRoot();
        }
    }

    public class Page : AbstractNode
    {
        public Page(INode node)
        {
            this.Parent = node; // 自信を格納するBook
        }
    }

    public class Book : AbstractNode
    {
        public Book()
        {
            this.Name = "root";
        }

        ObservableSynchronizedCollection<INode> m_children;
        public override ObservableSynchronizedCollection<INode> Children
        {
            get { return m_children; }
        }

        public override void Remove(INode node)
        {
            m_children.Remove(node);
        }

        public override void Add(INode node)
        {
            m_children.Add(node);
        }
    }
                                
}
