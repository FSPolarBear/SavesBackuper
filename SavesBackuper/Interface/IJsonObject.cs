using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SavesBackuper
{
    /// <summary>
    /// 实现该接口的类可从json中解析出该类对象，也可将该类对象转化为json。
    /// </summary>
    public interface IJsonObject
    {
        /// <summary>
        /// 从json中解析该对象，并覆盖当前对象
        /// </summary>
        /// <param name="json">json</param>
        void FromJson(JObject json);

        /// <summary>
        /// 将当前对象转化为json
        /// </summary>
        /// <returns>json</returns>
        JObject ToJson();
    }
}
