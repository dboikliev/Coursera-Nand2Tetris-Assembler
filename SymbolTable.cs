﻿using System;
using System.Collections.Generic;

namespace HackAssembler
{
    class SymbolTable
    {
        private static readonly Dictionary<string, string> _computationCodes = new Dictionary<string, string>
        {
            { string.Empty, "0000000" },
            { "0", "0101010" },
            { "1", "0111111"},
            { "-1", "0111010" },
            { "D", "0001100" },
            { "A", "0110000" },
            { "!D", "0001101" },
            { "!A", "0110001" },
            { "-D", "0001111" },
            { "-A", "0110011" },
            { "D+1", "0011111" },
            { "A+1", "0110111" },
            { "D-1", "0001110" },
            { "A-1", "0110010" },
            { "D+A", "0000010" },
            { "D-A", "0010011" },
            { "A-D", "0000111" },
            { "D&A", "0000000" },
            { "D|A", "0010101" },
            { "M", "1110000" },
            { "!M", "1110001" },
            { "-M", "1110011" },
            { "M+1", "1110111" },
            { "M-1", "1110010" },
            { "D+M", "1000010" },
            { "D-M", "1010011" },
            { "M-D", "1000111" },
            { "D&M", "1000000" },
            { "D|M", "1010101" }
        };

        private static readonly Dictionary<string, string> _labels = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> _variables = new Dictionary<string, string>
        {
            {"SP", "0" },
            { "LCL", "1" },
            { "ARG", "10" },
            { "THIS", "11" },
            { "THAT", "100" },
            {"SCREEN", "100000000000000‬"},
            {"KBD", "‭110000000000000‬"},
            {"R0", "0" },
            {"R1", "1"},
            {"R2", "10"},
            {"R3", "11"},
            {"R4", "100"},
            {"R5", "101"},
            {"R6", "110"},
            {"R7", "111"},
            {"R8", "1000"},
            {"R9", "1001"},
            {"R10", "1010"},
            {"R11", "1011"},
            {"R12", "1100"},
            {"R13", "1101"},
            {"R14", "1110"},
            {"R15", "1111"}
        };

        private static readonly Dictionary<string, string> _destinationCodes = new Dictionary<string, string>
        {
            { "0", "000" },
            { string.Empty, "000" },
            { "M", "001" },
            { "D", "010" },
            { "MD", "011" },
            { "A", "100" },
            { "AM", "101" },
            { "AD", "110" },
            { "AMD", "111" }
        };

        private static readonly Dictionary<string, string> _jumpCodes = new Dictionary<string, string>
        {
            { "0", "000" },
            { string.Empty, "000" },
            { "JGT", "001" },
            { "JEQ", "010" },
            { "JGE", "011" },
            { "JLT", "100" },
            { "JNE", "101" },
            { "JLE", "110" },
            { "JMP", "111" }
        };

        public static void LoadLabels(string path)
        {

            using (var parser = new Parser(path))
            {
                while (parser.HasMoreCommands())
                {
                    var symbol = parser.GetNextSymbol();
                    if (symbol != null && symbol.Type == SymbolType.Label)
                    {
                        _labels[symbol.Value] =  Convert.ToString(parser.CurrentCommandIndex, 2);
                    }
                }
            }
        }

        public static void LoadSymbols(string path)
        {
            using (var parser = new Parser(path))
            {
                int variableAddress = 16;
                while (parser.HasMoreCommands())
                {
                    var symbol = parser.GetNextSymbol();
                    if (symbol != null && symbol.Type == SymbolType.Variable && !_labels.ContainsKey(symbol.Value) && !_variables.ContainsKey(symbol.Value))
                    {
                        _variables[symbol.Value] = Convert.ToString(variableAddress, 2);
                        variableAddress++;
                    }
                }
            }
        }

        public static string GetCode(ComponentType type, string name)
        {
            switch (type)
            {
                case ComponentType.Address:
                    return _variables.ContainsKey(name) ? _variables[name] : _labels[name];
                case ComponentType.Computation:
                    return _computationCodes[name];
                case ComponentType.Destination:
                    return _destinationCodes[name];
                case ComponentType.Jump:
                    return _jumpCodes[name];
                default:
                    throw new Exception("component not found");
            }
        }
    }
}