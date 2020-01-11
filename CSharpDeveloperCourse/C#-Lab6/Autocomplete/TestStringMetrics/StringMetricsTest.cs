namespace TestAutocomplete
{
    using Autocomplete;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringMetricsTest
    {
        [TestMethod]
        public void LongestCommonSubstringLengthTest()
        {
            Assert.AreEqual(0, string.Empty.Similarity(string.Empty));
            Assert.AreEqual(0, string.Empty.Similarity(null));
            Assert.AreEqual(0, StringMetrics.Similarity(null, string.Empty));
            Assert.AreEqual(0, "a".Similarity(string.Empty));
            Assert.AreEqual(0, "a".Similarity(null));
            Assert.AreEqual(0, string.Empty.Similarity("a"));
            Assert.AreEqual(0, StringMetrics.Similarity(null, "a"));
            Assert.AreEqual(1, "a".Similarity("a"));
            Assert.AreEqual(1, "a".Similarity("ba"));
            Assert.AreEqual(1, "ba".Similarity("a"));
            Assert.AreEqual(2, "ba".Similarity("ba"));
            Assert.AreEqual(7, "abracadabra".Similarity("cadabra"));      // cadabra -> 7
            Assert.AreEqual(5, "abracadabra".Similarity("aaaaaaa"));      // aaaaa -> 5
            Assert.AreEqual(0, "abracadabra".Similarity("system"));       // -> 0
            Assert.AreEqual(11, "abracadabra".Similarity("abrAcaDabRa")); // abracadabra -> 11
            Assert.AreEqual(1, "monopoly".Similarity("m"));               // m -> 1
            Assert.AreEqual(1, "monopoly".Similarity("y"));               // y -> 1
            Assert.AreEqual(3, "monopoly".Similarity("yloponom"));        // ooo | opo | ono -> 3
            Assert.AreEqual(2, "quadrocycle".Similarity("helicopter"));   // oe | ce -> 2
        }
    }
}
