using System;

namespace CityWork.Infrastructure
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        Guid? UserId { get; }
    }
}
