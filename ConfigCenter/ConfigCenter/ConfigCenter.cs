using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using ConfigCenter.Dto;
using ServiceStack;
using ZooKeeperNet;

namespace ConfigCenter
{
    public class ConfigCenter
    {
        private static string _currentVersion;

        private static Task _task;

        private static readonly JsonServiceClient Client = new JsonServiceClient("http://configcenter.xx.com/api");

        private static ZooKeeper zk;


        static ConfigCenter()
        {
            var appVersionResponse = Client.Get(new GetAppVersion { AppId = "ZookeeperConfig" });

            if (appVersionResponse.AppDto != null)
            {
                var appSettings = Client.Get(new GetAppSettings { AppId = appVersionResponse.AppDto.Id });
                if (appSettings != null)
                {
                    foreach (AppSettingDto appSetting in appSettings.AppSettings)
                    {
                        if (appSetting.ConfigKey == "ZookeeperAddress")
                        {
                            zk = new ZooKeeper(appSetting.ConfigValue, new TimeSpan(0, 0, 0, 50000), null);

                        }
                    }

                }
            }
        }


        public static void Init(string appId)
        {
            SyncVersion(appId);  //先同步获取一次，保证global之后执行的代码的都能获取到配置

            if (zk != null)   //如果zookeeper可用，则注册zookeeper
            {

                ZookeeperWatcherHelp.Register(zk, "/ConfigCenter/" + appId, (@event, resut) =>
                {
                    if (@event.Type == EventType.NodeDataChanged)
                    {
                        SyncVersion(appId);
                    }
                }, null);
            }

            _task = new Task(SyncVersion, appId, 20000, 10000);  //如果zookeeper不可用，同时为了保证在zookeeper挂掉后，有补偿措施，启动一个定时获取的任务。

        }

        public static void Stop()
        {
            zk.Dispose();
            _task.Stop();
        }

        private static void SyncVersion(string appid)
        {
            var appVersionResponse = Client.Get(new GetAppVersion { AppId = appid });
            if (appVersionResponse.AppDto != null)
            {
                if (_currentVersion != appVersionResponse.AppDto.Version) //客户端保存的版本号和服务端的版本号不一致，需要客户端去更新
                {
                    var appSettings = Client.Get(new GetAppSettings { AppId = appVersionResponse.AppDto.Id });
                    if (appSettings != null)
                    {
                        var syncSuccess = SyncLocalSetting(appSettings.AppSettings);

                        if (syncSuccess)
                        {
                            _currentVersion = appVersionResponse.AppDto.Version;
                        }
                    }
                }
            }
        }

        private static bool SyncLocalSetting(List<AppSettingDto> appSettingDtos)
        {
            try
            {
                var config =
                    WebConfigurationManager.OpenWebConfiguration("/");

                foreach (var appSettingDto in appSettingDtos)
                {
                    if (appSettingDto.ConfigType == 0) //普通配置节点
                    {
                        if (!ConfigurationManager.AppSettings.AllKeys.Contains(appSettingDto.ConfigKey))
                        {
                            config.AppSettings.Settings.Add(appSettingDto.ConfigKey, appSettingDto.ConfigValue);
                        }
                        else
                        {
                            if (ConfigurationManager.AppSettings[appSettingDto.ConfigKey] != appSettingDto.ConfigValue)
                            {
                                config.AppSettings.Settings[appSettingDto.ConfigKey].Value = appSettingDto.ConfigValue;
                            }
                        }
                    }
                    else //connection配置节点
                    {
                        if (ConfigurationManager.ConnectionStrings[appSettingDto.ConfigKey] == null)
                        {
                            config.ConnectionStrings.ConnectionStrings.Add(
                                new ConnectionStringSettings(appSettingDto.ConfigKey, appSettingDto.ConfigValue));
                        }
                        else
                        {
                            if (ConfigurationManager.ConnectionStrings[appSettingDto.ConfigKey].ConnectionString !=
                                appSettingDto.ConfigValue)
                                config.ConnectionStrings.ConnectionStrings[appSettingDto.ConfigKey].ConnectionString =
                                    appSettingDto.ConfigValue;
                        }
                    }
                }

                //删除setting节点
                var settingdiffs = GetDiff(ConfigurationManager.AppSettings.AllKeys.ToList(),
                    (from appSettingDto in appSettingDtos
                     where appSettingDto.ConfigType == 0
                     select appSettingDto.ConfigKey).ToList());

                foreach (var settingdiff in settingdiffs)
                {
                    config.AppSettings.Settings.Remove(settingdiff);
                }

                //删除 connectionString节点
                var connectionStringdiffs =
                    GetDiff(
                        (from ConnectionStringSettings connectionStringSetting in ConfigurationManager.ConnectionStrings
                         select connectionStringSetting.Name).ToList(),
                        (from appSettingDto in appSettingDtos
                         where appSettingDto.ConfigType == 1
                         select appSettingDto.ConfigKey).ToList());

                foreach (var connectionStringdiff in connectionStringdiffs)
                {
                    config.ConnectionStrings.ConnectionStrings.Remove(connectionStringdiff);
                }

                config.Save(ConfigurationSaveMode.Minimal);
                ConfigurationManager.RefreshSection("appSettings");
                ConfigurationManager.RefreshSection("connectionStrings");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static List<string> GetDiff(List<string> localKeys, List<string> remoteKeys)
        {
            return localKeys.Where(localKey => !remoteKeys.Contains(localKey)).ToList();
        }
    }
}