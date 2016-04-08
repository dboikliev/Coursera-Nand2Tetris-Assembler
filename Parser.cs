using System;
using System.IO;
using System.Text.RegularExpressions;

namespace HackAssembler
{
    class Parser : IDisposable
    {
        private readonly StreamReader _reader;
        public int CurrentCommandIndex { get; private set; }

        public Parser(FileStream file)
        {
            _reader = new StreamReader(file);
        }

        public Parser(string fileName) : this(File.Open(fileName, FileMode.Open, FileAccess.Read)) { }

        public Command GetNextCommand()
        {
            string command;
            do
            {
                command = _reader.ReadLine();
                if (command == null)
                {
                    return null;
                }
                command = command.Trim();
            } while (!_reader.EndOfStream && !IsValidCommand(command));

            command = TrimComments(command);
            CurrentCommandIndex++;
            if (IsACommand(command))
            {
                return new АCommand(command);
            }
            return new CCommand(command);
        }

        public Symbol GetNextSymbol()
        {
            string command;
            do
            {
                command = _reader.ReadLine();
                if (command == null)
                {
                    return null;
                }
                command = command.Trim();
                if (IsValidCommand(command))
                {
                    CurrentCommandIndex++;
                }

            } while (!_reader.EndOfStream && !IsVariable(command) && !IsLabel(command));
            if (IsVariable(command) || IsLabel(command))
            {
                return new Symbol(command);
            }
            return new Symbol(string.Empty);
        }


        public bool IsVariable(string line)
        {
            return Regex.IsMatch(line, @"@[\w_]+");
        }

        public bool HasMoreCommands()
        {
            return !_reader.EndOfStream;
        }

        private string TrimComments(string command)
        {
            var trimmedCommand = Regex.Replace(command, @"(\*{1,}|//|/\*{1,}).*", string.Empty).Trim();
            return trimmedCommand;
        }

        bool IsValidCommand(string line)
        {
            var isValidCommand = !(IsEmpty(line) || IsComment(line) || IsLabel(line));
            return isValidCommand;
        }

        private bool IsLabel(string line)
        {
            var isLabel = Regex.IsMatch(line.Trim(), @"^\(.+\)$");
            return isLabel;
        }

        private bool IsComment(string line)
        {
            var isComment = Regex.IsMatch(line.Trim(), @"^(\*{1,}|//|/\*{1,}).*");
            return isComment;
        }
        private bool IsEmpty(string line)
        {
            return string.IsNullOrWhiteSpace(line);
        }

        bool IsACommand(string line)
        {
            var isACommand = Regex.IsMatch(line, @"@[\d\w]+");
            return isACommand;
        }
        
        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
