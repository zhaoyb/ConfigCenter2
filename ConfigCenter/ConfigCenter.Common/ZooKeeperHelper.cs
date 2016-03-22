using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace ConfigCenter.Common
{
    public class ZooKeeperHelper
    {
        private static readonly string ZookeeperAddress = ConfigurationManager.AppSettings["ZookeeperAddress"];
        private static readonly ZooKeeper Zk = new ZooKeeper(ZookeeperAddress, new TimeSpan(0, 0, 0, 50000), null);

        public static string ZooKeeperRootNode
        {
            get { return ConfigurationManager.AppSettings["ZookeeperRootNode"]; }
        }

        public static void SetData(string path, string data, int version)
        {
            var nodeData = data == null ? null : data.GetBytes();
            Zk.SetData(path, nodeData, version);
        }

        public static byte[] GetData(string path)
        {
            var data = Zk.GetData(path, false, null);
            return data;
        }

        public static void Create(string path, string data)
        {
            var nodeData = data == null ? null : data.GetBytes();
            Zk.Create(path, nodeData, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
        }

        public static void Delete(string path, int version)
        {
            Zk.Delete(path, version);
        }

        public static List<string> GetChildren(string path)
        {
            var nodepath = Zk.GetChildren(path, false).ToList();
            return nodepath;
        }

        public static bool Exists(string path)
        {
            var exists = Zk.Exists(path, false) != null;
            return exists;
        }
    }
}