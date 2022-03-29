using Newtonsoft.Json.Linq;
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
    public partial class MainForm : Form
    {

        private Config config = new Config();

        //若当前无游戏，则此项为false，除添加游戏外所有功能均不可用
        private bool enable = false;

        private SaveInfo info = null;

        //默认备份路径
        //{0}:GameName
        private static readonly string DEFAULT_BACKUP_DIRECTORY_FORMAT = Environment.CurrentDirectory +  @"\saves\{0}";

        public MainForm()
        {
            InitializeComponent();


            //test
            /*config.AddSaveInfo("test2");
            config.SetDirectory(@"E:\testDirectory\test2asda");
            config.SetUseChangeableDirectory(false);

            config.AddSaveName("save1");*/


        }


        /// <summary>
        /// 初始化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// version 1.1.0
        private void MainForm_Load(object sender, EventArgs e)
        {
            //加载显示的字符串
            this.Text = Texts.TITLE;
            textBox_label_SaveInfo.Text = Texts.LABEL_SAVEINFO;
            textBox_label_Directory.Text = Texts.LABEL_DIRECTORY;
            textBox_label_ChangeableDirectory.Text = Texts.LABEL_CHANGEABLEDIRECTORY;
            textBox_label_SaveName.Text = Texts.LABEL_SAVENAME;
            button_SaveInfo_Add.Text = Texts.BUTTON_SAVEINFO_ADD;
            button_SaveInfo_Delete.Text = Texts.BUTTON_SAVEINFO_DELETE;
            button_Directory_Input.Text = Texts.BUTTON_DIRECTORY_INPUT;
            button_Directory_Choose.Text = Texts.BUTTON_DIRECTORY_CHOOSE;
            button_SaveName_Add.Text = Texts.BUTTON_SAVENAME_ADD;
            button_SaveName_Delete.Text = Texts.BUTTON_SAVENAME_DELETE;
            button_Backup_Default.Text = Texts.BUTTON_BACKUP_DEFAULT;
            button_Backup_Custom.Text = Texts.BUTTON_BACKUP_CUSTOM;
            button_Backup_Batch_Default.Text= Texts.BUTTON_BACKUP_BATCH_DEFAULT;
            button_Restore_Default.Text = Texts.BUTTON_RESTORE_DEFAULT;
            button_Restore_Custom.Text = Texts.BUTTON_RESTORE_CUSTOM;
            button_Restore_Batch_Default.Text = Texts.BUTTON_RESTORE_BATCH_DEFAULT;
            checkBox_UseChangeableDirectory.Text = Texts.CHECKBOX_USECHANGEABLEDIRECTORY;

            //加载信息
            RefreshGameNameList();
            RefreshSaveInfo();
        }

        /// <summary>
        /// 加载游戏名列表
        /// </summary>
        private void RefreshGameNameList()
        {
            comboBox_SaveInfo.Items.Clear();
            List<string> names = config.GetGameNames();
            comboBox_SaveInfo.Items.AddRange(names.ToArray<string>());
            if (comboBox_SaveInfo.Items.Count > 0)
            {
                comboBox_SaveInfo.SelectedIndex = config.Index;
            }
           
        }



        /// <summary>
        /// 重新加载当前游戏信息
        /// </summary>
        private void RefreshSaveInfo()
        {
            info = config.GetInfo();
            if(info == null)
            {
                enable = false;
                
                textBox_Directory.Text = "";
                checkBox_UseChangeableDirectory.Checked = false;
                textBox_ChangeableDirectory.Text = "";
                textBox_ChangeableDirectory.Enabled = false;
                comboBox_SaveName.Items.Clear();

            }
            else
            {
                enable = true;
                textBox_Directory.Text = info.Directory;     
                checkBox_UseChangeableDirectory.Checked = info.UseChangeableDirectory;
                textBox_ChangeableDirectory.Enabled = info.UseChangeableDirectory;
                textBox_ChangeableDirectory.Text = info.ChangeableDirectory;
                comboBox_SaveName.Items.Clear();
                comboBox_SaveName.Items.AddRange(info.SaveName.ToArray<string>());
                if(comboBox_SaveName.Items.Count > 0 )
                {
                    comboBox_SaveName.SelectedIndex = info.SaveNameIndex;
                }
                

            }
        }


        /// <summary>
        /// 切换选中的游戏。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_SaveInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!enable)
                return;
            config.Change(comboBox_SaveInfo.SelectedIndex);
            RefreshSaveInfo();
        }


        /// <summary>
        /// 切换存档名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_SaveName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!enable)
                return;
            config.ChangeSaveName(comboBox_SaveName.SelectedIndex);
        }


        /// <summary>
        /// 添加游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SaveInfo_Add_Click(object sender, EventArgs e)
        {
            InputDialog inputDialog = new InputDialog(Texts.INPUTDIALOG_TITLE_SAVEINFO_ADD);
            if(inputDialog.ShowDialog() == DialogResult.OK)
            {
                string name = inputDialog.Value;
                
                //如果添加成功则刷新界面
                if(config.AddSaveInfo(name))
                {
                    RefreshGameNameList();
                    RefreshSaveInfo();
                }
            }
        }


        /// <summary>
        /// 移除游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SaveInfo_Delete_Click(object sender, EventArgs e)
        {
            if (!enable)
                return;
            if (MessageBox.Show(Texts.MESSAGE_SAVEINFO_DELETE,"", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                config.DeleteSaveInfo();
                RefreshGameNameList();
                RefreshSaveInfo();
            }
            
        }

        /// <summary>
        /// 输入存档目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Directory_Input_Click(object sender, EventArgs e)
        {
            if (!enable)
                return;
            InputDialog inputDialog = new InputDialog(Texts.INPUTDIALOG_TITLE_DIRECTORY, info.Directory);
            if (inputDialog.ShowDialog() == DialogResult.OK)
            {
                string directory = inputDialog.Value;
                config.SetDirectory(directory);
                RefreshSaveInfo();
            }
        }

        /// <summary>
        /// 选择存档目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Directory_Choose_Click(object sender, EventArgs e)
        {
            if (!enable)
                return;

            //此处选择目录时选“WPS网盘”会抛异常，所以加了个try
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string directory = fbd.SelectedPath;
                    config.SetDirectory(directory);
                    RefreshSaveInfo();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(Texts.MESSAGE_CHOOSE_DIRECTORY_OR_FILE_FAILED);
            }
            
        }

        /// <summary>
        /// 切换是否使用可变目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_UseChangeableDirectory_CheckedChanged(object sender, EventArgs e)
        {
            if (!enable)
                return;
            config.SetUseChangeableDirectory(checkBox_UseChangeableDirectory.Checked);
            textBox_ChangeableDirectory.Enabled = checkBox_UseChangeableDirectory.Checked;
        }

        /// <summary>
        /// 完成输入可变路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_ChangeableDirectory_Leave(object sender, EventArgs e)
        {
            if (!enable)
                return;
            string ChangeableDirectory = textBox_ChangeableDirectory.Text;
            config.SetChangeableDirectory(ChangeableDirectory);
        }


        /// <summary>
        /// 添加存档名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SaveName_Add_Click(object sender, EventArgs e)
        {
            if (!enable)
                return;
            InputDialog inputDialog = new InputDialog(Texts.INPUTDIALOG_TITLE_SAVENAME_ADD);
            if (inputDialog.ShowDialog() == DialogResult.OK)
            {
                string name = inputDialog.Value;

                //如果添加成功则刷新界面
                if (config.AddSaveName(name))
                {
                    RefreshSaveInfo();
                }
            }
        }

        /// <summary>
        /// 移除存档名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SaveName_Delete_Click(object sender, EventArgs e)
        {
            //如无存档名，按钮不生效。
            if (!enable)
                return;
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return;

            if (MessageBox.Show(Texts.MESSAGE_SAVENAME_DELETE, "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                config.DeleteSaveName();
                RefreshSaveInfo();
            }
        }


        /// <summary>
        /// 保存到默认位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Backup_Default_Click(object sender, EventArgs e)
        {
            //如无存档名，按钮不生效。
            if (!enable)
                return;
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return ;

            if (MessageBox.Show(Texts.MESSAGE_BACKUP_DEFAULT, "",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (BackupRestore.DefaultBackup(info))
                {
                    MessageBox.Show(Texts.MESSAGE_BACKUP_SUCCEED);
                }
                else
                {
                    MessageBox.Show(Texts.MESSAGE_BACKUP_FAILED);
                }
            }
        }

        /// <summary>
        /// 备份到指定位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Backup_Custom_Click(object sender, EventArgs e)
        {
            //如无存档名，按钮不生效。
            if (!enable)
                return;
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return;

            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                string backup_directory = string.Format(DEFAULT_BACKUP_DIRECTORY_FORMAT, info.GameName);
                sfd.InitialDirectory = backup_directory;
                sfd.FileName = info.SaveName[info.SaveNameIndex];
                

                if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName != null)
                {
                    if (BackupRestore.Backup(info, sfd.FileName))
                    {
                        MessageBox.Show(Texts.MESSAGE_BACKUP_SUCCEED);
                    }
                    else
                    {
                        MessageBox.Show(Texts.MESSAGE_BACKUP_FAILED);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Texts.MESSAGE_CHOOSE_DIRECTORY_OR_FILE_FAILED);
            }

            
        }

        /// <summary>
        /// 从默认位置还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Restore_Default_Click(object sender, EventArgs e)
        {
            //如无存档名，按钮不生效。
            if (!enable)
                return;
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return;


            if (MessageBox.Show(Texts.MESSAGE_RESTORE_DEFAULT, "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (BackupRestore.DefaultRestore(info))
                {
                    MessageBox.Show(Texts.MESSAGE_RESTORE_SUCCEED);
                }
                else
                {
                    MessageBox.Show(Texts.MESSAGE_RESTORE_FAILED);
                }
            }
        }

        /// <summary>
        /// 从指定位置还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Restore_Custom_Click(object sender, EventArgs e)
        {
            //如无存档名，按钮不生效。
            if (!enable)
                return;
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return;

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                string backup_directory = string.Format(DEFAULT_BACKUP_DIRECTORY_FORMAT, info.GameName);
                ofd.InitialDirectory = backup_directory;

                if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != null)
                {
                    if (BackupRestore.Restore(info, ofd.FileName))
                    {
                        MessageBox.Show(Texts.MESSAGE_RESTORE_SUCCEED);
                    }
                    else
                    {
                        MessageBox.Show(Texts.MESSAGE_RESTORE_FAILED);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Texts.MESSAGE_CHOOSE_DIRECTORY_OR_FILE_FAILED);
            }
            
        }


        /// <summary>
        /// 批量备份至默认位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// version 1.1.0
        private void button_Backup_Batch_Default_Click(object sender, EventArgs e)
        {
            //如无存档名，按钮不生效。
            if (!enable)
                return;
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return;

            try
            {
                if(MessageBox.Show(Texts.MESSAGE_BACKUP_BATCH_DEFAULT, "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    bool[] succeed = BackupRestore.DefaultBackupBatch(info);
                    int numFailed = 0;//失败的数量
                    string failSaves = "";  //失败的文件名，多个文件使用顿号隔开
                    for(int i = 0; i < succeed.Length; i++)
                    {
                        if(!succeed[i])
                        {
                            if(numFailed > 0)
                            {
                                failSaves += Texts.COMMA;
                            }
                            failSaves += info.SaveName[i];
                            numFailed++;
                        }
                    }
                    //全部成功
                    if(numFailed == 0)
                    {
                        MessageBox.Show(Texts.MESSAGE_BACKUP_BATCH_ALL_SUCCEED);
                    }
                    //全部失败
                    else if(numFailed == succeed.Length)
                    {    
                        MessageBox.Show(Texts.MESSAGE_BACKUP_BATCH_ALL_FAILED);
                    }
                    //部分成功
                    else
                    {
                        MessageBox.Show(string.Format(Texts.MESSAGE_BACKUP_BATCH_PART_SUCCEED_FORMAT, succeed.Length, succeed.Length - numFailed, failSaves));
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(Texts.MESSAGE_BACKUP_BATCH_ERROR);
            }


        }


        /// <summary>
        /// 从默认位置批量恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// version 1.1.0
        private void button_Restore_Batch_Default_Click(object sender, EventArgs e)
        {
            //如无存档名，按钮不生效。
            if (!enable)
                return;
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return;

            try
            {
                if (MessageBox.Show(Texts.MESSAGE_RESTORE_BATCH_DEFAULT, "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    bool[] succeed = BackupRestore.DefaultRestoreBatch(info);
                    int numFailed = 0;//失败的数量
                    string failSaves = "";  //失败的文件名，多个文件使用顿号隔开
                    for (int i = 0; i < succeed.Length; i++)
                    {
                        if (!succeed[i])
                        {
                            if (numFailed > 0)
                            {
                                failSaves += Texts.COMMA;
                            }
                            failSaves += info.SaveName[i];
                            numFailed++;
                        }
                    }
                    //全部成功
                    if (numFailed == 0)
                    {
                        MessageBox.Show(Texts.MESSAGE_RESTORE_BATCH_ALL_SUCCEED);
                    }
                    //全部失败
                    else if (numFailed == succeed.Length)
                    {
                        MessageBox.Show(Texts.MESSAGE_RESTORE_BATCH_ALL_FAILED);
                    }
                    //部分成功
                    else
                    {
                        MessageBox.Show(string.Format(Texts.MESSAGE_RESTORE_BATCH_PART_SUCCEED_FORMAT, succeed.Length, succeed.Length - numFailed, failSaves));
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(Texts.MESSAGE_RESTORE_BATCH_ERROR);
            }
        }
    }
}
