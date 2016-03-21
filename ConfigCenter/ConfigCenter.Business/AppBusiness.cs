using AutoMapper;
using ConfigCenter.Dto;
using ConfigCenter.Repository;
using System.Collections.Generic;

namespace ConfigCenter.Business
{
    public class AppBusiness
    {
        public static List<AppDto> GetApps(int pageIndex, int pageSize, out long totalPage)
        {
            var page = App.Page(pageIndex, pageSize, "", new object[] { });
            totalPage = page.TotalItems;
            return Mapper.Map<List<App>, List<AppDto>>(page.Items);
        }

        public static AppDto GetAppById(int Id)
        {
            return Mapper.Map<App, AppDto>(App.SingleOrDefault("WHERE id=@0", Id));
        }

        public static AppDto GetAppVersion(string appId)
        {
            return Mapper.Map<App, AppDto>(App.SingleOrDefault("WHERE AppId=@0", appId));
        }


        public static void SaveApp(AppDto appDto)
        {
            var app = Mapper.Map<AppDto, App>(appDto);
            app.Save();
        }

        public static bool DeleteAppById(int id)
        {
            return App.Delete(id) > 0;
        }
    }
}