using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ConfigCenter.Common;
using ConfigCenter.Dto;
using ConfigCenter.Repository;

namespace ConfigCenter.Business
{
    public class AppSettingBusiness
    {

        public static List<AppSettingDto> GetAppSettings(int appId)
        {
            return Mapper.Map<List<AppSetting>, List<AppSettingDto>>(AppSetting.Query("WHERE AppId=@0", appId).ToList());
        }

        public static List<AppSettingDto> GetAppSettings(int appId, int pageIndex, int pageSize, string kword, out long totalPage)
        {
            var page = AppSetting.Page(pageIndex, pageSize, "WHERE AppId=@0 AND ConfigKey LIKE @1",
                new object[] { appId, "%" + kword + "%" });
            totalPage = page.TotalItems;
            return Mapper.Map<List<AppSetting>, List<AppSettingDto>>(page.Items);
        }

        public static AppSettingDto GetAppSettingById(int id)
        {
            return Mapper.Map<AppSetting, AppSettingDto>(AppSetting.SingleOrDefault("WHERE Id=@0", id));
        }

        public static void SaveAppSetting(AppSettingDto appSettingDto)
        {
            var appSetting = Mapper.Map<AppSettingDto, AppSetting>(appSettingDto);
            appSetting.Save();

            var app = App.SingleOrDefault(appSettingDto.AppId);
            if (app != null)
            {
                app.Version = DateTime.Now.ToString("yyyyMMddHHmmss");
                app.Save();

                //更新zookeeper的值
                var path = ZooKeeperHelper.ZooKeeperRootNode + "/" + app.AppId;
                if (!ZooKeeperHelper.Exists(path))
                {
                    ZooKeeperHelper.Create(path, null);
                }
                ZooKeeperHelper.SetData(path, app.Version, -1);

            }
        }

        public static bool DeleteAppSettingById(int id)
        {
            return AppSetting.Delete(id) > 0;
        }
    }
}
