using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace WpfOpenWebpage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<WebPage> oldPages = new List<WebPage>();
        List<WebPage> newPages = new List<WebPage>();
        public MainWindow()
        {
            InitializeComponent();
            //初始化托盘程序
            IniNotifyIcon();
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.WindowState = WindowState.Minimized;
            Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                //this.Width = 400;
                //this.Height = 250;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var value = MessageBox.Show("确认退出该程序!", "", MessageBoxButton.YesNo);
            if (value == MessageBoxResult.Yes)
            {
                //终止所有线程 
                Environment.Exit(Environment.ExitCode);
            }
        }

        private string FromTBGetPages()
        {
            newPages.Clear();
            string error = string.Empty;
            string[] webCont = webContentes.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            webContentes.Text = string.Empty;

            if (webCont.Length >= 0)
            {
                int index = 0;
                int fin = 0;

                foreach (var str in webCont)
                {
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        index++;
                        continue;
                    }
                    WebPage page = new WebPage();
                    string p = @"(http|https)://(?<domain>[^(:|/]*)";
                    Regex reg = new Regex(p, RegexOptions.IgnoreCase);
                    Match m = reg.Match(str);
                    string Result = m.Groups["domain"].Value;
                    if (!string.IsNullOrWhiteSpace(Result))
                    {
                        page.Id = fin;
                        page.Page = Result;
                        webContentes.Text += "https://" + Result + "\r\n";
                        newPages.Add(page);
                        fin++;
                    }
                    else
                    {
                        webContentes.Text += str + "\r\n";
                        error += (index + 1).ToString();
                    }
                    index++;
                }
            }
            return error;
        }

        private void WriteSeting()
        {
            ConfigureAppConfig.AddAndUPAppSeting("pageNum", newPages.Count.ToString());
            foreach (var page in newPages)
            {
                if (string.IsNullOrEmpty(page.Page))
                {
                    continue;
                }
                ConfigureAppConfig.AddAndUPAppSeting("page" + page.Id.ToString(), page.Page);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(ConfigureAppConfig.GetAppSettingsKeyValue("PageNum")))
            {
                StringBuilder sb = new StringBuilder();
                int pageN = Convert.ToInt32(ConfigureAppConfig.GetAppSettingsKeyValue("PageNum"));
                for (int i = 0; i < pageN; i++)
                {
                    WebPage page = new WebPage();
                    page.Id = i;
                    page.Page = ConfigureAppConfig.GetAppSettingsKeyValue($"page{i.ToString()}");
                    if (!string.IsNullOrWhiteSpace(page.Page))
                    {
                        oldPages.Add(page);
                        sb.Append("https://" + page.Page + "\r\n");
                    }
                }
                webContentes.Text = sb.ToString();
                foreach (var page in oldPages)
                {
                    if (string.IsNullOrEmpty(page.Page))
                    {
                        continue;
                    }
                    System.Diagnostics.Process.Start(page.Page);
                }
                //this.WindowState = WindowState.Minimized;
                Hide();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //清除
            newPages.Clear();
            //获取文本内容
            string error = FromTBGetPages();
            if (!string.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show($"第{error}行网址输入错误请修改");
                return;
            }
            //写入配置
            WriteSeting();
            //开机启动+检测
            ConfigureAppConfig.AddAndUPAppSeting("isAuto", "1");
            CheckAdministrator();
            AutoRun();
            MessageBox.Show("设置成功!欢迎使用!");
        }

        /// <summary>
        /// 根据 app.config 中"isAuto"，设置是否开机启动
        /// </summary>
        public void AutoRun()
        {
            RegistryKey _rlocal = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;   //E:\Code\XXX.JobRunner\bin\Debug\KMHC.OCP.JobRunner.exe     XXX是路径和namespace
            var appName = appPath.Substring(appPath.LastIndexOf('\\') + 1);     //XXX.JobRunner.exe  XXX 是namespace
            try
            {
                var isAuto = ConfigurationManager.AppSettings["isAuto"];
                if (isAuto == "1")
                {
                    _rlocal.SetValue(appName, string.Format(@"""{0}""", appPath));
                }
                else
                {
                    _rlocal.DeleteValue(appName, false);

                }
                _rlocal.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("设置开机是否启动失败: {0}", ex.Message));
            }
        }

        /// <summary>
        /// 检查是否是管理员身份
        /// </summary>
        private void CheckAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            if (!runAsAdmin)
            {
                var val = MessageBox.Show("需要开启管理员权限才能对注册表进行操作,\n是否重启该程序?", "", MessageBoxButton.YesNo);
                if (val == MessageBoxResult.Yes)
                {
                    // It is not possible to launch a ClickOnce app as administrator directly,
                    // so instead we launch the app as administrator in a new process.
                    var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                    // The following properties run the new process as administrator
                    processInfo.UseShellExecute = true;
                    processInfo.Verb = "runas";

                    // Start the new process
                    try
                    {
                        Process.Start(processInfo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    // Shut down the current process
                    Environment.Exit(0);
                }
                else
                {
                    MessageBox.Show("Sorry,无法进行该操作!,\n请以管理员方式运行!");
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ConfigureAppConfig.DellAll();
            ConfigureAppConfig.AddAndUPAppSeting("isAuto", "2");
            CheckAdministrator();
            AutoRun();
            MessageBox.Show("初始化程序,完成!\n请重新配置!");
        }


        private System.Windows.Forms.NotifyIcon notify;

        private void IniNotifyIcon()
        {
            this.notify = new System.Windows.Forms.NotifyIcon();
            this.notify.BalloonTipText = "运行中.....";
            this.notify.ShowBalloonTip(2000);
            this.notify.Text = "运行中.....";
            //this.notify.Icon = new System.Drawing.Icon(@"icon.ico");
            this.notify.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this.notify.Visible = true;
            //打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("Open");
            open.Click += new EventHandler(Show);
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += new EventHandler(Close);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            notify.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            this.notify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left) this.Show(o, e);
            });
        }
        private void Show(object sender, EventArgs e)
        {
            window.Visibility = System.Windows.Visibility.Visible;
            //window.ShowInTaskbar = true;
            window.Activate();
        }

        private void Hide(object sender, EventArgs e)
        {
            window.ShowInTaskbar = false;
            window.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Close(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }

}
