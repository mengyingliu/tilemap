using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TileMapManager
{
    /// <summary>
    /// 静态的界面加载， 通过多线程防止欢迎界面假死状态
    /// </summary>
    public class Splash
    {
        static LoadForm mySplashForm = null;
        static Thread MySplashThread = null;

        static void ShowThread()
        {
            mySplashForm = new LoadForm();
            Application.Run(mySplashForm);
        }

        static public void Show()
        {
            if (MySplashThread != null)
                return;
            MySplashThread = new Thread(new ThreadStart(Splash.ShowThread));
            MySplashThread.IsBackground = true;
            MySplashThread.SetApartmentState(ApartmentState.STA);
            MySplashThread.Start();
        }
        static public void Close()
        {
            if (MySplashThread == null) return;
            if (mySplashForm == null) return;
            try
            {
                mySplashForm.Invoke(new MethodInvoker(mySplashForm.Close));
            }
            catch (Exception)
            {
            }
            MySplashThread = null;
            mySplashForm = null;
        }
        /// <summary>
        /// 设置加载状态
        /// </summary>
        /// <param name="state">加载状态</param>
        /// <param name="progress">加载进度</param>
        static public void setLoadState(String state,int progress)
        {
            if (mySplashForm == null)
                return;
            mySplashForm.StatusInfo = state;
            mySplashForm.LoadProgress = progress;
            mySplashForm.ChangeLoadStatus();
        }
    }
}
