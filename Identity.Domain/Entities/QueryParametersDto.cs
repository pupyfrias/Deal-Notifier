namespace Identity.Domain.Entities
{
    public class QueryParametersDto
    {
        public bool Descending { get; set; }
        public int Limit { get; set; } = 10;
        public int Offset { get; set; }
        public string Search { get; set; }
        public IEnumerable<string> SortableProperties { get; set; }
    }
}