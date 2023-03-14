using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISoftWare_SW
{
    using dicKeyList = Dictionary<String, String>;
    using dicTitleList = Dictionary<String, Dictionary<String, String>>;
    public class INILoader
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(String section, String key, String value, String filepath);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileString(String section, String key, String value, StringBuilder retValue, int size, String filepath);

        public String m_FileName;
        public String m_szTitle;

        dicTitleList m_TitleList = new dicTitleList();

        bool m_bSaveLog;
        StreamWriter m_Log;
        public INILoader()
        {
            m_FileName = "";
            m_szTitle = "";
        }

        ~INILoader()
        {
        }

        public INILoader(String szFile, bool save_log)
        {
            if (!File.Exists(szFile))
                return;

            m_bSaveLog = save_log;
            if (m_bSaveLog)
            {

                String log_file = Path.GetFileName(szFile);
                m_Log = new StreamWriter(String.Format("./{0}.log", log_file), false, Encoding.Default);
            }

            m_FileName = szFile;
            m_szTitle = "";
            Parsing();
        }

        public void SetFileName(String szFile)
        {
            m_TitleList.Clear();
            m_FileName = szFile;
            m_szTitle = "";
            if (File.Exists(szFile))
            {
                Parsing();
            }
            else
            {
                WriteLog(String.Format("ini 파일이 없습니다 : [{0}]", szFile));
            }
        }

        public String GetFileName() { return m_FileName; }
        public void Parsing()
        {
            WriteLog("<<< -------------------- INI Load -------------------- >>>");
            String szLine = String.Empty;
            String szLastLine = String.Empty;

            StreamReader sr = new StreamReader(m_FileName, Encoding.Default);
            dicKeyList tempKeyList = new dicKeyList();
            while (sr.Peek() > -1)
            {
                if (szLastLine == String.Empty || szLastLine[0] != '[')
                {
                    szLine = sr.ReadLine();
                }
                else
                {
                    szLine = szLastLine;
                    szLastLine = String.Empty;
                }

                szLine = szLine.Replace("\t", "");
                if (szLine == String.Empty)
                {
                    continue;
                }

                if (szLine[0] != '[')
                {
                    continue;
                }

                szLastLine = ParseTitle(szLine, ref sr);
            }

            sr.Close();
            WriteLog("<<< -------------------- INI Load Complete -------------------- >>>");
            if (m_bSaveLog)
                m_Log.Close();
        }

        private String ParseTitle(String szTitle, ref StreamReader sr)
        {
            String szLine = "";
            dicKeyList tempKeyList = new dicKeyList();
            while (sr.Peek() > -1)
            {
                szLine = sr.ReadLine();
                szLine = szLine.Replace("\t", "");
                if (szLine == String.Empty)
                    continue;

                if (szLine[0] == ';')
                    continue;

                if (szLine[0] == '[')
                    break;

                ParseKey(szTitle, szLine, ref tempKeyList);
            }

            if (tempKeyList.Count > 0)
            {
                char[] charSeparators = new char[] { '[', ']' };
                String[] RetString = szTitle.Split(charSeparators);
                if (RetString.Length < 3)
                    WriteLog(String.Format("알수없는 오류 발생 : {0}", szTitle));

                dicKeyList retList;
                if (!m_TitleList.TryGetValue(RetString[1], out retList))
                {
                    m_TitleList.Add(RetString[1], tempKeyList);
                }
                else
                {
                    int iCnt = retList.Count;
                    WriteLog(String.Format("중복섹션이 발생했습니다. : {0}", szTitle));
                }
            }

            return szLine;
        }

        private void ParseKey(String szTitle, String szLine, ref dicKeyList KeyList)
        {
            char[] charSeparators = new char[] { '=', '\t' };
            String[] szWord = szLine.Split(charSeparators, 2);
            if (szWord.Length > 1)
            {
                String szKey = szWord[0].Trim();
                String szValue = szWord[1].Trim();
                String RetValue;
                if (!KeyList.TryGetValue(szKey, out RetValue))
                {
                    KeyList.Add(szKey, szValue);
                }
                else
                {
                    WriteLog(String.Format("중복키가 발생했습니다. : {0} {1}", szTitle, szKey));
                }
            }
        }

        public void SetTitle(String szSection)
        {
            m_szTitle = szSection;
        }

        public void WriteValue(String key, String value)
        {
            if (String.IsNullOrEmpty(m_FileName) || String.IsNullOrEmpty(m_szTitle))
                return;

            WritePrivateProfileString(m_szTitle, key, String.Format(" {0}", value), m_FileName);
        }

        public void WriteSection(String section, String key, String value)
        {
            WritePrivateProfileString(section, key, value, m_FileName);
        }

        public String GetValue(String key)
        {
            if (String.IsNullOrEmpty(m_szTitle))
                return String.Empty;

            String retValue = "";
            dicKeyList KeyList;
            try
            {
                if (m_TitleList.TryGetValue(m_szTitle, out KeyList))
                {
                    if (KeyList.TryGetValue(key, out retValue))
                        return retValue;
                }
            }
            catch
            {

            }

            return retValue;
        }

        public String LoadString(String key, String defVal)
        {
            String retVal = GetValue(key);
            if (String.IsNullOrEmpty(retVal))
                return defVal;

            return retVal.ToString();
        }

        public int LoadInt(String key, int defVal)
        {
            try
            {
                String retVal = GetValue(key);
                if (String.IsNullOrEmpty(retVal))
                    return defVal;

                return Int32.Parse(retVal);
            }
            catch
            {

            }
            return defVal;
        }

        public uint LoadUint(String key, uint defVal)
        {
            try
            {
                String retVal = GetValue(key);
                if (String.IsNullOrEmpty(retVal))
                    return defVal;

                return uint.Parse(retVal);
            }
            catch
            {

            }
            return defVal;
        }

        public float LoadFloat(String key, float defVal)
        {
            String retVal = GetValue(key);
            if (String.IsNullOrEmpty(retVal))
                return defVal;

            return float.Parse(retVal);
        }

        public bool LoadBool(String key, bool defVal)
        {
            try
            {
                String retVal = GetValue(key);
                if (String.IsNullOrEmpty(retVal))
                    return defVal;

                if (int.Parse(retVal) == 0)
                    return false;
                else
                    return true;
            }
            catch
            {

            }
            return defVal;
        }

        private void WriteLog(String log)
        {
            if (!m_bSaveLog)
                return;

            if (m_Log != null)
            {
                m_Log.WriteLine(log);
            }
        }
    }
}
