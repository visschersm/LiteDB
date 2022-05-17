using Xunit;
using FluentAssertions;
using System;

namespace LiteDB.Tests
{
    public class LiteExceptionTests
    {
        [Fact]
        public void MessageWithoutArguments_ShouldNotThrow()
        {
            Action act = () => new InternalException("some test message without arguments", 1, 2);
            act.Should().NotThrow();
        }

        [Fact]
        public void MessageWithArguments_NoneProvided_ShouldThrow()
        {
            Action act = () => new InternalException("foo {0}");
            act.Should().Throw<FormatException>();
        }

        public class InternalException : LiteException
        {
            public InternalException(string message, params object[] args)
                : base(404, message, args)
                {

                }
        }
    }
}