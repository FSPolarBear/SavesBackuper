using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace SavesBackuper{
    /// <summary>
    /// 备份和恢复
    /// </summary>
    static class BackupRestore
    {
        

        //默认备份文件
        //{0}:GameName        {1}:SaveName[SaveNameIndex]
        private static readonly string DEFAULT_BACKUP_FILE_FORMAT = @"saves\{0}\default_save_file_of_{1}.defaultsave";


        


        /// <summary>
        /// 备份到指定位置
        /// </summary>
        /// <param name="info">要备份的存档信息</param>
        /// <param name="target">备份文件名和路径</param>
        /// <returns>是否备份成功</returns>
        public static bool Backup(SaveInfo info, string target)
        {
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return false;

            try
            {
                
                string source = info.GetFileName();

                //目标文件所在文件夹。如果不存在就创建。用于备份到默认位置。
                string dir = Path.GetDirectoryName(target);
                if(!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.Copy(source, target, true);

                //test
                /*if (!File.Exists(filename))
                    throw new Exception();*/
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 备份到默认位置
        /// </summary>
        /// <param name="info">要备份的存档信息</param>
        /// <returns>是否备份成功</returns>
        public static bool DefaultBackup(SaveInfo info)
        {
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return false;

            string target = string.Format(DEFAULT_BACKUP_FILE_FORMAT,info.GameName, info.SaveName[info.SaveNameIndex]);
            return Backup(info, target);
        }


        /// <summary>
        /// 从指定位置还原
        /// </summary>
        /// <param name="info">要还原的存档信息</param>
        /// <param name="backup">备份文件名和路径</param>
        /// <returns>是否还原成功</returns>
        public static bool Restore(SaveInfo info, string backup)
        {
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return false;

            try
            {
                //
                string target = info.GetFileName();

                File.Copy(backup, target, true);

                //test
                /*if (!File.Exists(filename))
                    throw new Exception();*/
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 从默认位置还原
        /// </summary>
        /// <param name="info">要还原的存档信息</param>
        /// <returns>是否还原成功</returns>
        public static bool DefaultRestore(SaveInfo info)
        {
            if (info.SaveNameIndex < 0 || info.SaveNameIndex >= info.SaveName.Count)
                return false;

            string backup = string.Format(DEFAULT_BACKUP_FILE_FORMAT, info.GameName, info.SaveName[info.SaveNameIndex]);
            return Restore(info, backup);
        }


        /// <summary>
        /// 将所有存档名的存档备份至默认位置
        /// </summary>
        /// <param name="info">要备份的存档信息</param>
        /// <returns>每个存档是否备份成功</returns>
        public static bool[] DefaultBackupBatch(SaveInfo info)
        {
            bool[] succeed = new bool[info.SaveName.Count];

            SaveInfo saveinfo = new SaveInfo(info);

            for (int i = 0; i < succeed.Length; i++)
            {
                saveinfo.SaveNameIndex = i;
                succeed[i] = DefaultBackup(saveinfo);
            }
            return succeed;
        }


        /// <summary>
        /// 从默认位置恢复所有存档名的存档
        /// </summary>
        /// <param name="info">要还原的存档信息</param>
        /// <returns>每个存档是否还原成功</returns>
        public static bool[] DefaultRestoreBatch(SaveInfo info)
        {
            bool[] succeed = new bool[info.SaveName.Count];
            SaveInfo saveinfo = new SaveInfo(info);
            for(int i = 0; i < succeed.Length; i++)
            {
                saveinfo.SaveNameIndex=i;
                succeed[i] = DefaultRestore(saveinfo);
            }
            return succeed;
        }


    }
}
