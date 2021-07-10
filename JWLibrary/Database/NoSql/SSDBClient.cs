using System;
using System.Collections.Generic;
using System.Text;
using eXtensionSharp;

namespace JWLibrary.Database
{
    public class SSDBClient : IDisposable
    {
        private string _resp_code;
        private readonly SSDBLinker _ssdbLinker;

        public SSDBClient(string host, int port)
        {
            _ssdbLinker = new SSDBLinker(host, port);
        }

        public void Dispose()
        {
            if (_ssdbLinker.xIsNotNull()) _ssdbLinker.close();
        }

        /// <summary>
        ///     di-ctor
        /// </summary>
        ~SSDBClient()
        {
        }

        public List<byte[]> request(string cmd, params string[] args)
        {
            return _ssdbLinker.request(cmd, args);
        }

        public List<byte[]> request(string cmd, params byte[][] args)
        {
            return _ssdbLinker.request(cmd, args);
        }

        public List<byte[]> request(List<byte[]> req)
        {
            return _ssdbLinker.request(req);
        }


        private void assert_ok()
        {
            if (_resp_code != "ok") throw new Exception(_resp_code);
        }

        private byte[] _bytes(string s)
        {
            return Encoding.Default.GetBytes(s);
        }

        private string _string(byte[] bs)
        {
            return Encoding.Default.GetString(bs);
        }

        private KeyValuePair<string, byte[]>[] parse_scan_resp(List<byte[]> resp)
        {
            _resp_code = _string(resp[0]);
            assert_ok();

            var size = (resp.Count - 1) / 2;
            var kvs = new KeyValuePair<string, byte[]>[size];
            for (var i = 0; i < size; i += 1)
            {
                var key = _string(resp[i * 2 + 1]);
                var val = resp[i * 2 + 2];
                kvs[i] = new KeyValuePair<string, byte[]>(key, val);
            }

            return kvs;
        }

        /***** kv *****/

        public bool exists(byte[] key)
        {
            var resp = request("exists", key);
            _resp_code = _string(resp[0]);
            if (_resp_code == "not_found") return false;
            assert_ok();
            if (resp.Count != 2) throw new Exception("Bad response!");
            return _string(resp[1]) == "1" ? true : false;
        }

        public bool exists(string key)
        {
            return exists(_bytes(key));
        }

