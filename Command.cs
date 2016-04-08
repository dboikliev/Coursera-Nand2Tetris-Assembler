namespace HackAssembler
{
    abstract class Command
    {
        public CommandType Type { get; private set; }
        protected  CommandComponent[] Components;

        protected Command(CommandType type, params CommandComponent[] components)
        {
            Type = type;
            Components = components;
        }

        public abstract string ToBinary();
    }
}