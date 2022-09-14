using System;

namespace TwitterClone.Domain.Helpers.Interfaces;

public class IDateTimeProvider
{
    DateTime Now { get; }
}