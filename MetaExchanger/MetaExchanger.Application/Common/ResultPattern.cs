﻿namespace MetaExchanger.Application.Common
{
    /// <summary>
    /// Implementation of Error in Result Pattern.
    /// </summary>
    public class Error
    {
        public string Message { get; }

        public Error(string message) => Message = message;

        public override string ToString() => Message;
    }

    /// <summary>
    /// Returs empty result if Ok or error.
    /// </summary>
    public class Result
    {
        private readonly Error? _error;

        public bool Ok => _error is null;

        public Error? Error => _error;

        private Result(Error error) => _error = error;

        public static readonly Result Empty = new();

        public static implicit operator Result(Error error) => new(error);

        private Result() { }
    }

    /// <summary>
    /// Returs value or error.
    /// </summary>
    public class Result<T>
    {
        private readonly Error? _error;

        public T? Value { get; }

        public bool Ok => _error is null;

        public Error? Error => _error;

        private Result(T value) => Value = value;

        private Result(Error error) => _error = error;

        public static implicit operator Result<T>(T value) => new(value);

        public static implicit operator Result<T>(Error error) => new(error);
    }
}