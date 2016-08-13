using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

using Livet;
using MIV.Utils;
using System.Text.RegularExpressions;

namespace MIV.Models
{
    public class UserModel : NotificationObject
    {             
        private readonly Regex IdFormat = new Regex(Properties.Settings.Default.IdFormat);
        private readonly Regex PswFormat = new Regex(Properties.Settings.Default.PswFormat);

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
            // 使い終わったらDB閉じるため。
            // なくてもDisposeされる気がするけど念のため。
            // いちいちDB開いて問い合わせるのすごい無駄だけどまあぁいいや。
            using (var uRep = new UserRepository(Properties.Settings.Default.userDBPath))
            {
                var user = uRep.Select<UserEntity>(this.Id);
                if (user == null) return false;
                return true;
            }
        }

        /// <summary>                            
        /// ユーザIDの書式が正しいかどうかを返す
        /// </summary>                 
        /// <returns>
        /// ユーザIDの書式が正しいか
        /// </returns>
        public bool IsValidID()
        {
            return this.IdFormat.IsMatch(this.Id ?? string.Empty);
        }

        /// <summary>                            
        /// パスワードの書式が正しいかどうかを返す
        /// </summary>                 
        /// <returns>
        /// パスワードの書式が正しいか
        /// </returns>
        public bool IsValidPsw()
        {
            return this.PswFormat.IsMatch(this.Psw ?? string.Empty);
        }

        /// <summary>                            
        /// ID&pswの組み合わせが正しいかどうかを返す
        /// </summary>                 
        /// <returns>
        /// ID&pswの組み合わせが書式が正しいか
        /// </returns>
        public bool IsValidPair()
        {
            // DB問い合わせ前に書式の妥当性検証(Openするの重そうなので)
            if (!this.IsValidID()) return false; // 不適切なID
            if (!this.IsValidPsw()) return false; // 不適切なPsw
            // DB問い合わせ
            if (!this.Exists()) return false; // 存在しないユーザ  

            // DB開いてパスワード整合チェック
            using (var uRep = new UserRepository(Properties.Settings.Default.userDBPath))
            {
                var user = uRep.Select<UserEntity>(this.Id);
                // userがnullにならないことはチェック済み
                return user.Psw.Equals(this.Psw);
            }            
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
            Properties.Settings.Default.currentUserID = this.Id;
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
            if (!this.IsValidID()) return @"ID:半角英数6文字で入力しろ。";
            if (!this.Exists()) return @"ID:そんな奴はいない";
            if (!this.IsValidPair()) return @"ID:ちがう";
            return @"ID:OKな気がする";
        }

        /// <summary>
        /// Pswについての正当性チェックした結果を返す
        /// </summary>
        /// <returns>
        /// チェック結果のエラーコメントを返す。
        /// </returns>
        public string PswErrComment()
        {
            if (!this.IsValidPsw()) return @"パスワード:半角数字4文字で入力しろ。";
            if (!this.IsValidPair()) return @"パスワード:ちがう";
            return @"パスワード:OKな気がする";
        }
    }

    #region DB関連
    public class UserEntity
    {                                           
        public string Id { get; set; }
        public string Psw { get; set; }

        public UserEntity() { }
        public UserEntity(string id, string psw)
        {
            this.Id = id;
            this.Psw = psw;
        }
    }

    public class UserRepository : GenericRepository
    {
        public UserRepository(string dbPath) : base(dbPath)
        {
            this.DB.CreateTable<UserEntity>();
            if(Select<UserEntity>(@"mh0817") == null)
            {
                Insert(new UserEntity(@"mh0817", @"0817"));    
            }


        }


    }
    #endregion

}
