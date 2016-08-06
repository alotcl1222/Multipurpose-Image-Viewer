using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace MIV.Models
{
    public class UserModel : NotificationObject
    {
        /*
         * NotificationObjectはプロパティ変更通知の仕組みを実装したオブジェクトです。
         */                                
        private string id;
        public string Id { get; set; }
        private string psw;
        public string Psw { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="id">
        /// IDの文字列
        /// </param>
        /// <param name="psw">
        /// パスワードの文字列
        /// </param>
        public UserModel(string id, string psw)
        {
            this.id = id;
            this.psw = psw;
        }

        /// <summary>
        /// コンストラクタその2。id/pswは空文字列が入る
        /// </summary>    
        public UserModel()
        {
            this.id = string.Empty;
            this.psw = string.Empty;
        }

        /// <summary>                            
        /// ユーザIDが存在するかどうかを返す
        /// </summary> 
        /// <returns>
        /// ユーザIDが存在するか
        /// </returns>
        public bool Exists()
        {
            return true;
        }

        /// <summary>                            
        /// ユーザIDの書式が正しいかどうかを返す
        /// </summary>                 
        /// <returns>
        /// ユーザIDの書式が正しいか
        /// </returns>
        public bool IsValidID()
        {
            return true;
        }

        /// <summary>                            
        /// パスワードの書式が正しいかどうかを返す
        /// </summary>                 
        /// <returns>
        /// パスワードの書式が正しいか
        /// </returns>
        public bool IsValidPsw()
        {
            return true;
        }

        /// <summary>                            
        /// ID&pswの組み合わせが正しいかどうかを返す
        /// </summary>                 
        /// <returns>
        /// ID&pswの組み合わせが書式が正しいか
        /// </returns>
        public bool IsValidPair()
        {
            return true;
        }
                
        /// <summary>                            
        /// 現在のユーザ情報でログインする。
        /// </summary>  
        /// <returns>
        /// ログインに成功したかどうか
        /// </returns>
        public bool Login()
        {                      
            // 正しいID&pswの組み合わせでないのにログインを試行した場合false
            if (!this.IsValidPair()) return false;

            // 設定ファイルにログインするユーザー名を書いておく
            Properties.Settings.Default.currentUserID = this.id;
            Properties.Settings.Default.Save();
            return true;
        }

        /// <summary>
        /// IDについての正当性チェックした結果を返す
        /// </summary>
        /// <returns>
        /// チェック結果のエラーコメントを返す。
        /// </returns>
        public string IDErrComment()
        {
            if (!this.IsValidID()) return @"半角英数6文字で入力しろ。";
            if (!this.Exists()) return @"そんな奴はいない";
            if (!this.IsValidPair()) return @"ちがう";
            return @"OKな気がする";
        }

        /// <summary>
        /// Pswについての正当性チェックした結果を返す
        /// </summary>
        /// <returns>
        /// チェック結果のエラーコメントを返す。
        /// </returns>
        public string PswErrComment()
        {
            if (!this.IsValidPsw()) return @"半角数字4文字で入力しろ。";
            if (!this.IsValidPair()) return @"ちがう";
            return @"OKな気がする";
        }
    }
}
