﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace JWLibrary.Database
{
    internal class SSDBLinker
    {
        private MemoryStream recv_buf = new(8 * 1024);
        private TcpClient sock;

        public SSDBLinker(string host, int port)
        {
            sock = new TcpClient(host, port);
            sock.NoDelay = true;
            sock.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
        }

        ~SSDBLinker()
        {
            close();
        }

        public void close()
        {
            if (sock != null) sock.Close();
            sock = null;
        }

        public List<byte[]> request(string cmd, params string[] args)
        {
            var req = new List<byte[]>(1 + args.Length);
            req.Add(Encoding.Default.GetBytes(cmd));
            foreach (var s in args) req.Add(Encoding.Default.GetBytes(s));
            return request(req);
        }

        public List<byte[]> request(string cmd, params byte[][] args)
        {
            var req = new List<byte[]>(1 + args.Length);
            req.Add(Encoding.Default.GetBytes(cmd));
            req.AddRange(args);
            return request(req);
        }

        public List<byte[]> request(List<byte[]> req)
        {
            var buf = new MemoryStream();
            foreach (var p in req)
            {
                var len = Encoding.Default.GetBytes(p.Length.ToString());
                buf.Write(len, 0, len.Length);
                buf.WriteByte((byte) '\n');
                buf.Write(p, 0, p.Length);
                buf.WriteByte((byte) '\n');
            }

            buf.WriteByte((byte) '\n');

            var bs = buf.GetBuffer();
            sock.GetStream().Write(bs, 0, (int) buf.Length);
            //Console.Write(Encoding.Default.GetString(bs, 0, (int)buf.Length));
            return recv();
        }

        private List<byte[]> recv()
        {
            while (true)
            {
                var ret = parse();
                if (ret != null) return ret;
                var bs = new byte[8192];
                var len = sock.GetStream().Read(bs, 0, bs.Length);
                //Console.WriteLine("<< " + Encoding.Default.GetString(bs));
                recv_buf.Write(bs, 0, len);
            }
        }

        private static int memchr(byte[] bs, byte b, int offset)
        {
            for (var i = offset; i < bs.Length; i++)
                if (bs[i] == b)
                    return i;
            return -1;
        }

        private List<byte[]> parse()
        {
            var list = new List<byte[]>();
            var buf = recv_buf.GetBuffer();

            var idx = 0;
            while (true)
            {
                var pos = memchr(buf, (byte) '\n', idx);
                //System.out.println("pos: " + pos + " idx: " + idx);
                if (pos == -1) break;
                if (pos == idx || pos == idx + 1 && buf[idx] == '\r')
                {
                    idx += 1; // if '\r', next time will skip '\n'
                    // ignore empty leading lines
                    if (list.Count == 0)
                    {
                        continue;
                    }

                    var left = (int) recv_buf.Length - idx;
                    recv_buf = new MemoryStream(8192);
                    if (left > 0) recv_buf.Write(buf, idx, left);
                    return list;
                }

                var lens = new byte[pos - idx];
                Array.Copy(buf, idx, lens, 0, lens.Length);
                var len = int.Parse(Encoding.Default.GetString(lens));

                idx = pos + 1;
                if (idx + len >= recv_buf.Length) break;
                var data = new byte[len];
                Array.Copy(buf, idx, data, 0, data.Length);

                //Console.WriteLine("len: " + len + " data: " + Encoding.Default.GetString(data));
                idx += len + 1; // skip '\n'
                list.Add(data);
            }

            return null;
        }
    }
}