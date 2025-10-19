namespace FileStorage.Infrastructure;

public class Result
{
    public bool IsFailure { get; }
    public Dictionary<string, string[]> Errors { get; }

    protected Result(bool isFailure, Dictionary<string, string[]> errors)
    {
        IsFailure = isFailure;
        Errors = errors;
    }

    public static Dictionary<string, string[]> ToDict(string key, string[] value) =>
        new Dictionary<string, string[]>{{key, value}};

    public static Dictionary<string, string[]> ToDict(string key, string value) =>
        new Dictionary<string, string[]>{{key, [value]}};

    public static Result Success() => new(false, null!);
    public static Result Failure(Dictionary<string, string[]> errors) => new(true, errors);
}

public class Result<T> : Result
{
    public T Value { get; }

    private Result(T value, bool isFailure, Dictionary<string, string[]> errors)
        : base(isFailure, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(value, false, null!);
    public new static Result<T> Failure(Dictionary<string, string[]> errors) => new(default!, true, errors);
}