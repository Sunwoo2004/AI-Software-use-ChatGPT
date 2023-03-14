namespace AISoftWare_SW
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AnswerBox = new System.Windows.Forms.TextBox();
            this.FirstQuestion = new System.Windows.Forms.Button();
            this.MoreQuestion = new System.Windows.Forms.Button();
            this.QuestionBox = new System.Windows.Forms.TextBox();
            this.AnswerGetTime = new System.Windows.Forms.Timer(this.components);
            this.ReadyCheck = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // AnswerBox
            // 
            this.AnswerBox.Location = new System.Drawing.Point(13, 66);
            this.AnswerBox.Multiline = true;
            this.AnswerBox.Name = "AnswerBox";
            this.AnswerBox.Size = new System.Drawing.Size(434, 25);
            this.AnswerBox.TabIndex = 0;
            this.AnswerBox.Visible = false;
            this.AnswerBox.TextChanged += new System.EventHandler(this.AnswerBox_TextChanged);
            // 
            // FirstQuestion
            // 
            this.FirstQuestion.Location = new System.Drawing.Point(12, 33);
            this.FirstQuestion.Name = "FirstQuestion";
            this.FirstQuestion.Size = new System.Drawing.Size(212, 27);
            this.FirstQuestion.TabIndex = 1;
            this.FirstQuestion.Text = "첫 질문";
            this.FirstQuestion.UseVisualStyleBackColor = true;
            this.FirstQuestion.Click += new System.EventHandler(this.FirstQuestion_Click);
            // 
            // MoreQuestion
            // 
            this.MoreQuestion.Enabled = false;
            this.MoreQuestion.Location = new System.Drawing.Point(234, 33);
            this.MoreQuestion.Name = "MoreQuestion";
            this.MoreQuestion.Size = new System.Drawing.Size(212, 27);
            this.MoreQuestion.TabIndex = 2;
            this.MoreQuestion.Text = "추가 질문";
            this.MoreQuestion.UseVisualStyleBackColor = true;
            this.MoreQuestion.Click += new System.EventHandler(this.MoreQuestion_Click);
            // 
            // QuestionBox
            // 
            this.QuestionBox.Location = new System.Drawing.Point(13, 7);
            this.QuestionBox.Name = "QuestionBox";
            this.QuestionBox.Size = new System.Drawing.Size(433, 21);
            this.QuestionBox.TabIndex = 3;
            // 
            // AnswerGetTime
            // 
            this.AnswerGetTime.Tick += new System.EventHandler(this.AnswerGetTime_Tick);
            // 
            // ReadyCheck
            // 
            this.ReadyCheck.Enabled = true;
            this.ReadyCheck.Tick += new System.EventHandler(this.ReadyCheck_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(458, 65);
            this.Controls.Add(this.QuestionBox);
            this.Controls.Add(this.MoreQuestion);
            this.Controls.Add(this.FirstQuestion);
            this.Controls.Add(this.AnswerBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "AI SoftWare";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AnswerBox;
        private System.Windows.Forms.Button FirstQuestion;
        private System.Windows.Forms.Button MoreQuestion;
        private System.Windows.Forms.TextBox QuestionBox;
        private System.Windows.Forms.Timer AnswerGetTime;
        private System.Windows.Forms.Timer ReadyCheck;
    }
}

