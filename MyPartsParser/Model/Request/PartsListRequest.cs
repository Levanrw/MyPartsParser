namespace MyPartsParser.Model.Request
{
    public class PartsListRequest
    {
        public PartsListRequest(int Page, int Pr_type_id, int Limit)
        {
            pr_type_id = Pr_type_id;
            page = Page;
            limit = Limit;
        }
        public int page { get; set; }
        public int pr_type_id { get; set; }
        public int limit { get; set; }
    }
}
