using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expedia;
using Rhino.Mocks;
using System.Collections.Generic;

namespace ExpediaTest
{
    [TestClass]
    public class CarTest
    {
        private Car targetCar;
        private MockRepository mocks;

        [TestInitialize]
        public void TestInitialize()
        {
            targetCar = new Car(5);
            mocks = new MockRepository();
        }

        [TestMethod]
        public void TestThatCarInitializes()
        {
            Assert.IsNotNull(targetCar);
        }

        [TestMethod]
        public void TestThatCarHasCorrectBasePriceForFiveDays()
        {
            Assert.AreEqual(50, targetCar.getBasePrice());
        }

        [TestMethod]
        public void TestThatCarHasCorrectBasePriceForTenDays()
        {
            var target = new Car(10);
            Assert.AreEqual(80, target.getBasePrice());
        }

        [TestMethod]
        public void TestThatCarHasCorrectBasePriceForSevenDays()
        {
            var target = new Car(7);
            Assert.AreEqual(10 * 7 * .8, target.getBasePrice());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestThatCarThrowsOnBadLength()
        {
            new Car(-5);
        }

        [TestMethod()]
        public void TestThatCarDoesGetLocationFromTheDatabase()
        {
            IDatabase mockDB = mocks.DynamicMock<IDatabase>();
            String location = "5523 Place Rd";
            String anotherLocation = "7485 Location Ave";
            Expect.Call(mockDB.getCarLocation(5)).Return(location);
            Expect.Call(mockDB.getCarLocation(30)).Return(anotherLocation);



            mockDB.Stub(x => x.getCarLocation(Arg<int>.Is.Anything)).Return("Unknown Car");
            mocks.ReplayAll();

            Car target = new Car(10);
            target.Database = mockDB;

            String result;
            result = target.getCarLocation(5);
            Assert.AreEqual(location, result);
            result = target.getCarLocation(30);
            Assert.AreEqual(anotherLocation, result);
            result = target.getCarLocation(300);
            Assert.AreEqual("Unknown Car", result);
            mocks.VerifyAll();


        }


        [TestMethod()]
        public void TestThatCarDoesGetMileageFromDatabase()
        {
            IDatabase mockDatabase = mocks.StrictMock<IDatabase>();
            Int32 Miles = 0;
            for (var i = 0; i < 100; i++)
            {
                Miles = Miles + 1;
            }
            Expect.Call(mockDatabase.Miles).PropertyBehavior();
            mocks.ReplayAll();
            mockDatabase.Miles = Miles;
            var target = new Car(10);
            target.Database = mockDatabase;
            int mileage = target.Mileage;
            Assert.AreEqual(mileage, Miles);
            mocks.VerifyAll();
        }
        [TestMethod()]
        public void TestBMWObjectMother()
        {
            var target = ObjectMother.BMW();
            Assert.AreEqual(80, target.getBasePrice());
            Assert.AreEqual("BMW R8 Sports Car", target.Name);
        }

    }
}
