using Ardalis.Result;

namespace ApplicationTest.Fixture;
public class ResultWithStatusCode<T> : Result<T>
{
    public int StatusCode { get; set; }
}

