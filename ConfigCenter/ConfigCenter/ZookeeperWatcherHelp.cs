using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace ConfigCenter
{
    public class ZookeeperWatcherHelp
    {
        public static void Register(ZooKeeper zk, string path, Action<WatchedEvent, byte[]> nodeChangecallback,
            Action<WatchedEvent, string[]> nodeChildrenChangecallback)
        {
            new ZookeeperWatcherWrapper(zk, path, nodeChangecallback,
                nodeChildrenChangecallback);

        }

        public class ZookeeperWatcherWrapper : IWatcher
        {
            private readonly ZooKeeper _zk;
            private readonly Action<WatchedEvent, byte[]> _nodeChangecallback;
            private readonly Action<WatchedEvent, string[]> _nodeChildrenChangecallback;
            private readonly string _path;

            public ZookeeperWatcherWrapper(ZooKeeper zk, string path, Action<WatchedEvent, byte[]> nodeChangecallback, Action<WatchedEvent, string[]> nodeChildrenChangecallback)
            {
                _path = path;
                _nodeChangecallback = nodeChangecallback;
                _nodeChildrenChangecallback = nodeChildrenChangecallback;
                _zk = zk;

                if (zk.Exists(path, false) == null)
                {
                    zk.Exists(path, this);
                }
                else
                {
                    zk.GetData(path, this, null);
                    zk.GetChildren(path, this, null);
                }
            }
            public void Process(WatchedEvent @event)
            {
                try
                {
                    if (@event.Type == EventType.None) return;

                    if (@event.Type == EventType.NodeCreated)
                    {
                        _zk.GetChildren(@event.Path, this, null);
                        var nodeData = _zk.GetData(@event.Path, this, null);

                        if (_nodeChangecallback != null)
                            _nodeChangecallback(@event, nodeData);
                    }
                    else if (@event.Type == EventType.NodeDeleted)
                    {
                        _zk.Exists(@event.Path, this);

                        if (_nodeChangecallback != null)
                            _nodeChangecallback(@event, null);
                    }
                    else if (@event.Type == EventType.NodeChildrenChanged)
                    {
                        var chlidrenNode = _zk.GetChildren(@event.Path, this, null);
                        if (_nodeChildrenChangecallback != null)
                            _nodeChildrenChangecallback(@event, chlidrenNode.ToArray());
                    }
                    else
                    {
                        var nodeData = _zk.GetData(@event.Path, this, null);
                        if (_nodeChangecallback != null)
                            _nodeChangecallback(@event, nodeData);
                    }
                }
                catch (KeeperException.NoNodeException e)
                {
                    _zk.Exists(@event.Path, this);
                    Console.WriteLine("ERROR");
                }
            }

        }
    }
}
