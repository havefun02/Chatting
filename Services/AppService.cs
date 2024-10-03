using App.Core;
using App.Exceptions;
using App.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace App.Services
{
    public class AppService:IAppService
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        public AppService(IMapper mapper, IServiceProvider serviceProvider) {
            _mapper = mapper;
            _serviceProvider = serviceProvider; 
        }

        public async Task<List<Message>> GetAllMessagesAsync() {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _appRepo = scope.ServiceProvider.GetRequiredService<IRepository<Message>>();
                var dbContext = _appRepo.GetDbSet();
                var res = await dbContext.Include(u => u.User).OrderBy(m => m.CreatedAt).ToListAsync();
                return res;
            }
        }
        public async Task<OffsetPageResult<Message>> GetOffsetMessageAsync(OffsetParams queryParams)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _paginateService = scope.ServiceProvider.GetRequiredService<IPaginationService<Message>>() as OffsetPaginationService<Message>;

                if (_paginateService == null)
                {
                    throw new ServiceUnavailableException("Can't resolve "+typeof(IPaginationService<>));
                }
                var _appRepo = scope.ServiceProvider.GetRequiredService<IRepository<Message>>();
                var dbContext = _appRepo.GetDbSet();
                var queryResult=dbContext.Include(m => m.User).OrderByDescending(m => m.CreatedAt).Select(m=>new Message
                {
                    MessageId = m.MessageId,
                    Content = m.Content,
                    CreatedAt =m.CreatedAt,
                    User =new User {UserName=m.User!.UserName },
                });
                var res=await _paginateService.Paginate(queryResult, queryParams) as OffsetPageResult<Message>;
                if (res == null)
                {
                    throw new ArgumentException("Error occur during try to paginate");
                }
                return res;
            }
        }

        public async Task<Message> AddMessage(MessageDto messageDto)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                if (messageDto?.Content!.Trim().Length == 0)
                {
                    throw new ArgumentNullException(nameof(messageDto.Content) + " can not be null");
                }
                var _appRepo = scope.ServiceProvider.GetRequiredService<IRepository<Message>>();
                var dbContext = _appRepo.GetDbSet();
                var message = _mapper.Map<Message>(messageDto);
                var res = await dbContext.AddAsync(message);
                await _appRepo.SaveChangesAsync();
                var messageResult=await dbContext.Include(u=>u.User).FirstOrDefaultAsync(m=>m.MessageId==message.MessageId);
                if (messageResult != null)
                {
                    return messageResult;
                }
                else {
                    throw new UnknownException("Error occur during query messages");
                }
            }
        }
    }
}
