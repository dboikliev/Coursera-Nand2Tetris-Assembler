using System.IO;

namespace HackAssembler
{
    class Program
    {
        static void Main(string[] args)
        {
            var asmFilePath = @"D:\Deyan\Documents\pCloud Sync\Courses\Coursera\From NAND to Tetris Part I\nand2tetris\projects\06\max\Max.asm";//args[0];
            var outputFilePath = @"D:\Max.hack"; //args[1];
            SymbolTable.LoadLabels(asmFilePath);
            SymbolTable.LoadSymbols(asmFilePath);
            var parser = new Parser(asmFilePath);
            using (StreamWriter writer = new StreamWriter(new FileInfo(outputFilePath).Open(FileMode.Create, FileAccess.Write)))
            {
                while (parser.HasMoreCommands())
                {
                    var command = parser.GetNextCommand();
                    var binary = command.ToBinary();
                    writer.WriteLine(binary);
                }
            }
        }
    }
}
