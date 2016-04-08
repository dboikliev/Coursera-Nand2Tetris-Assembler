using System.Text.RegularExpressions;

namespace HackAssembler
{
    class Symbol
    {
        public string Value { get; }

        public SymbolType Type { get; }
        public Symbol(string symbol)
        {
            Value = symbol;

            var match = Regex.Match(Value, @"R\d+");
            if (match.Success)
            {
                Type = SymbolType.Register;
            }
            else if (Regex.IsMatch(Value, @"\(.+\)"))
            {
                Type = SymbolType.Label;
            }
            else if (Regex.IsMatch(Value, @"@[a-zA-Z_\.]+"))
            {
                Type = SymbolType.Variable;
            }
            else
            {
                Type = SymbolType.Unknown;
            }

            Value = Value.Trim('@', '(', ')');
        }
    }
}
