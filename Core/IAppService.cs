using App.Models;

namespace App.Core
{
    public interface IAppService
    {
        public Task<List<Message>> GetAllMessagesAsync();
        public Task<Message> AddMessage(MessageDto messageDto);
        public Task<OffsetPageResult<Message>> GetOffsetMessageAsync(OffsetParams offsetParams);
    }
}
