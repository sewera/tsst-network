using System;

namespace nn.src.Ui.Parsers.Exceptions
{
    public class ParserException : Exception
    {
        public string ExceptionMessage { get; }

        public ParserException(string message) : base($"Parse exception: {message}")
        {
            ExceptionMessage = message;
        }
    }
}
