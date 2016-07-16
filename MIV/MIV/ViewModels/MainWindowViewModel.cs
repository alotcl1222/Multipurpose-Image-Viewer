using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using System.Windows.Input;

using MIV.Models;

namespace MIV.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {      
        public void Initialize()
        {
        }
             
        INode m_node;
        public INode Node
        {
            get
            {
                if (m_node == null)
                {
                    m_node = new Book(); //root 
                }                      
                return m_node;
            }
            set
            {
                m_node = value;
                RaisePropertyChanged("Node");
            }
        }                    

        ReadOnlyDispatcherCollection<NodeViewModel> m_children;
        public ReadOnlyDispatcherCollection<NodeViewModel> Children
        {
            get
            {                   
                if (m_children == null)
                {
                    m_children = ViewModelHelper.CreateReadOnlyDispatcherCollection(Node.Children
                        , m => new NodeViewModel(m, this)
                        , DispatcherHelper.UIDispatcher
                        );
                    CompositeDisposable.Add(m_children);
                }
                return m_children;
            }        
        }

        public INode SelectedItem { get; set; }

        Livet.Commands.ViewModelCommand m_nextCommand;
        public ICommand NextCommand
        {
            get
            {
                if (m_nextCommand == null)
                {
                    m_nextCommand = new Livet.Commands.ViewModelCommand(() =>
                    {
                        if (this.Node.HasNext) this.Node = this.Node.Next;
                    });
                }
                return m_nextCommand;
            }
        }

        Livet.Commands.ViewModelCommand m_prevCommand;
        public ICommand PrevCommand
        {
            get
            {
                if (m_prevCommand == null)
                {
                    m_prevCommand = new Livet.Commands.ViewModelCommand(() =>
                    {
                        if (this.Node.HasPrev) this.Node = this.Node.Prev;
                    });
                }
                return m_prevCommand;
            }
        }

        Livet.Commands.ViewModelCommand m_upCommand;
        public ICommand UpCommand
        {
            get
            {
                if (m_upCommand == null)
                {
                    m_upCommand = new Livet.Commands.ViewModelCommand(() =>
                    {
                        if (this.Node.Parent == null) return;
                        this.Node = this.Node.Parent;
                    });
                }
                return m_upCommand;
            }
        }

        public void FolderSelected(FolderSelectionMessage m)
        {
            if (!m_node.IsDir) return;
            m_node.Add(new Book(m.Response), true);
        }  

        public void Selected()
        {
            if (this.SelectedItem == null) return;
            this.Node = this.SelectedItem;

        }
    }
      
    public class NodeViewModel : Livet.ViewModel, INode
    {
        INode m_node;
        MainWindowViewModel m_parent;

        public NodeViewModel(INode node, MainWindowViewModel parent)
        {
            m_node = node;
            m_parent = parent;
        }

        public string Name
        {
            get { return m_node.Name; }
            set { m_node.Name = value; }
        }

        public INode Parent
        {
            get { return m_node.Parent; }
            set { m_node.Parent = value; }
        }

        public ObservableSynchronizedCollection<INode> Children
        {
            get { return m_node.Children; }
            set { m_node.Children = value; }
        }

        public void Remove(INode node)
        {
            m_node.Remove(node);
        }

        public void Add(INode node, bool isDir)
        {
            m_node.Add(node, isDir);
        }

        public string Path
        {
            get { return m_node.Path; }
            set { m_node.Path = value; }
        }

        public INode Next
        {
            get { return m_node.Next; }
            set { m_node.Next = value; }
        }

        public INode Prev
        {
            get { return m_node.Prev; }
            set { m_node.Prev = value; }
        }

        public bool HasNext
        {
            get { return m_node.Next != default(INode); }
        }

        public bool HasPrev
        {
            get { return m_node.Prev != default(INode); }
        }                              

        public INode FindRoot()
        {
            return m_node.FindRoot();
        }   

        public bool IsDir { get; set; }   
                  
    }   

                
}
