using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_recognition.Model;

namespace Text_recognition
{
    public class Api
    {
        private String appid = "19897467";
        private String ApiKey = "RrFVeDjk3eYu3mLd5QnydCjd";
        private String secretKey = "wA3cSdE3iEu0XcodFSSRbRQzTduHN6nV";
        public Baidu.Aip.Ocr.Ocr ocr;
		public returnInfo rt = new returnInfo();
        public Api()
        {
            ocr = new Baidu.Aip.Ocr.Ocr(ApiKey, secretKey);
            ocr.Timeout = 6000;
        }
		public String GeneralBasicDemo(String file)
		{
			var image =File.ReadAllBytes(file);
			// 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
			//var result = ocr.GeneralBasic(image);
			//Console.WriteLine(result);
			// 如果有可选参数
			var options = new Dictionary<string, object>{
		        {"language_type", "CHN_ENG"},
				{"detect_direction", "true"},
				{"detect_language", "true"},
				{"probability", "true"}
			};
			// 带参数调用通用文字识别, 图片参数为本地图片
			var result = ocr.GeneralBasic(image, options);
			Console.WriteLine(result);
			String end = null;
			rt.log_id = result["log_id"].ToString();
			rt.direction = result["direction"].ToString();
			rt.words_result_num =Int32.Parse(result["words_result_num"].ToString());
			Newtonsoft.Json.Linq.JArray jArray = new Newtonsoft.Json.Linq.JArray();
			jArray = result.Value<JArray>("words_result");
			JObject jObject;
			for (int i = 0; i < jArray.Count;i++)
			{
				jObject = JObject.Parse(jArray[i].ToString());
				rt.words = rt.words +  jObject["words"].ToString() + "\r\n";
			}
			return rt.words;
		}
	}
}
