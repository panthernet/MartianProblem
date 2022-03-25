using System;
using MartianProblem.Solver.Classes;

namespace MartianProblem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rc = new RobotController();
            rc.InputCommand("5 3");
            rc.InputCommand("1 1 E");
            rc.InputCommand("RFRFRFRF");
            rc.InputCommand("3 2 N");
            rc.InputCommand("FRRFLLFFRRFLL");
            rc.InputCommand("0 3 W");
            rc.InputCommand("LLFFFLFLFL");

            rc.Run();

            Console.ReadKey();
        }
    }
}
