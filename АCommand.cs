using System;

namespace HackAssembler
{
    class АCommand : Command
    {
        public АCommand(string command) : base(CommandType.A)
        {
            Components = new[] {new CommandComponent() { Type = ComponentType.Address, Value = command.Substring(1)}};
        }

        public override string ToBinary()
        {
            int address;
            var binary = int.TryParse(Components[0].Value, out address) ? Convert.ToString(address, 2) : SymbolTable.GetCode(Components[0].Type, Components[0].Value);
            var aCommandInBinary = "0" + binary.PadLeft(15, '0');
            return aCommandInBinary;
        }
    }
}