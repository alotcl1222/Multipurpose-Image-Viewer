using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MIV.Models;
using System.Linq;

namespace ModelTestStayTime
{
    [TestClass]
    public class ModelTestStayTime
    {
        [TestMethod]
        public void 最後にアクセスした日時をUTCで取得できるか()
        {
            #region テスト準備
            var root = new Book();
            var book = new Book();    
            book.FolderPath = System.Environment.CurrentDirectory + @"\media\";            
            root.Add(book, true);
            #endregion
            // 1ページ目を開く
            book.Children[0].Open();    
            // UTCでアクセス時間を取得できるか(+例外が発生しないか)テスト。
            Assert.AreEqual(book.Children[0].LastAccessed.Kind, DateTime.UtcNow.Kind);
        }

        [TestMethod]
        public void アクセスのたびにアクセス日時が更新されているか()
        {
            #region テスト準備
            var root = new Book();
            var book = new Book();                                             
            book.FolderPath = System.Environment.CurrentDirectory + @"\media\";
            root.Add(book, true);
            #endregion
            // 以降の1ページめへのアクセスがめんどいのでtargetという名前で呼ぶ
            var target = book.Children[0]; 
            //ページを開いて、アクセス時刻を取得
            target.Open();
            var time1 = target.LastAccessed;
            //ちょっと待つ
            System.Threading.Thread.Sleep(10);
            //閉じてもう一回開く
            target.Close();
            target.Open();
            // 0秒間を表す変数
            var zero = new TimeSpan(0);

            // アクセスのたびにアクセス日時が更新されているか(0秒以上経過しているか)テスト。
            Assert.IsTrue(target.LastAccessed - time1 > zero);
        }
    }
}
