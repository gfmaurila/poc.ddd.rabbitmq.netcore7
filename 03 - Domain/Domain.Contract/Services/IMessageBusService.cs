﻿namespace Domain.Contract.Services;

public interface IMessageBusService
{
    void Publish(string queue, byte[] message);
}
