namespace Application.Responses;

public class CreatedUserResponse : BaseResponse
{
    public CreatedUserResponse(int id) => Id = id;

    public int Id { get; }
}