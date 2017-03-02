using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProviderQuality.Console;

namespace ProviderQuality.Tests
{
    [TestClass]
    public class AwardsTests
    {
        List<string> AwardTypes;

        Dictionary<string, List<Award>> AllAwards;

        [TestInitialize]
        public void TestInit()
        {
            AllAwards = new Dictionary<string, List<Award>>();

            AwardTypes = new List<string>(
                new string[] {
                    "Blue First"
                    , "Blue Compare"
                    , "Blue Distinction Plus"
                    , "Gov Quality Plus"
                    , "ACME Partner Facility"
                    , "Top Connected Providers"
                }
            );

            foreach (string award in AwardTypes)
            {
                List<Award> awards = new List<Award>();

                // 51 Quality records from 0 to 50
                for (int quality = 0; quality <= 50; quality++)
                {
                    // 13 Expires records from 11 to -1
                    // Currently the only date thresholds that matter are: (n > 10), (10 >= n > 5), (5 >= n >= 0), (n < 0) Expired
                    for (int expiresIn = 11; expiresIn >= -1; expiresIn--)
                    {
                        if (!AllAwards.ContainsKey(award))
                        {
                            AllAwards.Add(award, awards);
                        }

                        // Total should be 6 types X (51 qualities X 13 expiresIn) [663] = 3,978 awards
                        AllAwards[award].Add(AwardFactory.Get(award, expiresIn, quality));
                    }
                }
            }
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            AwardTypes.Clear();
            AwardTypes = null;

            foreach(string key in AllAwards.Keys)
            {
                AllAwards[key].Clear();
            }

            AllAwards.Clear();
            AllAwards = null;
        }

        /// <summary>
        /// Verifying the construction of all instances of all awards with all possible combinations of properties.
        /// </summary>
        [TestMethod]
        public void TestAllAwardsValidation()
        {
            Assert.IsNotNull(AllAwards);
            Assert.IsTrue(AllAwards.Keys.Count == 6);

            Assert.IsTrue(AllAwards.ContainsKey("ACME Partner Facility"));
            Assert.IsTrue(AllAwards.ContainsKey("Gov Quality Plus"));
            Assert.IsTrue(AllAwards.ContainsKey("Top Connected Providers"));

            Assert.IsTrue(AllAwards.ContainsKey("Blue First"));
            Assert.IsTrue(AllAwards.ContainsKey("Blue Compare"));

            Assert.IsTrue(AllAwards.ContainsKey("Blue Distinction Plus"));

            Assert.IsTrue(AllAwards["ACME Partner Facility"].Count == 663);
            Assert.IsTrue(AllAwards["Gov Quality Plus"].Count == 663);
            Assert.IsTrue(AllAwards["Top Connected Providers"].Count == 663);

            Assert.IsTrue(AllAwards["Blue First"].Count == 663);
            Assert.IsTrue(AllAwards["Blue Compare"].Count == 663);

            Assert.IsTrue(AllAwards["Blue Distinction Plus"].Count == 663);
        }
    }
}
