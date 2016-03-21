using AutoMapper;
using ConfigCenter.Dto;
using ConfigCenter.Repository;

namespace ConfigCenter.Common
{
    public class ObjectMapping
    {
        public static void Init()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<ConfigCenterObjectMappingProfile>(); });
        }
    }

    public class ConfigCenterObjectMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<App, AppDto>();
            CreateMap<AppDto, App>();

            CreateMap<AppSetting, AppSettingDto>();
            CreateMap<AppSettingDto, AppSetting>();
        }

        //配置的名称，默认可以定义为当前的类名
        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }
    }
}