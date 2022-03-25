using MartianProblem.Solver.Classes;
using MartianProblem.Solver.Enums;
using MartianProblem.Solver.Interfaces;
using NUnit.Framework;

namespace MartianProblem.Tests
{
    public class RobotTests
    {
        private RobotFactory<char> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new RobotFactory<char>();
        }

        [Test]
        public void TestCreate()
        {
            var robot = CreateRobot();
            Assert.IsNotNull(robot);
            Assert.Pass();
        }

        [Test]
        public void TestRotate()
        {
            var robot = CreateRobot();
            robot.SetOrientation('W');
            Assert.AreEqual(robot.Rotation, 270);

            var rotation = robot.GetOrientation();
            Assert.AreEqual(rotation, 'W');

            robot.RotateRight();
            Assert.AreEqual(robot.Rotation, 0);

            robot.RotateLeft();
            Assert.AreEqual(robot.Rotation, 270);

            Assert.Pass();
        }

        [Test]
        public void TestMove()
        {
            var robot = CreateRobot();
            robot.Move();
            Assert.AreEqual(robot.PositionX, 0);
            Assert.AreEqual(robot.PositionY, 1);

            Assert.Pass();
        }

        [Test]
        public void TestSimulateMove()
        {
            var robot = CreateRobot();
            var (key, value) = robot.SimulateMove();
            Assert.AreEqual(key, 0);
            Assert.AreEqual(value, 1);

            Assert.Pass();
        }

        [Test]
        public void TestAddCommand()
        {
            var robot = CreateRobot();
            robot.AddCommands(new []{'R','F','F'});
            var command = robot.FetchNextCommand();
            Assert.AreEqual(command, 'R');
            command = robot.FetchNextCommand();
            Assert.AreEqual(command, 'F');
            command = robot.FetchNextCommand();
            Assert.AreEqual(command, 'F');

            Assert.Pass();
        }

        [Test]
        public void TestLost()
        {
            var robot = CreateRobot();
            robot.MarkAsLost();
            Assert.AreEqual(robot.IsLost, true);

            Assert.Pass();
        }

        private IRobot<char> CreateRobot()
        {
            var robot = _factory.CreateRobot(RobotTypeEnum.Default, 0, 0);
            Assert.IsNotNull(robot);
            return robot;
        }
    }
}