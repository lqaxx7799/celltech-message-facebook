namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookPagedResponse<T>
{
    public List<T> Data { get; set; } = default!;

    public FacebookPaging? Paging { get; set; }
}

public class FacebookPaging
{
    public FacebookCursor Cursors { get; set; } = default!;
}

public class FacebookCursor
{
    public string? Before { get; set; }
    public string? After { get; set; }
}