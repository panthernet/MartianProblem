using MartianProblem.Solver.Classes;
using MartianProblem.Solver.Enums;
using NUnit.Framework;

namespace MartianProblem.Tests
{
    public class RobotFactoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCreate()
        {
            var factory = new RobotFactory<char>();
            Assert.IsNotNull(factory);

            Assert.Pass();
        }

        [Test]
        public void TestCreateRobot()
        {
            var factory = new RobotFactory<char>();
            Assert.IsNotNull(factory);

            var robot = factory.CreateRobot(RobotTypeEnum.Default, 0, 0);
            Assert.IsNotNull(robot);

            Assert.Pass();
        }

    }
}