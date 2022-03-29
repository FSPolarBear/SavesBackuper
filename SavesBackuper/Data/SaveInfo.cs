using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SavesBackuper
{
    /// <summary>
    /// 存档信息
    /// </summary>
    public class SaveInfo : IJsonObject
    {
        public string GameName { get; private set; }
        public string Directory { get; set; }
        public bool UseChangeableDirectory { get; set; }

        public string ChangeableDirectory { get; set; }

        public List<string> SaveName { get; }

        public int SaveNameIndex { get; set; }


        public SaveInfo() : this("") { }

        public SaveInfo(string GameName)
        {
            this.GameName = GameName;
            this.Directory = "";
            this.UseChangeableDirectory = false;
            this.ChangeableDirectory = "";
            this.SaveName = new List<string>();
            this.SaveNameIndex = 0;

        }

        public SaveInfo(SaveInfo saveInfo)
        {
            this.SaveName = new List<string>();
            this.FromJson(saveInfo.ToJson());
        }

        public void FromJson(JObject json)
        {
            try
            {
                this.GameName = json.Value<string>("GameName");

                if (!json.ContainsKey("Directory"))
                {
                    this.Directory = "";
                }
                else
                {
                    this.Directory = json.Value<string>("Directory");
                }
                

                if (!json.ContainsKey("UseChangeableDirectory"))
                {
                    this.UseChangeableDirectory = false;
                }
                else
                {
                    this.UseChangeableDirectory = json.Value<bool>("UseChangeableDirectory");
                }
                

                if (!json.ContainsKey("ChangeableDirectory"))
                {
                    this.ChangeableDirectory = "";
                }
                else
                {
                    this.ChangeableDirectory = json.Value<string>("ChangeableDirectory");
                }
                

                this.SaveName.Clear();
                if (json.ContainsKey("SaveName"))
                {
                    JArray array = json.Value<JArray>("SaveName");
                    for (int i = 0; i < array.Count; i++)
                    {
                        this.SaveName.Add(array.Value<string>(i));
                    }
                }

                if (json.ContainsKey("SaveNameIndex"))
                {
                    this.SaveNameIndex = json.Value<int>("SaveNameIndex");
                }

            }
            catch (Exception ex)
            {
                throw new JsonException(Texts.JSON_EXCEPTION_MESSAGE);
            }


        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json.Add("GameName", GameName);
            json.Add("Directory", Directory);
            json.Add("UseChangeableDirectory", UseChangeableDirectory);
            json.Add("ChangeableDirectory", ChangeableDirectory);
            json.Add("SaveNameIndex", SaveNameIndex);

            JArray array = new JArray();
            for (int i = 0; i < SaveName.Count; i++)
            {
                array.Add(SaveName[i]);
            }
            json.Add("SaveName", array);



            return json;
        }

        //我的文档
        private static readonly string DOCUMENTS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// 获取当前对象的存档目录
        /// </summary>

        /// <returns>存档所在目录</returns>
        /// version 1.1.0
        public string GetDirectory()
        {
            string directory = this.Directory;

            //将"?doc?"替换为我的文档
            directory = directory.Replace(@"?doc?", DOCUMENTS_PATH);

            //将两个%中间的内容替换为环境变量的路径。如%appdata%等
            Regex r = new Regex("%.*?%");
            MatchCollection mc = r.Matches(directory);
            foreach (Match m in mc)
            {
                string v = m.Value;
                directory = directory.Replace(v, Environment.GetEnvironmentVariable(v.Substring(1, v.Length - 2)));
            }

            //如果使用可变路径，将"??"替换为可变路径
            if (this.UseChangeableDirectory)
            {
                directory = directory.Replace("??", this.ChangeableDirectory);
            }
            return directory;
        }

        /// <summary>
        /// 获取当前对象的存档文件完整路径。
        /// 本函数处理路径中的环境变量、我的文档、可变路径等
        /// </summary>
        /// <returns>存档文件的完整路径</returns>
        /// version 1.1.0
        public string GetFileName()
        {
            string directory = GetDirectory();
            string filename = directory +  @"\" + this.SaveName[this.SaveNameIndex];
            return filename;
        }



    }
}
