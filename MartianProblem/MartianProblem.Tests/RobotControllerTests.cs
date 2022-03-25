using System;
using MartianProblem.Solver.Classes;
using NUnit.Framework;

namespace MartianProblem.Tests
{
    public class RobotControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateTest()
        {
            RobotController rc;
            try
            {
                rc = new RobotController();
            }
            catch(Exception ex)
            {
                Assert.Fail($"Unhandled exception: {ex}");
                return;
            }

            Assert.IsNotNull(rc);
            Assert.Pass();
        }

        [Test]
        public void RunValidTest()
        {
            var rc = new RobotController();
            Assert.IsNotNull(rc);

            var result = rc.InputCommand("5 3");
            Assert.AreEqual(result, true);

            result = rc.InputCommand("1 1 E");
            Assert.AreEqual(result, true);

            result = rc.InputCommand("RFFLL");
            Assert.AreEqual(result, true);

            rc.Run();


            Assert.Pass();
        }

        [Test]
        public void RunInvalidTest()
        {
            var rc = new RobotController();
            Assert.IsNotNull(rc);

            var result = rc.InputCommand("1 1 E");
            Assert.AreEqual(result, false);

            result = rc.Run();
            Assert.AreEqual(result, false);


            Assert.Pass();
        }

        [Test]
        public void RunInvalidTest1()
        {
            var rc = new RobotController();
            Assert.IsNotNull(rc);

            var result = rc.InputCommand("55 3");
            Assert.AreEqual(result, false);

            result = rc.InputCommand("0 3");
            Assert.AreEqual(result, false);

            result = rc.InputCommand("10 -1");
            Assert.AreEqual(result, false);

            result = rc.InputCommand("10 A");
            Assert.AreEqual(result, false);

            result = rc.InputCommand("1 1 E");
            Assert.AreEqual(result, false);


            Assert.Pass();
        }

        [Test]
        public void RunInvalidTest2()
        {
            var rc = new RobotController();
            Assert.IsNotNull(rc);

            var result = rc.InputCommand("5 3");
            Assert.AreEqual(result, true);

            result = rc.InputCommand("2 1 1 E");
            Assert.AreEqual(result, false);

            result = rc.InputCommand("2 1");
            Assert.AreEqual(result, false);

            result = rc.InputCommand("RFF");
            Assert.AreEqual(result, false);

            result = rc.InputCommand("2 1");
            Assert.AreEqual(result, false);

            Assert.Pass();
        }

        [Test]
        public void RunInvalidTest3()
        {
            var rc = new RobotController();
            Assert.IsNotNull(rc);

            var result = rc.InputCommand("5 3");
            Assert.AreEqual(result, true);

            result = rc.InputCommand("2 1 E");
            Assert.AreEqual(result, true);

            result = rc.InputCommand("FFA");
            Assert.AreEqual(result, false);

            result = rc.InputCommand("2 1 N");
            Assert.AreEqual(result, false);

            result = rc.InputCommand("FFFFFRRRRRFFFFFRRRRRFFFFFRRRRRFFFFFRRRRRFFFFFRRRRRFFFFFRRRRRFFFFFRRRRRFFFFFRRRRRFFFFFRRRRRFFFFFRRRRRFFFFFRRRRR");
            Assert.AreEqual(result, false);

            Assert.Pass();
        }
    }
}