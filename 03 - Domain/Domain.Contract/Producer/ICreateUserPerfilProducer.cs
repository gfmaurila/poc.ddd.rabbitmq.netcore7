using Application.DTOs;

namespace Domain.Contract.Producer;

public interface ICreateUserPerfilProducer
{
    void Publish(UserListDto dto);
}
