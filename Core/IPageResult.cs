namespace App.Core
{
    public interface IPageResult<T>
    {
        public int totalItems { get; set; }
        public List<T>? Items { get; set; }
        public int limit { get; set; }
    }
}
