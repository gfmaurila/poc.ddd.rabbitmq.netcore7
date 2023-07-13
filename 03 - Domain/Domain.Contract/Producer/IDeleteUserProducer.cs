using Application.DTOs;

namespace Domain.Contract.Producer;

public interface IDeleteUserProducer
{
    void Publish(UserListDto dto);
}
