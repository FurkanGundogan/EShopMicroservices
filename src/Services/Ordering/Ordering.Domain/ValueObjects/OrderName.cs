﻿namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int DefaultLength = 3;
    public string Value { get; }

    private OrderName(string value) => Value = value;

    public static OrderName Of(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        //ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);

        return new OrderName(value);
    }
}
