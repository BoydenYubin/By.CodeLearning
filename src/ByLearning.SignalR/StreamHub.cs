using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using System.Threading.Channels;

namespace ByLerning.SignalR
{
    public class StreamHub : Hub
    {
        public ChannelReader<byte[]> DownloadFileTest()
        {
            var channel = Channel.CreateUnbounded<byte[]>(new UnboundedChannelOptions() { });
            Exception localException = null;
            try
            {
                //自行添加test.zip进行测试
                var stream = new FileStream("test.zip", FileMode.Open);
                int bufferSize = 4096;
                var buffer = new byte[bufferSize];
                //为防止多写入buffer数据，需要重点考虑流的尾巴tail处的数据
                //否则拷贝数据文件大小不一致(多写入0000)
                bufferSize = stream.Read(buffer, 0, bufferSize);
                while (bufferSize > 0)
                {
                    if (bufferSize < buffer.Length)
                    {
                        byte[] tail = new byte[bufferSize];
                        Array.Copy(buffer, tail, bufferSize);
                        channel.Writer.WriteAsync(tail);
                    }
                    else
                    {
                        channel.Writer.WriteAsync(buffer);
                    }
                    bufferSize = stream.Read(buffer, 0, bufferSize);
                }
                stream.Close();
            }
            catch (Exception e)
            {
                localException = e;
            }
            finally
            {
                channel.Writer.Complete(localException);
            }
            return channel.Reader;
        }
    }
}
