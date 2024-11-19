namespace LibraryAPI.Models;

public class PagedResult<T>
{
    public int TotalItems { get; set; } // Total items
    public IEnumerable<T> Items { get; set; } // Items on page
}