        public void set(byte[] key, byte[] val)
        {
            var resp = request("set", key, val);
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void set(string key, string val)
        {
            set(_bytes(key), _bytes(val));
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns>returns true if name.key is found, otherwise returns false.</returns>
        public bool get(byte[] key, out byte[] val)
        {
            val = null;
            var resp = request("get", key);
            _resp_code = _string(resp[0]);
            if (_resp_code == "not_found") return false;
            assert_ok();
            if (resp.Count != 2) throw new Exception("Bad response!");
            val = resp[1];
            return true;
        }

        public bool get(string key, out byte[] val)
        {
            return get(_bytes(key), out val);
        }

        public bool get(string key, out string val)
        {
            val = null;
            byte[] bs;
            if (!get(key, out bs)) return false;
            val = _string(bs);
            return true;
        }

        public void del(byte[] key)
        {
            var resp = request("del", key);
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void del(string key)
        {
            del(_bytes(key));
        }

        public KeyValuePair<string, byte[]>[] scan(string key_start, string key_end, long limit)
        {
            var resp = request("scan", key_start, key_end, limit.ToString());
            return parse_scan_resp(resp);
        }

        public KeyValuePair<string, byte[]>[] rscan(string key_start, string key_end, long limit)
        {
            var resp = request("rscan", key_start, key_end, limit.ToString());
            return parse_scan_resp(resp);
        }

        /***** hash *****/

        public void hset(byte[] name, byte[] key, byte[] val)
        {
            var resp = request("hset", name, key, val);
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void hset(string name, string key, byte[] val)
        {
            hset(_bytes(name), _bytes(key), val);
        }

        public void hset(string name, string key, string val)
        {
            hset(_bytes(name), _bytes(key), _bytes(val));
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns>returns true if name.key is found, otherwise returns false.</returns>
        public bool hget(byte[] name, byte[] key, out byte[] val)
        {
            val = null;
            var resp = request("hget", name, key);
            _resp_code = _string(resp[0]);
            if (_resp_code == "not_found") return false;
            assert_ok();
            if (resp.Count != 2) throw new Exception("Bad response!");
            val = resp[1];
            return true;
        }

        public bool hget(string name, string key, out byte[] val)
        {
            return hget(_bytes(name), _bytes(key), out val);
        }

        public bool hget(string name, string key, out string val)
        {
            val = null;
            byte[] bs;
            if (!hget(name, key, out bs)) return false;
            val = _string(bs);
            return true;
        }

        public void hdel(byte[] name, byte[] key)
        {
            var resp = request("hdel", name, key);
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void hdel(string name, string key)
        {
            hdel(_bytes(name), _bytes(key));
        }

        public bool hexists(byte[] name, byte[] key)
        {
            var resp = request("hexists", name, key);
            _resp_code = _string(resp[0]);
            if (_resp_code == "not_found") return false;
            assert_ok();
            if (resp.Count != 2) throw new Exception("Bad response!");
            return _string(resp[1]) == "1" ? true : false;
        }

        public bool hexists(string name, string key)
        {
            return hexists(_bytes(name), _bytes(key));
        }

        public long hsize(byte[] name)
        {
            var resp = request("hsize", name);
            _resp_code = _string(resp[0]);
            assert_ok();
            if (resp.Count != 2) throw new Exception("Bad response!");
            return long.Parse(_string(resp[1]));
        }

        public long hsize(string name)
        {
            return hsize(_bytes(name));
        }

        public KeyValuePair<string, byte[]>[] hscan(string name, string key_start, string key_end, long limit)
        {
            var resp = request("hscan", name, key_start, key_end, limit.ToString());
            return parse_scan_resp(resp);
        }

        public KeyValuePair<string, byte[]>[] hrscan(string name, string key_start, string key_end, long limit)
        {
            var resp = request("hrscan", name, key_start, key_end, limit.ToString());
            return parse_scan_resp(resp);
        }

        public void multi_hset(byte[] name, KeyValuePair<byte[], byte[]>[] kvs)
        {
            var req = new byte[kvs.Length * 2 + 1][];
            req[0] = name;
            for (var i = 0; i < kvs.Length; i++)
            {
                req[2 * i + 1] = kvs[i].Key;
                req[2 * i + 2] = kvs[i].Value;
            }

            var resp = request("multi_hset", req);
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void multi_hset(string name, KeyValuePair<string, string>[] kvs)
        {
            var req = new KeyValuePair<byte[], byte[]>[kvs.Length];
            for (var i = 0; i < kvs.Length; i++)
                req[i] = new KeyValuePair<byte[], byte[]>(_bytes(kvs[i].Key), _bytes(kvs[i].Value));
            multi_hset(_bytes(name), req);
        }

        public void multi_hdel(byte[] name, byte[][] keys)
        {
            var req = new byte[keys.Length + 1][];
            req[0] = name;
            for (var i = 0; i < keys.Length; i++) req[i + 1] = keys[i];
            var resp = request("multi_hdel", req);
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void multi_hdel(string name, string[] keys)
        {
            var req = new byte[keys.Length][];
            for (var i = 0; i < keys.Length; i++) req[i] = _bytes(keys[i]);
            multi_hdel(_bytes(name), req);
        }

        public KeyValuePair<string, byte[]>[] multi_hget(byte[] name, byte[][] keys)
        {
            var req = new byte[keys.Length + 1][];
            req[0] = name;
            for (var i = 0; i < keys.Length; i++) req[i + 1] = keys[i];
            var resp = request("multi_hget", req);
            var ret = parse_scan_resp(resp);

            return ret;
        }

        public KeyValuePair<string, byte[]>[] multi_hget(string name, string[] keys)
        {
            var req = new byte[keys.Length][];
            for (var i = 0; i < keys.Length; i++) req[i] = _bytes(keys[i]);
            return multi_hget(_bytes(name), req);
        }

        /***** zset *****/

        public void zset(byte[] name, byte[] key, long score)
        {
            var resp = request("zset", name, key, _bytes(score.ToString()));
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void zset(string name, string key, long score)
        {
            zset(_bytes(name), _bytes(key), score);
        }

        public long zincr(byte[] name, byte[] key, long increment)
        {
            var resp = request("zincr", name, key, _bytes(increment.ToString()));
            _resp_code = _string(resp[0]);
            assert_ok();
            if (resp.Count != 2) throw new Exception("Bad response!");
            return long.Parse(_string(resp[1]));
        }

        public long zincr(string name, string key, long increment)
        {
            return zincr(_bytes(name), _bytes(key), increment);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <param name="score"></param>
        /// <returns>returns true if name.key is found, otherwise returns false.</returns>
        public bool zget(byte[] name, byte[] key, out long score)
        {
            score = -1;
            var resp = request("zget", name, key);
            _resp_code = _string(resp[0]);
            if (_resp_code == "not_found") return false;
            assert_ok();
            if (resp.Count != 2) throw new Exception("Bad response!");
            score = long.Parse(_string(resp[1]));
            return true;
        }

        public bool zget(string name, string key, out long score)
        {
            return zget(_bytes(name), _bytes(key), out score);
        }

        public void zdel(byte[] name, byte[] key)
        {
            var resp = request("zdel", name, key);
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void zdel(string name, string key)
        {
            zdel(_bytes(name), _bytes(key));
        }

        public long zsize(byte[] name)
        {
            var resp = request("zsize", name);
            _resp_code = _string(resp[0]);
            assert_ok();
            if (resp.Count != 2) throw new Exception("Bad response!");
            return long.Parse(_string(resp[1]));
        }

        public long zsize(string name)
        {
            return zsize(_bytes(name));
        }

        public bool zexists(byte[] name, byte[] key)
        {
            var resp = request("zexists", name, key);
            _resp_code = _string(resp[0]);
            if (_resp_code == "not_found") return false;
            assert_ok();
            if (resp.Count != 2) throw new Exception("Bad response!");
            return _string(resp[1]) == "1" ? true : false;
        }

        public bool zexists(string name, string key)
        {
            return zexists(_bytes(name), _bytes(key));
        }

        public KeyValuePair<string, long>[] zrange(string name, int offset, int limit)
        {
            var resp = request("zrange", name, offset.ToString(), limit.ToString());
            var kvs = parse_scan_resp(resp);
            var ret = new KeyValuePair<string, long>[kvs.Length];
            for (var i = 0; i < kvs.Length; i++)
            {
                var key = kvs[i].Key;
                var score = long.Parse(_string(kvs[i].Value));
                ret[i] = new KeyValuePair<string, long>(key, score);
            }

            return ret;
        }

        public KeyValuePair<string, long>[] zrrange(string name, int offset, int limit)
        {
            var resp = request("zrrange", name, offset.ToString(), limit.ToString());
            var kvs = parse_scan_resp(resp);
            var ret = new KeyValuePair<string, long>[kvs.Length];
            for (var i = 0; i < kvs.Length; i++)
            {
                var key = kvs[i].Key;
                var score = long.Parse(_string(kvs[i].Value));
                ret[i] = new KeyValuePair<string, long>(key, score);
            }

            return ret;
        }

        public KeyValuePair<string, long>[] zscan(string name, string key_start, long score_start, long score_end,
            long limit)
        {
            var score_s = "";
            var score_e = "";
            if (score_start != long.MinValue) score_s = score_start.ToString();
            if (score_end != long.MaxValue) score_e = score_end.ToString();
            var resp = request("zscan", name, key_start, score_s, score_e, limit.ToString());
            var kvs = parse_scan_resp(resp);
            var ret = new KeyValuePair<string, long>[kvs.Length];
            for (var i = 0; i < kvs.Length; i++)
            {
                var key = kvs[i].Key;
                var score = long.Parse(_string(kvs[i].Value));
                ret[i] = new KeyValuePair<string, long>(key, score);
            }

            return ret;
        }

        public KeyValuePair<string, long>[] zrscan(string name, string key_start, long score_start, long score_end,
            long limit)
        {
            var score_s = "";
            var score_e = "";
            if (score_start != long.MaxValue) score_s = score_start.ToString();
            if (score_end != long.MinValue) score_e = score_end.ToString();
            var resp = request("zrscan", name, key_start, score_s, score_e, limit.ToString());
            var kvs = parse_scan_resp(resp);
            var ret = new KeyValuePair<string, long>[kvs.Length];
            for (var i = 0; i < kvs.Length; i++)
            {
                var key = kvs[i].Key;
                var score = long.Parse(_string(kvs[i].Value));
                ret[i] = new KeyValuePair<string, long>(key, score);
            }

            return ret;
        }

        public void multi_zset(byte[] name, KeyValuePair<byte[], long>[] kvs)
        {
            var req = new byte[kvs.Length * 2 + 1][];
            req[0] = name;
            for (var i = 0; i < kvs.Length; i++)
            {
                req[2 * i + 1] = kvs[i].Key;
                req[2 * i + 2] = _bytes(kvs[i].Value.ToString());
            }

            var resp = request("multi_zset", req);
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void multi_zset(string name, KeyValuePair<string, long>[] kvs)
        {
            var req = new KeyValuePair<byte[], long>[kvs.Length];
            for (var i = 0; i < kvs.Length; i++)
                req[i] = new KeyValuePair<byte[], long>(_bytes(kvs[i].Key), kvs[i].Value);
            multi_zset(_bytes(name), req);
        }

        public void multi_zdel(byte[] name, byte[][] keys)
        {
            var req = new byte[keys.Length + 1][];
            req[0] = name;
            for (var i = 0; i < keys.Length; i++) req[i + 1] = keys[i];
            var resp = request("multi_zdel", req);
            _resp_code = _string(resp[0]);
            assert_ok();
        }

        public void multi_zdel(string name, string[] keys)
        {
            var req = new byte[keys.Length][];
            for (var i = 0; i < keys.Length; i++) req[i] = _bytes(keys[i]);
            multi_zdel(_bytes(name), req);
        }

        public KeyValuePair<string, long>[] multi_zget(byte[] name, byte[][] keys)
        {
            var req = new byte[keys.Length + 1][];
            req[0] = name;
            for (var i = 0; i < keys.Length; i++) req[i + 1] = keys[i];
            var resp = request("multi_zget", req);
            var kvs = parse_scan_resp(resp);
            var ret = new KeyValuePair<string, long>[kvs.Length];
            for (var i = 0; i < kvs.Length; i++)
            {
                var key = kvs[i].Key;
                var score = long.Parse(_string(kvs[i].Value));
                ret[i] = new KeyValuePair<string, long>(key, score);
            }

            return ret;
        }

        public KeyValuePair<string, long>[] multi_zget(string name, string[] keys)
        {
            var req = new byte[keys.Length][];
            for (var i = 0; i < keys.Length; i++) req[i] = _bytes(keys[i]);
            return multi_zget(_bytes(name), req);
        }
    }
}