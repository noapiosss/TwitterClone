using System;

using TwitterClone.Domain.Helpers.Interfaces;

namespace TwitterClone.Domain.Helpers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}