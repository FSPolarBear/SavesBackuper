using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SavesBackuper
{
    /// <summary>
    /// 将存档信息保存在配置文件中
    /// </summary>
    class Config
    {
        private static readonly string CONFIG_FILE = "config.json";


        //所有游戏存档信息
        private List<SaveInfo> infos;

        //当前游戏编号
        public int Index { get; private set; }


        /// <summary>
        /// 从config.json中读取游戏存档信息和当前游戏编号。如读取错误或无config.json则设置存档信息为空，存档编号为0。
        /// </summary>
        public Config()
        {
            try
            {
                string configString = File.ReadAllText(CONFIG_FILE);
                JObject config = JObject.Parse(configString);

                Index = config.Value<int>("index");

                JArray array = config.Value<JArray>("infos");
                infos = new List<SaveInfo>();
                for(int i=0; i<array.Count;i++)
                {
                    JObject json = array.Value<JObject>(i);
                    SaveInfo info = new SaveInfo();
                    info.FromJson(json);
                    infos.Add(info);
                }
            }catch(Exception ex)
            {
                Index = 0;
                infos = new List<SaveInfo>();
            }
        }



        /// <summary>
        /// 返回当前游戏存档信息
        /// </summary>
        /// <returns></returns>
        public SaveInfo GetInfo()
        {
            if (Index < 0 || Index >= infos.Count)
                return null;
            return new SaveInfo(infos[Index]);
        }


        /// <summary>
        /// 将游戏信息和游戏存档保存至config.json
        /// </summary>
        private void SaveConfig()
        {
            JObject config = new JObject();
            config.Add("index", Index);

            JArray array = new JArray();
            for(int i=0;i<infos.Count;i++)
            {
                JObject json = infos[i].ToJson();
                array.Add(json);
            }
            config.Add("infos", array);

            File.WriteAllText(CONFIG_FILE, config.ToString());
        }

        /// <summary>
        /// 切换当前游戏
        /// </summary>
        /// <param name="index">要切换的游戏编号</param>
        public void Change(int index)
        {
            // 0 <= index < infos.Count是有效值。否则什么都不做。
            if(index < 0 || index >= infos.Count)
            {
                return;
            }

            this.Index = index;
            SaveConfig();

        }

        /// <summary>
        /// 创建一个新游戏的存档信息，并将当前游戏设置为新创建的游戏。如果游戏名已存在则创建失败
        /// </summary>
        /// <param name="GameName">新游戏名</param>
        /// <returns>是否创建成功</returns>
        public bool AddSaveInfo(string GameName)
        {
            foreach (SaveInfo saveinfo in infos)
            {
                if (saveinfo.GameName == GameName)
                    return false;
            }

            SaveInfo info = new SaveInfo(GameName);
            infos.Add(info);
            Change(infos.Count - 1);
            SaveConfig();
            return true;
        }

        /// <summary>
        /// 删除一个游戏的存档信息。
        /// </summary>
        /// <param name="index">删除的游戏编号。若无参数则默认为this.Index</param>
        public void DeleteSaveInfo(int index)
        {
            if (index < 0 || index >= infos.Count)
            {
                return;
            }

            infos.RemoveAt(index);

            //如果当前游戏为删除的游戏，则将当前游戏设为上一个游戏。否则不变。
            if (Index >= index)
            {
                Change(index - 1);
            }

            SaveConfig();
        }
        public void DeleteSaveInfo()
        {
            DeleteSaveInfo(this.Index);
        }

        /// <summary>
        /// 修改当前游戏存档信息的存档文件路径
        /// </summary>
        /// <param name="Directory">存档文件路径</param>
        public void SetDirectory(string Directory)
        {
            infos[Index].Directory = Directory;
            SaveConfig();
        }

        /// <summary>
        /// 修改当前游戏存档信息的是否使用可变路径
        /// </summary>
        /// <param name="UseChangeableDirectory">是否使用可变路径</param>
        public void SetUseChangeableDirectory(bool UseChangeableDirectory)
        {
            infos[Index].UseChangeableDirectory = UseChangeableDirectory;
            SaveConfig();
        }

        /// <summary>
        /// 修改当前游戏存档信息的可变路径
        /// </summary>
        /// <param name="ChangeableDirectory">可变路径</param>
        public void SetChangeableDirectory(string ChangeableDirectory)
        {
            infos[Index].ChangeableDirectory = ChangeableDirectory;
            SaveConfig();
        }
        /*
        /// <summary>
        /// 修改当前选中存档名编号
        /// </summary>
        /// <param name="SaveNameIndex">存档名编号</param>
        public void SetSaveNameIndex(int SaveNameIndex)
        {
            if(SaveNameIndex < 0 || SaveNameIndex >= infos[Index].SaveName.Count)
            {
                return;
            }

            infos[Index].SaveNameIndex = SaveNameIndex;
            SaveConfig();
        }*/

        /// <summary>
        /// 获取所有游戏名
        /// </summary>
        /// <returns>所有游戏名</returns>
        public List<string> GetGameNames()
        {
            List<string> result = new List<string>();
            for(int i=0;i<infos.Count;i++)
            {
                result.Add(infos[i].GameName);
            }
            return result;
        }

        /// <summary>
        /// 添加新存档名，并将当前存档名设为新添加的存档名。如果存档名已存在则添加失败。
        /// </summary>
        /// <param name="Name">新存档名</param>
        /// <returns>是否创建成功</returns>
        public bool AddSaveName(string Name)
        {
            foreach(string name in infos[Index].SaveName)
            {
                if(name == Name)
                {
                    return false;
                }
            }


            infos[Index].SaveName.Add(Name);
            infos[Index].SaveNameIndex = infos[Index].SaveName.Count - 1;
            SaveConfig();
            return true;
        }

        /// <summary>
        /// 切换存档名
        /// </summary>
        /// <param name="index">存档名编号</param>
        public void ChangeSaveName(int index)
        {
            if (index < 0 || index >= infos[Index].SaveName.Count)
            {
                return;
            }
            infos[Index].SaveNameIndex = index;
            SaveConfig();
        }

        

        /// <summary>
        /// 删除一个存档名
        /// </summary>
        /// <param name="index">删除的存档名编号。若无参数则默认为this.infos[this.Index].SaveNameIndex</param>
        public void DeleteSaveName(int index)
        {
            if(index < 0 || index >= infos[Index].SaveName.Count)
            {
                return;
            }

            infos[Index].SaveName.RemoveAt(index);
            //如果删除的存档名为选中存档名，则切换至前一个存档名。否则不变。
            if(infos[Index].SaveNameIndex >= index)
            {
                //infos[Index].SaveNameIndex -= 1;
                ChangeSaveName(infos[Index].SaveNameIndex - 1);
            }

            SaveConfig();
        }
        public void DeleteSaveName()
        {
            DeleteSaveName(this.infos[this.Index].SaveNameIndex);
        }

    }
}
