using Application.DTOs;

namespace Domain.Contract.Producer;

public interface ICreateUserProducer
{
    void Publish(UserListDto dto);
}
