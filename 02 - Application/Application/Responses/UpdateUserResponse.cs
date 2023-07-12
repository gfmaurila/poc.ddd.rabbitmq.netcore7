namespace Application.Responses;

public class UpdateUserResponse : BaseResponse
{
    public UpdateUserResponse(int id) => Id = id;

    public int Id { get; }
}