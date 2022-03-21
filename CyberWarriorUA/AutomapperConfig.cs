using System;
using AutoMapper;
using CyberWarriorUA.Models;

namespace CyberWarriorUA
{
    public static class AutomapperConfig
    {
        public static MapperConfiguration CreateMapperConfig()
        {
            var config = new MapperConfiguration((t) =>
            {
                t.AllowNullCollections = true;
                t.AllowNullDestinationValues = true;
                t.AddProfile(typeof(DefaultProfile));
            });

            return config;
        }

        public class DefaultProfile : Profile
        {
            public DefaultProfile()
            {
                CreateMap<AttackModel, AttackInfo>()
                    .ReverseMap();
            }
        }
    }
}
