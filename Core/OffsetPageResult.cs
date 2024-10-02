

namespace App.Core
{
    public class OffsetPageResult<T> : IPageResult<T>
    {
        public int totalItems { set; get; }
        public List<T>? Items { set; get; }
        public int limit { set; get; }
        public int offset { set; get; } 
    }
}
