using System;
using System.Collections.Generic;
using System.Linq;

namespace HackAssembler
{
    class CCommand : Command
    {
        public CCommand(string command) : base(CommandType.C)
        {
            var components = command.Split(new[] {' ', '=', ';'}, StringSplitOptions.RemoveEmptyEntries);
            var comps = new List<CommandComponent>();
            if (components.Length == 2 && command.Contains("="))
            {
                comps.Add(new CommandComponent { Type = ComponentType.Computation, Value = components[1] });
                comps.Add(new CommandComponent { Type = ComponentType.Destination, Value = components[0] });
                comps.Add(new CommandComponent { Type = ComponentType.Jump, Value = string.Empty });
            }
            else if (components.Length == 2)
            {
                comps.Add(new CommandComponent { Type = ComponentType.Computation, Value = components[0] });
                comps.Add(new CommandComponent { Type = ComponentType.Destination, Value = string.Empty });
                comps.Add(new CommandComponent { Type = ComponentType.Jump, Value = components[1]});
            }
            else if (components.Length == 3)
            {
                comps.Add(new CommandComponent { Type = ComponentType.Computation, Value = components[1] });
                comps.Add(new CommandComponent { Type = ComponentType.Destination, Value = components[0] });
                comps.Add(new CommandComponent { Type = ComponentType.Jump, Value = components[2] });
            }
            Components = comps.ToArray();
        }

        public override string ToBinary()
        {
            var result = "111";
            var command = Components.Aggregate(string.Empty, (current, component) => current + SymbolTable.GetCode(component.Type, component.Value));
            result += command.PadLeft(13, '0');
            return result;
        }
    }
}
