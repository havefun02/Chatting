namespace App.Core
{
    public class FilterCriterion
    {
        public required string PropertyName { get; set; }
        public object? Value { get; set; }
    }
    public class FilterParams
    {
        public List<FilterCriterion>? filterList { set; get; } = new List<FilterCriterion>();
    }
    public class RangeFilterCriterion : FilterCriterion
    {
        public object? StartValue { get; set; }
        public object? EndValue { get; set; }
    }

}
