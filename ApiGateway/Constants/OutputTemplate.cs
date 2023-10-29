namespace ApiGateway.Constants
{
    public static class OutputTemplate
    {
        /*
        @t = Timestamp
        @l = Level
        @m = Message
        @x = Exception
        */

        public const string Console = "[{@t:HH:mm:ss} {@l:u3}] {Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1),-11} {@m}\n{@x}";
        public const string File = "[{@t:yyyy/MM/dd HH:mm:ss.fff zzz} {@l:u3}] {Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1),-11} {@m}\n{@x}";
    }
}