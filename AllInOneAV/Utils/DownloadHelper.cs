using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DownloadHelper
    {
        public static string DownloadFile(string url, string path)
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Proxy = null;
                ServicePointManager.DefaultConnectionLimit = 5;
                ServicePointManager.Expect100Continue = false;
                //发送请求并获取相应回应数据
                //request.KeepAlive = false;
                //request.Timeout = 10000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();

                //创建本地文件写入流
                Stream stream = new FileStream(path, FileMode.Create);

                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                var exception = "Download file " + url + " failed" + e.ToString();
                Console.WriteLine(exception);
                return exception;
            }

            return "";
        }
    }
}
