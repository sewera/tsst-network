using System;

namespace cn.Ui.Parsers.Exceptions
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base($"Parse exception: {message}") {}
    }
}
