using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultipurposeImageViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage m_bitmap = null;

        public MainWindow()
        {
            InitializeComponent();
            ContentRendered += (s, e) => { openDefaultFile(); };    //起動時にファイルを読み込む
            
        }

        // メニュー - 開く
        //とりあえずデグレード
        /*
        private void miOpen_Click(object sender, RoutedEventArgs e)
        {
            // ファイルを開くダイアログ
            Microsoft.Win32.OpenFileDialog dlg =
                new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG|*.jpg|BMP|*.bmp";
            
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                // 既に読み込まれていたら解放する
                if (m_bitmap != null)
                {
                    m_bitmap = null;
                }
                // BitmapImageにファイルから画像を読み込む
                m_bitmap = new BitmapImage();
                m_bitmap.BeginInit();
                m_bitmap.UriSource = new Uri(filename);
                m_bitmap.EndInit();
                // Imageコントロールに表示
                image1.Source = m_bitmap;
            }
        }
        */
        private void miOpen_Click(object sender, RoutedEventArgs e)
        {
            //例外処理を追加する必要あり(2016/07/22杉山)
            //対象のファイルが存在しない場合アプリケーションが落ちるため

            string filename = "D:/images2.jpg";

            // 既に読み込まれていたら解放する
            if (m_bitmap != null)
            {
                m_bitmap = null;
            }
            // BitmapImageにファイルから画像を読み込む
            m_bitmap = new BitmapImage();
            m_bitmap.BeginInit();
            m_bitmap.UriSource = new Uri(filename);
            m_bitmap.EndInit();
            // Imageコントロールに表示
            image1.Source = m_bitmap;
        }

        public void openDefaultFile()
        {
            //例外処理を追加する必要あり(2016/07/22杉山)
            //対象のファイルが存在しない場合アプリケーションが落ちるため

            string filename = "D:/images.jpg";

            // 既に読み込まれていたら解放する
            if (m_bitmap != null)
            {
                m_bitmap = null;
            }
            // BitmapImageにファイルから画像を読み込む
            m_bitmap = new BitmapImage();
            m_bitmap.BeginInit();
            m_bitmap.UriSource = new Uri(filename);
            m_bitmap.EndInit();
            // Imageコントロールに表示
            image1.Source = m_bitmap;
        }

        //以下ノータッチ(2016/07/22 杉山)

        // メニュー - 終了
        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // メニュー - 画面に合わせる
        private void miFit_Click(object sender, RoutedEventArgs e)
        {
            if (m_bitmap != null)   // 画像が読み込まれている場合
            {
                // スクロールバーを非表示
                scrollViewer1.VerticalScrollBarVisibility =
                    ScrollBarVisibility.Disabled;
                scrollViewer1.HorizontalScrollBarVisibility =
                    ScrollBarVisibility.Disabled;
                // Imageコントロールのサイズを
                // ScrollViewerのサイズに合わせる
                image1.Width = scrollViewer1.Width;
                image1.Height = scrollViewer1.Height;
            }
        }

        // メニュー - 等倍表示
        private void mi100_Click(object sender, RoutedEventArgs e)
        {
            if (m_bitmap != null)   // 画像が読み込まれている場合
            {
                // ScrollViewerのサイズよりImageのサイズ
                // の方が大きい場合はスクロールバー表示
                scrollViewer1.VerticalScrollBarVisibility =
                    ScrollBarVisibility.Auto;
                scrollViewer1.HorizontalScrollBarVisibility =
                    ScrollBarVisibility.Auto;
                // Imageのサイズを読み込んだ画像のサイズに合わせる
                image1.Width = m_bitmap.PixelWidth;
                image1.Height = m_bitmap.PixelHeight;
            }
        }

    }
}
