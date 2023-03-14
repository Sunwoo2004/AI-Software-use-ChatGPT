using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AISoftWare_SW
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void AnswerBox_TextChanged(object sender, EventArgs e)
        {
            int iTextLength = AnswerBox.TextLength;
            int iLine = AnswerBox.GetLineFromCharIndex(iTextLength) + 1;
            if (iLine > AnswerBox.Lines.Length)
            {
                int NewHeight = (int)(iLine * (AnswerBox.Font.Height + AnswerBox.Font.Height * 0.2));
                int iCurrentWidth = AnswerBox.Width;

                AnswerBox.Multiline = false;
                AnswerBox.Width = 10;
                AnswerBox.Multiline = true;
                AnswerBox.Width = iCurrentWidth;
                AnswerBox.Height = NewHeight;
            }
        }

        private void FirstQuestion_Click(object sender, EventArgs e)
        {
            if (QuestionBox.Text.Length == 0)
            {
                MessageBox.Show("질문할 메시지를 입력 후 다시 시도해주세요.", "AISoftWare_SW");
                return;
            }
            
            AnswerBox.Visible = true; //답장 박스는 켜고
            
            MoreQuestion.Enabled = false; //추가질문도 닫고
            
            FirstQuestion.Enabled = false; //새 질문은 닫음

            SeleniumHelper.OnFinishReset();

            SeleniumHelper.SetSearchText(QuestionBox.Text);
            Thread thread = new Thread(SeleniumHelper.OnFirstSearch);
            thread.Start();

            AnswerGetTime.Enabled = true;
        }

        private void MoreQuestion_Click(object sender, EventArgs e)
        {
            if (QuestionBox.Text.Length == 0)
            {
                MessageBox.Show("추가 질문할 메시지를 입력 후 다시 시도해주세요.", "AISoftWare_SW");
                return;
            }

            MoreQuestion.Enabled = false; //추가질문도 닫고

            FirstQuestion.Enabled = false; //새 질문은 닫음

            SeleniumHelper.OnFinishReset();

            SeleniumHelper.SetSearchText(QuestionBox.Text);
            Thread thread = new Thread(SeleniumHelper.OnMoreSearch);
            thread.Start();

            AnswerGetTime.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SeleniumHelper.OnMainSetting();
        }

        private void AnswerGetTime_Tick(object sender, EventArgs e)
        {
            if (SeleniumHelper.IsFinish() == true)
            {
                AnswerGetTime.Enabled = false;
                MoreQuestion.Enabled = true;
                FirstQuestion.Enabled = true;
            }
            AnswerBox.Text = SeleniumHelper.GetAnswer();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SeleniumHelper.OnQuitSelenium();
        }

        private void ReadyCheck_Tick(object sender, EventArgs e)
        {
            if (SeleniumHelper.IsReady() == true)
            {
                ReadyCheck.Enabled = false;
                this.Text = "AI SoftWare [Ready]";
            }
        }
    }
}
