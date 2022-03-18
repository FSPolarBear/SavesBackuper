using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SavesBackuper
{
    public partial class InputDialog : Form
    {
        public string Value { get; private set; }

        public InputDialog(string title, string text)
        {
            InitializeComponent();
            button_OK.Text = Texts.INPUTDIALOG_OK;
            button_Cancel.Text = Texts.INPUTDIALOG_CANCEL;
            this.Text = title;
            textBox_Input.Text = text;
            Value = text;
        }

        public InputDialog(string title) : this(title, "") { }



        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OK_Click(object sender, EventArgs e)
        {
            OK();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void OK()
        {
            Value = textBox_Input.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 按回车确认，ESC取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void textBox_Input_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    OK();
                    break;
                case Keys.Escape:
                    Cancel();
                    break;
            }
        }
    }
}
