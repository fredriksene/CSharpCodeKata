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

        #region Set of tests for new Award.UpdateQuality refactor, matching the entire suite of tests, 1 for 1, from UpdateQualityAwardsTests

        #region Blue Compare

        /// <summary>
        /// Execute a QualityUpdate and make sure Quality increases by 1 when not expired
        /// Covers Blue Compare
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_QualityUpdate_Unexpired()
        {
            var bc = AllAwards["Blue Compare"].Find(x => x.ExpiresIn == 11 && x.Quality == 20);

            Assert.IsNotNull(bc);
            Assert.IsTrue(bc.Name == "Blue Compare");
            Assert.IsTrue(bc.ExpiresIn == 11);
            Assert.IsTrue(bc.Quality == 20);

            bc.UpdateQuality();

            Assert.IsTrue(bc.Quality == 21);
        }

        /// <summary>
        /// Covers Blue Compare
        /// Execute a QualityUpdate and make sure Quality drops to 0 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_QualityUpdate_Expired()
        {
            var bc = AllAwards["Blue Compare"].Find(x => x.ExpiresIn == 0 && x.Quality == 20);

            Assert.IsNotNull(bc);
            Assert.IsTrue(bc.Name == "Blue Compare");
            Assert.IsTrue(bc.ExpiresIn == 0);
            Assert.IsTrue(bc.Quality == 20);

            bc.UpdateQuality();

            Assert.IsTrue(bc.Quality == 0);
        }

        /// <summary>
        /// Covers Blue Compare
        /// Execute a QualityUpdate and make sure Quality increases by 3 the day before the award expires
        /// Execute a QualityUpdate and make sure Quality drops to 0 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_QualityUpdate_UnexpiredExpired()
        {
            var bc = AllAwards["Blue Compare"].Find(x => x.ExpiresIn == 1 && x.Quality == 20);

            Assert.IsNotNull(bc);
            Assert.IsTrue(bc.Name == "Blue Compare");
            Assert.IsTrue(bc.ExpiresIn == 1);
            Assert.IsTrue(bc.Quality == 20);

            bc.UpdateQuality();

            Assert.IsTrue(bc.Quality == 23);

            bc.UpdateQuality();

            Assert.IsTrue(bc.Quality == 0);
        }

        /// <summary>
        /// Covers Blue Compare
        /// Execute a QualityUpdate and make sure Quality does not exceed 50 when not expired
        /// Execute a QualityUpdate and make sure Quality drops to 0 when expired
        /// Execute a QualityUpdate and make sure Quality remains 0 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_QualityUpdate_MaxMinQuality()
        {
            var bc = AllAwards["Blue Compare"].Find(x => x.ExpiresIn == 1 && x.Quality == 50);

            Assert.IsNotNull(bc);
            Assert.IsTrue(bc.Name == "Blue Compare");
            Assert.IsTrue(bc.ExpiresIn == 1);
            Assert.IsTrue(bc.Quality == 50);

            bc.UpdateQuality();

            // Cannot exceed 50
            Assert.IsTrue(bc.Quality == 50);

            bc.UpdateQuality();

            // Drops to 0 after it expires
            Assert.IsTrue(bc.Quality == 0);

            bc.UpdateQuality();

            // Remains 0 after it expires
            Assert.IsTrue(bc.Quality == 0);
        }

        #endregion //END - Blue Compare

        #region Blue Distinction Plus

        /// <summary>
        /// Execute a QualityUpdate and make sure Quality remains 80
        /// Covers Blue Distinction Plus
        /// </summary>
        [TestMethod]
        public void TestImmutabilityOfBlueDistinctionPlus()
        {
            var awards = AllAwards["Blue Distinction Plus"];
            int count = awards.Count;
            for (int idx = 0; idx < count; idx++)
            {
                Assert.IsTrue(awards[idx].Quality == 80);

                awards[idx].UpdateQuality();

                Assert.IsTrue(awards[idx].Quality == 80);
            }
        }

        /// <summary>
        /// Execute a QualityUpdate and make sure Quality remains 80 when not expired
        /// Covers Blue Distinction Plus
        /// </summary>
        [TestMethod]
        public void Test_BlueDistinctionPlus_QualityUpdate_Unexpired()
        {
            var bdp = AllAwards["Blue Distinction Plus"].Find(x => x.ExpiresIn == 5 && x.Quality == 80);

            Assert.IsNotNull(bdp);
            Assert.IsTrue(bdp.Name == "Blue Distinction Plus");
            Assert.IsTrue(bdp.ExpiresIn == 5);
            Assert.IsTrue(bdp.Quality == 80);

            bdp.UpdateQuality();

            Assert.IsTrue(bdp.Quality == 80);
        }

        /// <summary>
        /// Covers Blue Distinction Plus
        /// Execute a QualityUpdate and make sure Quality remains at 80 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueDistinctionPlus_QualityUpdate_Expired()
        {
            var bdp = AllAwards["Blue Distinction Plus"].Find(x => x.ExpiresIn == 0 && x.Quality == 80);

            Assert.IsNotNull(bdp);
            Assert.IsTrue(bdp.Name == "Blue Distinction Plus");
            Assert.IsTrue(bdp.ExpiresIn == 0);
            Assert.IsTrue(bdp.Quality == 80);

            bdp.UpdateQuality();

            Assert.IsTrue(bdp.Quality == 80);
        }

        /// <summary>
        /// Covers Blue Distinction Plus
        /// Execute a QualityUpdate and make sure Quality remains 80 when not expired
        /// Execute a QualityUpdate and make sure Quality remains 80 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueDistinctionPlus_QualityUpdate_UnexpiredExpired()
        {
            var bdp = AllAwards["Blue Distinction Plus"].Find(x => x.ExpiresIn == 1 && x.Quality == 80);

            Assert.IsNotNull(bdp);
            Assert.IsTrue(bdp.Name == "Blue Distinction Plus");
            Assert.IsTrue(bdp.ExpiresIn == 1);
            Assert.IsTrue(bdp.Quality == 80);

            bdp.UpdateQuality();

            Assert.IsTrue(bdp.Quality == 80);

            bdp.UpdateQuality();

            Assert.IsTrue(bdp.Quality == 80);
        }

        #endregion //END - Blue Distinction Plus

        #region Blue First

        /// <summary>
        /// Execute a QualityUpdate and make sure Quality increases by 1 when not expired
        /// Covers Blue First
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_QualityUpdate_Unexpired()
        {
            var bf = AllAwards["Blue First"].Find(x => x.ExpiresIn == 2 && x.Quality == 0);

            Assert.IsNotNull(bf);
            Assert.IsTrue(bf.Name == "Blue First");
            Assert.IsTrue(bf.ExpiresIn == 2);
            Assert.IsTrue(bf.Quality == 0);

            bf.UpdateQuality();

            Assert.IsTrue(bf.Quality == 1);
        }

        /// <summary>
        /// Covers Blue First
        /// Execute a QualityUpdate and make sure Quality increases by 2 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_QualityUpdate_Expired()
        {
            var bf = AllAwards["Blue First"].Find(x => x.ExpiresIn == 0 && x.Quality == 10);

            Assert.IsNotNull(bf);
            Assert.IsTrue(bf.Name == "Blue First");
            Assert.IsTrue(bf.ExpiresIn == 0);
            Assert.IsTrue(bf.Quality == 10);

            bf.UpdateQuality();

            Assert.IsTrue(bf.Quality == 12);
        }

        /// <summary>
        /// Covers Blue First
        /// Execute a QualityUpdate and make sure Quality increases by 1 when not expired
        /// Execute a QualityUpdate and make sure Quality increases by 2 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_QualityUpdate_UnexpiredExpired()
        {
            var bf = AllAwards["Blue First"].Find(x => x.ExpiresIn == 1 && x.Quality == 10);

            Assert.IsNotNull(bf);
            Assert.IsTrue(bf.Name == "Blue First");
            Assert.IsTrue(bf.ExpiresIn == 1);
            Assert.IsTrue(bf.Quality == 10);

            bf.UpdateQuality();

            Assert.IsTrue(bf.Quality == 11);

            bf.UpdateQuality();

            Assert.IsTrue(bf.Quality == 13);
        }

        /// <summary>
        /// Covers Blue First
        /// Execute a QualityUpdate and make sure Quality increases by 2 the day it expires
        /// Execute a QualityUpdate and make sure Quality increases by 1 when expired to max of 50
        /// Execute a QualityUpdate and make sure Quality does not exceed 50 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_QualityUpdate_MaxQuality()
        {
            var bf = AllAwards["Blue First"].Find(x => x.ExpiresIn == 0 && x.Quality == 47);

            Assert.IsNotNull(bf);
            Assert.IsTrue(bf.Name == "Blue First");
            Assert.IsTrue(bf.ExpiresIn == 0);
            Assert.IsTrue(bf.Quality == 47);

            bf.UpdateQuality();

            // Now Expired
            Assert.IsTrue(bf.Quality == 49);

            bf.UpdateQuality();

            // Expired and at Max
            Assert.IsTrue(bf.Quality == 50);

            bf.UpdateQuality();

            // Cannot exceed 50
            Assert.IsTrue(bf.Quality == 50);
        }

        #endregion //END - Blue First

        #region Generic Awards

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_QualityUpdate_Unexpired()
        {
            var acme = AllAwards["ACME Partner Facility"].Find(x => x.ExpiresIn == 5 && x.Quality == 7);
            var gov = AllAwards["Gov Quality Plus"].Find(x => x.ExpiresIn == 10 && x.Quality == 20);
            var top = AllAwards["Top Connected Providers"].Find(x => x.ExpiresIn == 3 && x.Quality == 6);

            Assert.IsNotNull(acme);
            Assert.IsTrue(acme.Name == "ACME Partner Facility");
            Assert.IsTrue(acme.ExpiresIn == 5);
            Assert.IsTrue(acme.Quality == 7);
            Assert.IsNotNull(gov);
            Assert.IsTrue(gov.Name == "Gov Quality Plus");
            Assert.IsTrue(gov.ExpiresIn == 10);
            Assert.IsTrue(gov.Quality == 20);
            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.ExpiresIn == 3);
            Assert.IsTrue(top.Quality == 6);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            Assert.IsTrue(acme.Quality == 6);
            Assert.IsTrue(gov.Quality == 19);
            Assert.IsTrue(top.Quality == 5);
        }

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_QualityUpdate_Expired()
        {
            var acme = AllAwards["ACME Partner Facility"].Find(x => x.ExpiresIn == 0 && x.Quality == 7);
            var gov = AllAwards["Gov Quality Plus"].Find(x => x.ExpiresIn == 0 && x.Quality == 20);
            var top = AllAwards["Top Connected Providers"].Find(x => x.ExpiresIn == 0 && x.Quality == 6);

            Assert.IsNotNull(acme);
            Assert.IsTrue(acme.Name == "ACME Partner Facility");
            Assert.IsTrue(acme.ExpiresIn == 0);
            Assert.IsTrue(acme.Quality == 7);
            Assert.IsNotNull(gov);
            Assert.IsTrue(gov.Name == "Gov Quality Plus");
            Assert.IsTrue(gov.ExpiresIn == 0);
            Assert.IsTrue(gov.Quality == 20);
            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.ExpiresIn == 0);
            Assert.IsTrue(top.Quality == 6);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            Assert.IsTrue(acme.Quality == 5);
            Assert.IsTrue(gov.Quality == 18);
            Assert.IsTrue(top.Quality == 4);
        }

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// These Awards all have the same business rules for UpdateQuality
        /// Covers two calls to UpdateQuality
        ///     the first is when all three awards are unexpired
        ///     the second is when all three awards are expired
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_QualityUpdate_UnexpiredExpired()
        {
            var acme = AllAwards["ACME Partner Facility"].Find(x => x.ExpiresIn == 1 && x.Quality == 7);
            var gov = AllAwards["Gov Quality Plus"].Find(x => x.ExpiresIn == 1 && x.Quality == 20);
            var top = AllAwards["Top Connected Providers"].Find(x => x.ExpiresIn == 1 && x.Quality == 6);

            Assert.IsNotNull(acme);
            Assert.IsTrue(acme.Name == "ACME Partner Facility");
            Assert.IsTrue(acme.ExpiresIn == 1);
            Assert.IsTrue(acme.Quality == 7);
            Assert.IsNotNull(gov);
            Assert.IsTrue(gov.Name == "Gov Quality Plus");
            Assert.IsTrue(gov.ExpiresIn == 1);
            Assert.IsTrue(gov.Quality == 20);
            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.ExpiresIn == 1);
            Assert.IsTrue(top.Quality == 6);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Unexpired
            Assert.IsTrue(acme.Quality == 6);
            Assert.IsTrue(gov.Quality == 19);
            Assert.IsTrue(top.Quality == 5);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Expired
            Assert.IsTrue(acme.Quality == 4);
            Assert.IsTrue(gov.Quality == 17);
            Assert.IsTrue(top.Quality == 3);
        }

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_QualityUpdate_MinQuality()
        {
            var acme = AllAwards["ACME Partner Facility"].Find(x => x.ExpiresIn == 1 && x.Quality == 1);
            var gov = AllAwards["Gov Quality Plus"].Find(x => x.ExpiresIn == 1 && x.Quality == 1);
            var top = AllAwards["Top Connected Providers"].Find(x => x.ExpiresIn == 1 && x.Quality == 1);

            Assert.IsNotNull(acme);
            Assert.IsTrue(acme.Name == "ACME Partner Facility");
            Assert.IsTrue(acme.ExpiresIn == 1);
            Assert.IsTrue(acme.Quality == 1);
            Assert.IsNotNull(gov);
            Assert.IsTrue(gov.Name == "Gov Quality Plus");
            Assert.IsTrue(gov.ExpiresIn == 1);
            Assert.IsTrue(gov.Quality == 1);
            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.ExpiresIn == 1);
            Assert.IsTrue(top.Quality == 1);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Unexpired
            Assert.IsTrue(acme.Quality == 0);
            Assert.IsTrue(gov.Quality == 0);
            Assert.IsTrue(top.Quality == 0);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Expired
            Assert.IsTrue(acme.Quality == 0);
            Assert.IsTrue(gov.Quality == 0);
            Assert.IsTrue(top.Quality == 0);
        }

        #endregion //END - Generic Awards

        #endregion //END - Set of tests for new Award.UpdateQuality refactor, matching the entire suite of tests, 1 for 1, from UpdateQualityAwardsTests

    }
}
