//credit to neoRiley https://stackoverflow.com/questions/27021665/c-sharp-websocket-sending-message-back-to-client
//also the mozilla foundation documentation https://developer.mozilla.org/en-US/docs/Web/API/WebSockets_API/Writing_WebSocket_server
using System.Net.Sockets;
using System.Text;

namespace GameServer 
{
    class WSHelper
    {
        NetworkStream stream;

        public WSHelper(NetworkStream streamX)
        {
            stream = streamX;
        }
        public string Decoder(Byte[] buffer) {
            //decode websocket data
            bool fin = (buffer[0] & 0b10000000) != 0,
            mask = (buffer[1] & 0b10000000) != 0;

            int opcode = buffer[0] & 0b00001111, // expecting 1 - text message
            msglen = buffer[1] - 128, // & 0111 1111
            offset = 2;

            if (msglen == 126) {
                // was ToUInt16(bytes, offset) but the result is incorrect
                msglen = BitConverter.ToUInt16(new byte[] { buffer[3], buffer[2] }, 0);
                offset = 4;
            } 
            
            if (msglen == 0) {
                Console.WriteLine("msglen == 0");
            } else if (mask) {
                byte[] decoded = new byte[msglen];
                byte[] masks = new byte[4] { buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3] };
                offset += 4;
                for (int i = 0; i < msglen; ++i) {
                    decoded[i] = (byte)(buffer[offset + i] ^ masks[i % 4]);
                }
                return Encoding.UTF8.GetString(decoded);
            }
            return "";
        }

        public void SendMsg(string msg)
        {
            Queue<string> que = new Queue<string>(msg.SplitInGroups(125));
            int len = que.Count;

            while (que.Count > 0)
            {
                var header = GetHeader(
                    que.Count > 1 ? false : true,
                    que.Count == len ? false : true
                );

                byte[] list = Encoding.UTF8.GetBytes(que.Dequeue());
                header = (header << 7) + list.Length;
                stream?.Write(IntToByteArray((ushort)header), 0, 2);
                stream?.Write(list, 0, list.Length);
            }            
        }

        protected int GetHeader(bool finalFrame, bool contFrame)
        {
            int header = finalFrame ? 1 : 0;//fin: 0 = more frames, 1 = final frame
            header = (header << 1) + 0;//rsv1
            header = (header << 1) + 0;//rsv2
            header = (header << 1) + 0;//rsv3
            header = (header << 4) + (contFrame ? 0 : 1);//opcode : 0 = continuation frame, 1 = text
            header = (header << 1) + 0;//mask: server -> client = no mask

            return header;
        }
        protected byte[] IntToByteArray(ushort value)
        {
            var ary = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(ary);
            }

            return ary;
        }
    }

    /// ================= [ extension class ]==============>
    public static class XLExtensions
    {
        public static IEnumerable<string> SplitInGroups(this string original, int size)
        {
            var p = 0;
            var l = original.Length;
            while (l - p > size)
            {
                yield return original.Substring(p, size);
                p += size;
            }
            yield return original.Substring(p);
        }
    }
}