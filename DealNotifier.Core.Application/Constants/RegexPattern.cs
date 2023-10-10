namespace DealNotifier.Core.Application.Constants
{
    public static class RegExPattern
    {
        public static readonly string ModelNumber = "((?:sm-)?[a-z]\\d{3,4}[a-z]{0,3}\\d?)|(xt\\d{4}(-(\\d{1,2}|[a-z]))?)|((?:lg|lm)-?[a-z]{1,2}\\d{3}([a-z]{1,2})?\\d?(?:\\([a-z]\\))?)";
        public static readonly string ModelName = "((?<=samsung\\s+)(galaxy\\s+)?\\w+[+]?)|((?<=motorola\\s+)moto\\s+\\w+)|(lg\\s+\\w+( \\d)?)";
    }
}
