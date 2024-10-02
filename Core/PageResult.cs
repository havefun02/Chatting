using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Core
{
    public class PageResult<T>:IPageResult<T>
    {
        public int totalItems {  get; set; }
        public List<T>? Items { get; set; }
        public int limit { get; set; }
        public int offset { get;  set; }
    }
}
