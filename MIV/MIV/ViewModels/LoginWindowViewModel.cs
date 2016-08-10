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
    public class LoginWindowViewModel : ViewModel
    {
        Livet.EventListeners.WeakEvents.PropertyChangedWeakEventListener weak;

        private UserModel user = new UserModel();
                    
        public string IdErrComment { get; private set; }            
        public string InputID
        {
            get { return user.Id; }
            set
            {
                if (user.Id == value) return;
                user.Id = value;
                this.IdErrComment = user.IDErrComment();
                this.ErrComment = string.Empty;
                RaisePropertyChanged("ID");  
            }
        }
                                                       
        public string PswErrComment { get; private set; }            
        public string InputPsw
        {
            get { return user.Psw; }
            set
            {
                if (user.Psw == value) return;
                user.Psw = value;
                this.PswErrComment = user.PswErrComment();
                this.ErrComment = string.Empty;
                RaisePropertyChanged("Psw");
            }
        }
        private string errComment;
        public string ErrComment
        {
            get { return this.errComment ?? string.Empty; }
            set
            {
                if (this.errComment == value) return;
                this.errComment = value;                
                RaisePropertyChanged("ErrComment");
            }
        }

        public void Initialize()
        {
        }

        public LoginWindowViewModel()
        {    
            InitializeInput();

            weak = new Livet.EventListeners.WeakEvents.PropertyChangedWeakEventListener(user,
                (o, e) =>
                {
                    RaisePropertyChanged(e.PropertyName);
                    if (e.PropertyName == "CurrentUserID")
                    {
                        RaisePropertyChanged("CurrentUserID");
                    }
                });
            CompositeDisposable.Add(weak);
        }

        void InitializeInput()
        {
            InputID = user.Id;
            InputPsw = user.Psw;

            IdErrComment = @"なんか入力しろ";
            PswErrComment = @"なんか入力しろ";
        }


        Livet.Commands.ViewModelCommand doLoginCommand;
        public ICommand DoLoginCommand
        {
            get
            {
                if (doLoginCommand == null)
                {
                    doLoginCommand = new Livet.Commands.ViewModelCommand(() =>
                    {
                        if (!user.Login())
                        {
                            this.ErrComment = @"は？";
                        }
                        else
                        {
                            Messenger.Raise(new WindowActionMessage(Livet.Messaging.Windows.WindowAction.Close, "Close"));
                        }
                    });
                }                                         
                return doLoginCommand;      
            }
        }

    }
}
