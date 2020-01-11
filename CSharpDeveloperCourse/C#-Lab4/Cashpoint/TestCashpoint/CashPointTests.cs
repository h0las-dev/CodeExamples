namespace Cashpoint.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CashPointTests
    {
        [TestMethod]
        public void LoadFromXmlFileTest()
        {
            string filename = @"..\..\..\Data\input.xml";
            var target = CashPoint.LoadFromXmlFile(filename);
            Assert.AreEqual(target.Total, 897u, "Из файла input.xml загружено неверное количество денег");
            Assert.IsTrue(target.CanGrant(500u), "Банкомат не смог выдать 500 из файла input.xml");
            Assert.IsFalse(target.CanGrant(59u), "Банкомат как-то смог выдать 59 из файла input.xml");
        }

        [TestMethod]
        public void SaveToXmlFileTest()
        {
            var tmpCashpoint = new CashPoint();

            tmpCashpoint.AddBanknote(10u);
            tmpCashpoint.AddBanknote(20u);
            tmpCashpoint.AddBanknote(30u);
            tmpCashpoint.AddBanknote(3u);
            tmpCashpoint.AddBanknote(100u);

            tmpCashpoint.SaveToXmlFile(@"..\..\..\Data\outputTest.xml");

            var tmpCashpointCopy = new CashPoint();
            tmpCashpointCopy = CashPoint.LoadFromXmlFile(@"..\..\..\Data\outputTest.xml");

            Console.WriteLine(tmpCashpointCopy.Total + "  " + tmpCashpoint.Total);
            Assert.AreEqual(tmpCashpointCopy.Total, tmpCashpoint.Total, "Метод сохранил объект неверно.");
        }

        [TestMethod]
        public void AddBanknoteTest()
        {
            var target = new CashPoint();
            target.AddBanknote(5);
            Assert.AreEqual((uint)5, target.Total, "Добавление единственной банкноты не было произведено");

            target.AddBanknote(50);
            Assert.AreEqual((uint)55, target.Total, "Добавление второй банкноты не было произведено");

            for (var i = 0; i < 255; i++)
            {
                target.AddBanknote(7);
            }

            Assert.AreEqual((uint)1840, target.Total, "Добавление 255 банкнот было произведено неверно.");
        }
        
        [TestMethod]
        public void AddSeveralBanknoteTest()
        {
            var target = new CashPoint();

            target.AddBanknote(5, 5);
            Assert.AreEqual(target.Count, 5, "Первые 5 банкнот не были добавлены.");

            target.AddBanknote(7, 10);
            Assert.AreEqual(target.Count, 15, "Второе множественное добавление банкнот было произведено неверно.");

            target.AddBanknote(1);
            Assert.AreEqual(target.Count, 16, "После множественного добавления 1 банкнота была добавлена неверно.");

            target.AddBanknote(1, 0);
            Assert.AreEqual(target.Count, 16, "Нулевое добавление банкнот было произведено неверно.");
        }

        [TestMethod]
        public void RemoveSeveralBanknoteTest()
        {
            var target = new CashPoint();

            target.AddBanknote(5, 5);
            target.AddBanknote(7, 10);

            target.RemoveBanknote(5, 4);
            target.RemoveBanknote(7, 9);
            Assert.AreEqual(target.Count, 2, "Банкомат извлёк больше или меньше купюр, чем нужно.");

            target.RemoveBanknote(5, 1);
            target.RemoveBanknote(7, 1);
            Assert.AreEqual(target.Count, 0, "Банкомат извлёк купюры не до конца.");

            target.RemoveBanknote(7, 1);
            Assert.AreEqual(target.Count, 0, "Банкомат извлёк несуществующие.");

            target.RemoveBanknote(5, 0);
            Assert.AreEqual(target.Count, 0, "Банкомат извлёк нулевое количество купюр неверно.");
        }

        [TestMethod]
        public void RemoveBanknoteTest()
        {
            var target = new CashPoint();

            target.RemoveBanknote(5);
            Assert.AreEqual(0u, target.Total, "Извлечена несуществующая купюра из пустого банкомата");

            target.AddBanknote(7);
            target.RemoveBanknote(3);
            Assert.AreEqual(7u, target.Total, "Извлечена несуществующая купюра");

            target.RemoveBanknote(7);
            Assert.AreEqual(0u, target.Total, "Купюры извлечены не полностью");
        }

        [TestMethod]
        public void CanGrantTest()
        {
            var target = new CashPoint();

            Assert.IsTrue(target.CanGrant(0), "Банкомат не может выдать 0");

            target.AddBanknote(5);
            Assert.IsTrue(target.CanGrant(5), "Банкомат не может выдать 5 = 5");

            // Нагрузка для проверки скорости.
            for (uint i = 1; i < 100; i++)
            {
                target.AddBanknote(i, 2);
            }

            target.AddBanknote(1000, 10);
            target.AddBanknote(500, 5);
            target.AddBanknote(200, 10);
            target.AddBanknote(50, 50);
            target.AddBanknote(20, 20);

            target.RemoveBanknote(50, 2);
            target.RemoveBanknote(1000, 1);

            for (uint i = 1; i < 20; i++)
            {
                target.RemoveBanknote(i);
            }
        }

        [TestMethod]
        public void TotalTest()
        {
            var target = new CashPoint();
            Assert.AreEqual(0u, target.Total, "Новый банкомат оказался не пустой");
        }
    }
}