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
             
        INode node;
        public INode Node
        {
            get
            {
                if (this.node == null)
                {
                    this.node = new Book(); //root 
                }                      
                return this.node;
            }
            set
            {
                this.node = value;
                RaisePropertyChanged("Node");
            }
        }                    

        ReadOnlyDispatcherCollection<NodeViewModel> children;
        public ReadOnlyDispatcherCollection<NodeViewModel> Children
        {
            get
            {                   
                if (this.children == null)
                {
                    this.children = ViewModelHelper.CreateReadOnlyDispatcherCollection(Node.Children
                        , m => new NodeViewModel(m, this)
                        , DispatcherHelper.UIDispatcher
                        );
                    CompositeDisposable.Add(this.children);
                }
                return children;
            }        
        }

        public INode SelectedItem { get; set; }

        public void FolderSelected(FolderSelectionMessage m)
        {
            if (!this.node.IsDir) return;
            if (m.Response == null) return;
            this.node.Add(new Book(m.Response), true);  // これをVMでやるのおかしくない?
        }

        public void GoUp()
        {
            if (this.Node.Parent == null) return;
            this.Node = this.Node.Parent;
        }

        public void GoNext()
        {
            if (this.SelectedItem == null) return;
            if (this.SelectedItem.Next == null) return;
            this.SelectedItem = this.SelectedItem.Next;
            RaisePropertyChanged("SelectedItem");
        }

        public void GoPrev()
        {
            if (this.SelectedItem == null) return;
            if (this.SelectedItem.Prev == null) return;                 
            this.SelectedItem = this.SelectedItem.Prev;
            RaisePropertyChanged("SelectedItem");
        }

        public void Selected()
        {
            if (this.SelectedItem == null) return;
            if (!this.SelectedItem.IsDir) return;
            this.Node = this.SelectedItem;     
        }
    }
      
    public class NodeViewModel : Livet.ViewModel, INode
    {
        INode node;
        MainWindowViewModel parent;

        public NodeViewModel(INode node, MainWindowViewModel parent)
        {
            this.node = node;
            this.parent = parent;
        }

        public string Name
        {
            get { return this.node.Name; }
            set { this.node.Name = value; }
        }

        public INode Parent
        {
            get { return this.node.Parent; }
            set { this.node.Parent = value; }
        }

        public ObservableSynchronizedCollection<INode> Children
        {
            get { return this.node.Children; }
            set { this.node.Children = value; }
        }

        public void Remove(INode node)
        {
            this.node.Remove(node);
        }

        public void Add(INode node, bool isDir)
        {
            this.node.Add(node, isDir);
        }

        public string Path
        {
            get { return this.node.Path; }
            set { this.node.Path = value; }
        }

        public INode Next
        {
            get
            {
                this.node.Close();
                this.node.Next.Open();
                return this.node.Next;
            }      
        }

        public INode Prev
        {
            get
            {
                this.node.Close();
                this.node.Prev.Open();
                return this.node.Prev;
            }    
        }      

        public INode FindRoot()
        {
            return this.node.FindRoot();
        }   

        public bool IsDir { get; set; }

        public DateTime LastAccessed { get; }
        public List<TimeSpan> StayTimes { get; }
        public TimeSpan CurrentStayTime { get; }
        public TimeSpan EstimatedStayTime { get; }

        #region 実装しなきゃいけないから実装してるけど呼ばれることは想定してない
        public void Open()
        {
            this.node.Open();
        }

        public void Close()
        {
            this.node.Close();
        }
        #endregion
    }


}
