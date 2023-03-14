using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AISoftWare_SW
{
    public struct NeedCookie
    {
        public string szCfbm { get; set; }
        public string szSecureNextAuth { get; set; }
        public string szCfClearance { get; set; }
    }
    public class SeleniumHelper
    {
        public static string m_szWebUrl = "https://chat.openai.com/chat";
        static ChromeDriverService _driverService = null;
        static ChromeOptions _options = null;
        static ChromeDriver _driver = null;
        static string m_szSearchText = null;
        static bool m_bFinished = false;
        static string m_szAnswerText = null;
        static int m_iDivState = 0;
        static bool m_bReady = false;
        static NeedCookie m_NCData;

        public static void OnSetCookie(in NeedCookie needCookie)
        {
            m_NCData = new NeedCookie();
            m_NCData = needCookie;
        }
        public static void Initialize()
        {
            _driverService = ChromeDriverService.CreateDefaultService();
            _driverService.HideCommandPromptWindow = true;
            _options = new ChromeOptions();
            _options.AddArgument("disable-gpu");
            //_options.AddArgument("headless"); //가리기
        }

        public static void OnMainSetting()
        {
            Initialize();

            _driver = new ChromeDriver(_driverService, _options);
            _driver.Manage().Window.Minimize();
            _driver.Navigate().GoToUrl(m_szWebUrl);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            Cookie cookie1 = new Cookie("__cf_bm", m_NCData.szCfbm);
            
            Cookie cookie2 = new Cookie("__Secure-next-auth.callback-url", "https%3A%2F%2Fchat.openai.com%2Fchat"); //바꿀필요X
            
            Cookie cookie3 = new Cookie("__Secure-next-auth.session-token", m_NCData.szSecureNextAuth);
            //세션키
            Cookie cookie4 = new Cookie("cf_clearance", m_NCData.szCfClearance); //2개중 아래가 이부분이 캡챠임

            Cookie cookie5 = new Cookie("__Host-next-auth.csrf-token", "6c9178f8f2d803b5a2759e425fd5f2177a8fa3060c199a798c3c2d8b9b3416b6%7Cd3bda46acfc4fe693ba3b89abbd0f4901931b333934e8ca2e0b5ed3a822b27f2"); //2개중 아래가 이부분이 캡챠임

            _driver.Manage().Cookies.AddCookie(cookie1);
            _driver.Manage().Cookies.AddCookie(cookie2);
            _driver.Manage().Cookies.AddCookie(cookie3);
            _driver.Manage().Cookies.AddCookie(cookie4);
            _driver.Manage().Cookies.AddCookie(cookie5);
            _driver.Navigate().GoToUrl(m_szWebUrl); //캡챠는 쿠키넣고 리프래쉬로 넘김
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            var HeadLine1 = _driver.FindElement(By.XPath("//*[@id=\'headlessui-dialog-panel-:r1:\']/div[2]/div[4]/button"));
            HeadLine1.Click();
            for (int i = 0; i < 2; i++)
            {
                var HeadLine2 = _driver.FindElement(By.XPath("//*[@id='headlessui-dialog-panel-:r1:']/div[2]/div[4]/button[2]"));
                HeadLine2.Click();
            }

            m_bReady = true;
        }

        public static void OnMoreSearch()
        {
            m_iDivState = m_iDivState + 2; //2칸 뜀
            m_bFinished = false;

            var szMessageText = _driver.FindElement(By.TagName("textarea"));
            szMessageText.SendKeys(m_szSearchText);
            szMessageText.SendKeys(Keys.Enter);

            while (true)
            {
                try
                {
                    var szTextGet = _driver.FindElement(By.XPath($"//*[@id='__next']/div[1]/div/main/div[1]/div/div/div/div[{m_iDivState}]/div"));
                    m_szAnswerText = szTextGet.Text;

                    var szCheckText = _driver.FindElement(By.XPath("//*[@id='__next']/div[1]/div[1]/main/div[2]/form/div/div[1]/button"));
                    if (szCheckText.Text == "Regenerate response")
                    {
                        break;
                    }
                }
                catch
                {

                }
                Thread.Sleep(100);
            }

            m_bFinished = true;
        }
        public static void OnFirstSearch()
        {
            m_iDivState = 2; //엔서는 2에서 함.
            m_bFinished = false;

            _driver.Navigate().GoToUrl(m_szWebUrl);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            while (true)
            {
                try
                {
                    var RefreshCheck = _driver.FindElement(By.XPath("//*[@id='__next']/div[1]/div[1]/main/div[1]/div/div/div/div[1]/h1"));
                    if (RefreshCheck.Text == "ChatGPT")
                    {
                        break;
                    }
                }
                catch
                {

                }
                Thread.Sleep(100);
            }

            var szMessageText = _driver.FindElement(By.TagName("textarea"));
            szMessageText.SendKeys(m_szSearchText);
            szMessageText.SendKeys(Keys.Enter);

            while (true)
            {
                try
                {
                    var szTextGet = _driver.FindElement(By.XPath("//*[@id='__next']/div[1]/div/main/div[1]/div/div/div/div[2]/div"));
                    m_szAnswerText = szTextGet.Text;

                    var szCheckText = _driver.FindElement(By.XPath("//*[@id='__next']/div[1]/div[1]/main/div[2]/form/div/div[1]/button"));
                    if (szCheckText.Text == "Regenerate response")
                    {
                        break;
                    }
                }
                catch
                {

                }
                Thread.Sleep(100);
            }

            m_bFinished = true;
        }

        public static bool IsReady()
        {
            return m_bReady;
        }
        public static void OnQuitSelenium()
        {
            _driver.Quit();
        }
        public static void SetSearchText(string szText)
        {
            m_szSearchText = szText;
        }

        public static bool IsFinish()
        {
            return m_bFinished;
        }

        public static void OnFinishReset()
        {
            m_bFinished = false;
        }

        public static string GetAnswer()
        {
            return m_szAnswerText;
        }
    }
}
