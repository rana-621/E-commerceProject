namespace Ecom.Core.Sharing;

public class ProductParams
{
    //string sort, int? categoryId, int pageSize, int pageNumber
    public string Sort { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public int MaxPageSize { get; set; } = 6;
    private int _pageSize;

    public int pageSize
    {
        get { return _pageSize; }
        set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
    }

    public int PageNumber { get; set; } = 1;
}
