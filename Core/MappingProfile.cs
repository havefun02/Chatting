using App.Models;
using AutoMapper;
namespace App.Core
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<MessageDto, Message>();
            CreateMap<Message, MessageResultDto>().ForMember(dest => dest.UserName,opt=>opt.MapFrom(src=>src.User!.UserName));

        }
    }
}
