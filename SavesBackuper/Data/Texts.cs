using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavesBackuper
{
    /// <summary>
    /// 数据字典类。所有程序中出现的字符串放在这里。
    /// </summary>
    public static class Texts
    {
        //MainForm UI
        public static readonly string TITLE = "存档备份器";
        public static readonly string LABEL_SAVEINFO = "选择游戏：";
		public static readonly string BUTTON_SAVEINFO_ADD = "添加游戏";
		public static readonly string BUTTON_SAVEINFO_DELETE = "移除当前游戏";
		public static readonly string LABEL_DIRECTORY = "存档目录：";
		public static readonly string BUTTON_DIRECTORY_INPUT = "输入存档目录";
		public static readonly string BUTTON_DIRECTORY_CHOOSE = "选择存档目录";
		public static readonly string CHECKBOX_USECHANGEABLEDIRECTORY = "使用可变路径";
		public static readonly string LABEL_CHANGEABLEDIRECTORY = "可变路径：";
		public static readonly string LABEL_SAVENAME = "存档名：";
		public static readonly string BUTTON_SAVENAME_ADD = "添加存档名";
		public static readonly string BUTTON_SAVENAME_DELETE = "移除存档名";
		public static readonly string BUTTON_BACKUP_DEFAULT = "备份至默认位置";
		public static readonly string BUTTON_BACKUP_CUSTOM = "备份至自定义位置";
		public static readonly string BUTTON_RESTORE_DEFAULT = "从默认备份中恢复";
		public static readonly string BUTTON_RESTORE_CUSTOM = "从自定义备份中恢复";

		//InputDialog UI
		public static readonly string INPUTDIALOG_OK = "确定";
		public static readonly string INPUTDIALOG_CANCEL = "取消";


		//InputDialog title
		public static readonly string INPUTDIALOG_TITLE_SAVEINFO_ADD = "输入游戏名";
		public static readonly string INPUTDIALOG_TITLE_SAVENAME_ADD = "输入存档名";
		public static readonly string INPUTDIALOG_TITLE_DIRECTORY = "输入存档目录，使用??表示可变路径";

		//Message
		public static readonly string MESSAGE_SAVEINFO_DELETE = "是否移除游戏？";
		public static readonly string MESSAGE_SAVENAME_DELETE = "是否移除存档名？";
		public static readonly string MESSAGE_BACKUP_DEFAULT = "是否备份到默认位置？";
		public static readonly string MESSAGE_RESTORE_DEFAULT = "是否从默认位置还原？";
		public static readonly string MESSAGE_BACKUP_SUCCEED = "备份成功";
		public static readonly string MESSAGE_BACKUP_FAILED = "备份失败";
		public static readonly string MESSAGE_RESTORE_SUCCEED = "还原成功";
		public static readonly string MESSAGE_RESTORE_FAILED = "还原失败";
		public static readonly string MESSAGE_CHOOSE_DIRECTORY_OR_FILE_FAILED = "选择路径或文件失败";


		public static string JSON_EXCEPTION_MESSAGE = "存档信息解析失败";

    }
}
