using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProviderQuality.Console;
using System.Collections.Generic;

namespace ProviderQuality.Tests
{
    [TestClass]
    public class AwardsTests
    {
        IList<string> AwardTypes;

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
                    , "Blue Star"
                }
            );

            foreach (string award in AwardTypes)
            {
                List<Award> awards = new List<Award>();

                // 51 Quality records from 0 to 50
                for (int quality = 0; quality <= 50; quality++)
                {
                    // 13 Expires records from 11 to -1
                    // Currently the only date thresholds that matter are: (n > 10), (10 >= n > 5), (5 >= n >= 0), (n < 0) Expired, (n >=0) Unexpired
                    for (int expiresIn = 11; expiresIn >= -1; expiresIn--)
                    {
                        if (!AllAwards.ContainsKey(award))
                        {
                            AllAwards.Add(award, awards);
                        }

                        // Total should be 7 types X (51 qualities X 13 expiresIn) [663] = 4,641 awards
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
            Assert.IsTrue(AllAwards.Keys.Count == 7);

            Assert.IsTrue(AllAwards.ContainsKey("ACME Partner Facility"));
            Assert.IsTrue(AllAwards.ContainsKey("Gov Quality Plus"));
            Assert.IsTrue(AllAwards.ContainsKey("Top Connected Providers"));

            Assert.IsTrue(AllAwards.ContainsKey("Blue First"));
            Assert.IsTrue(AllAwards.ContainsKey("Blue Compare"));

            Assert.IsTrue(AllAwards.ContainsKey("Blue Distinction Plus"));

            Assert.IsTrue(AllAwards.ContainsKey("Blue Star"));

            Assert.IsTrue(AllAwards["ACME Partner Facility"].Count == 663);
            Assert.IsTrue(AllAwards["Gov Quality Plus"].Count == 663);
            Assert.IsTrue(AllAwards["Top Connected Providers"].Count == 663);

            Assert.IsTrue(AllAwards["Blue First"].Count == 663);
            Assert.IsTrue(AllAwards["Blue Compare"].Count == 663);

            Assert.IsTrue(AllAwards["Blue Distinction Plus"].Count == 663);
            Assert.IsTrue(AllAwards["Blue Star"].Count == 663);
        }

        #region Set of tests for new Award.UpdateQuality refactor, matching the entire suite of tests, 1 for 1, from UpdateQualityAwardsTests

        #region Blue Star

        /// <summary>
        /// Covers Blue Star and a Top Connected Providers award with the same values
        /// Execute an UpdateQuality and make sure Quality decreases by 2 when not expired
        /// </summary>
        [TestMethod]
        public void Test_BlueStar_UpdateQuality_Unexpired()
        {
            var top = AllAwards["Top Connected Providers"].Find(x => x.OriginalExpiresIn == 3 && x.Quality == 6);
            var bs = AllAwards["Blue Star"].Find(x => x.OriginalExpiresIn == 3 && x.Quality == 6);

            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.OriginalExpiresIn == 3);
            Assert.IsTrue(top.ExpiresIn == 3);
            Assert.IsTrue(top.Quality == 6);

            Assert.IsNotNull(bs);
            Assert.IsTrue(bs.Name == "Blue Star");
            Assert.IsTrue(bs.OriginalExpiresIn == 3);
            Assert.IsTrue(bs.ExpiresIn == 3);
            Assert.IsTrue(bs.Quality == 6);

            top.UpdateQuality();
            bs.UpdateQuality();

            // Decreased by 1 when not expired
            Assert.IsTrue(top.OriginalExpiresIn == 3);
            Assert.IsTrue(top.ExpiresIn == 2);
            Assert.IsTrue(top.Quality == 5);

            // Decreased by 2 when not expired
            Assert.IsTrue(bs.OriginalExpiresIn == 3);
            Assert.IsTrue(bs.ExpiresIn == 2);
            Assert.IsTrue(bs.Quality == 4);
        }

        /// <summary>
        /// Covers Blue Star and a Top Connected Providers award with the same values
        /// Execute an UpdateQuality and make sure Quality decreases by 4 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueStar_UpdateQuality_Expired()
        {
            var top = AllAwards["Top Connected Providers"].Find(x => x.OriginalExpiresIn == 0 && x.Quality == 6);
            var bs = AllAwards["Blue Star"].Find(x => x.OriginalExpiresIn == 0 && x.Quality == 6);

            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.OriginalExpiresIn == 0);
            Assert.IsTrue(top.ExpiresIn == 0);
            Assert.IsTrue(top.Quality == 6);

            Assert.IsNotNull(bs);
            Assert.IsTrue(bs.Name == "Blue Star");
            Assert.IsTrue(bs.OriginalExpiresIn == 0);
            Assert.IsTrue(bs.ExpiresIn == 0);
            Assert.IsTrue(bs.Quality == 6);

            top.UpdateQuality();
            bs.UpdateQuality();

            // Decreased by 2 when expired
            Assert.IsTrue(top.OriginalExpiresIn == 0);
            Assert.IsTrue(top.ExpiresIn == -1);
            Assert.IsTrue(top.Quality == 4);

            // Decreased by 4 when expired
            Assert.IsTrue(bs.OriginalExpiresIn == 0);
            Assert.IsTrue(bs.ExpiresIn == -1);
            Assert.IsTrue(bs.Quality == 2);
        }

        /// <summary>
        /// Covers Blue Star and a Top Connected Providers award with the same values
        /// Execute an UpdateQuality and make sure Quality decreases by 2 when not expired
        /// Execute an UpdateQuality and make sure Quality decreases by 4 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueStar_UpdateQuality_UnexpiredExpired()
        {
            var top = AllAwards["Top Connected Providers"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 6);
            var bs = AllAwards["Blue Star"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 6);

            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == 1);
            Assert.IsTrue(top.Quality == 6);

            Assert.IsNotNull(bs);
            Assert.IsTrue(bs.Name == "Blue Star");
            Assert.IsTrue(bs.OriginalExpiresIn == 1);
            Assert.IsTrue(bs.ExpiresIn == 1);
            Assert.IsTrue(bs.Quality == 6);

            top.UpdateQuality();
            bs.UpdateQuality();

            // Unexpired
            // Decreased by 1 when not expired
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == 0);
            Assert.IsTrue(top.Quality == 5);

            // Decreased by 2 when not expired
            Assert.IsTrue(bs.OriginalExpiresIn == 1);
            Assert.IsTrue(bs.ExpiresIn == 0);
            Assert.IsTrue(bs.Quality == 4);

            top.UpdateQuality();
            bs.UpdateQuality();

            // Expired
            // Decreased by 2 when expired
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == -1);
            Assert.IsTrue(top.Quality == 3);

            // Decreased by 4 when expired
            Assert.IsTrue(bs.OriginalExpiresIn == 1);
            Assert.IsTrue(bs.ExpiresIn == -1);
            Assert.IsTrue(bs.Quality == 0);
        }

        /// <summary>
        /// Covers Blue Star and a Top Connected Providers award with the same values
        /// Execute an UpdateQuality and make sure Quality decreases by 2 when not expired
        /// Execute an UpdateQuality and make sure Quality decreases by 4 when expired
        /// If less than min value, set to min value
        /// </summary>
        [TestMethod]
        public void Test_BlueStar_UpdateQuality_MinQuality()
        {
            var top = AllAwards["Top Connected Providers"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 1);
            var bs = AllAwards["Blue Star"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 1);

            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == 1);
            Assert.IsTrue(top.Quality == 1);
            Assert.IsNotNull(bs);
            Assert.IsTrue(bs.Name == "Blue Star");
            Assert.IsTrue(bs.OriginalExpiresIn == 1);
            Assert.IsTrue(bs.ExpiresIn == 1);
            Assert.IsTrue(bs.Quality == 1);

            top.UpdateQuality();
            bs.UpdateQuality();

            // Unexpired
            // Decreased by 1 when not expired
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == 0);
            Assert.IsTrue(top.Quality == 0);

            // Decreased by 2 when not expired, if less than min value, set to min value
            Assert.IsTrue(bs.OriginalExpiresIn == 1);
            Assert.IsTrue(bs.ExpiresIn == 0);
            Assert.IsTrue(bs.Quality == 0);

            top.UpdateQuality();
            bs.UpdateQuality();

            // Expired
            // Decreased by 2 when expired, if less than min value, set to min value
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == -1);
            Assert.IsTrue(top.Quality == 0);

            // Decreased by 4 when expired, if less than min value, set to min value
            Assert.IsTrue(bs.OriginalExpiresIn == 1);
            Assert.IsTrue(bs.ExpiresIn == -1);
            Assert.IsTrue(bs.Quality == 0);
        }

        #endregion //END - Blue Star

        #region Blue Compare

        /// <summary>
        /// Covers Blue Compare
        /// Execute an UpdateQuality and make sure Quality increases by 1 when not expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_UpdateQuality_Unexpired()
        {
            var bc = AllAwards["Blue Compare"].Find(x => x.OriginalExpiresIn == 11 && x.Quality == 20);

            Assert.IsNotNull(bc);
            Assert.IsTrue(bc.Name == "Blue Compare");
            Assert.IsTrue(bc.OriginalExpiresIn == 11);
            Assert.IsTrue(bc.ExpiresIn == 11);
            Assert.IsTrue(bc.Quality == 20);

            bc.UpdateQuality();

            // Increased by 1 when not expired
            Assert.IsTrue(bc.OriginalExpiresIn == 11);
            Assert.IsTrue(bc.ExpiresIn == 10);
            Assert.IsTrue(bc.Quality == 21);
        }

        /// <summary>
        /// Covers Blue Compare
        /// Execute an UpdateQuality and make sure Quality drops to 0 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_UpdateQuality_Expired()
        {
            var bc = AllAwards["Blue Compare"].Find(x => x.OriginalExpiresIn == 0 && x.Quality == 20);

            Assert.IsNotNull(bc);
            Assert.IsTrue(bc.Name == "Blue Compare");
            Assert.IsTrue(bc.OriginalExpiresIn == 0);
            Assert.IsTrue(bc.ExpiresIn == 0);
            Assert.IsTrue(bc.Quality == 20);

            bc.UpdateQuality();

            // Decreased to 0 when expired
            Assert.IsTrue(bc.OriginalExpiresIn == 0);
            Assert.IsTrue(bc.ExpiresIn == -1);
            Assert.IsTrue(bc.Quality == 0);
        }

        /// <summary>
        /// Covers Blue Compare
        /// Execute an UpdateQuality and make sure Quality increases by 3 the day before the award expires
        /// Execute an UpdateQuality and make sure Quality drops to 0 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_UpdateQuality_UnexpiredExpired()
        {
            var bc = AllAwards["Blue Compare"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 20);

            Assert.IsNotNull(bc);
            Assert.IsTrue(bc.Name == "Blue Compare");
            Assert.IsTrue(bc.OriginalExpiresIn == 1);
            Assert.IsTrue(bc.ExpiresIn == 1);
            Assert.IsTrue(bc.Quality == 20);

            bc.UpdateQuality();

            //Unexpired
            // Increased by 3 on the day the award expires
            Assert.IsTrue(bc.OriginalExpiresIn == 1);
            Assert.IsTrue(bc.ExpiresIn == 0);
            Assert.IsTrue(bc.Quality == 23);

            bc.UpdateQuality();

            //Expired
            // Decreased to 0 when expired
            Assert.IsTrue(bc.OriginalExpiresIn == 1);
            Assert.IsTrue(bc.ExpiresIn == -1);
            Assert.IsTrue(bc.Quality == 0);
        }

        /// <summary>
        /// Covers Blue Compare
        /// Execute an UpdateQuality and make sure Quality does not exceed 50 when not expired
        /// Execute an UpdateQuality and make sure Quality drops to 0 when expired
        /// Execute an UpdateQuality and make sure Quality remains 0 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_UpdateQuality_MaxMinQuality()
        {
            var bc = AllAwards["Blue Compare"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 50);

            Assert.IsNotNull(bc);
            Assert.IsTrue(bc.Name == "Blue Compare");
            Assert.IsTrue(bc.OriginalExpiresIn == 1);
            Assert.IsTrue(bc.ExpiresIn == 1);
            Assert.IsTrue(bc.Quality == 50);

            bc.UpdateQuality();

            //Unexpired
            Assert.IsTrue(bc.OriginalExpiresIn == 1);
            Assert.IsTrue(bc.ExpiresIn == 0);
            // Cannot exceed 50
            Assert.IsTrue(bc.Quality == 50);

            bc.UpdateQuality();

            //Expired
            Assert.IsTrue(bc.OriginalExpiresIn == 1);
            Assert.IsTrue(bc.ExpiresIn == -1);
            // Drops to 0 after it expires
            Assert.IsTrue(bc.Quality == 0);

            bc.UpdateQuality();

            //Expired
            Assert.IsTrue(bc.OriginalExpiresIn == 1);
            Assert.IsTrue(bc.ExpiresIn == -2);
            // Remains 0 after it expires
            Assert.IsTrue(bc.Quality == 0);
        }

        #endregion //END - Blue Compare

        #region Blue Distinction Plus

        /// <summary>
        /// Covers Blue Distinction Plus
        /// Execute an UpdateQuality and make sure Quality remains 80
        /// </summary>
        [TestMethod]
        public void TestImmutabilityOfBlueDistinctionPlus()
        {
            var awards = AllAwards["Blue Distinction Plus"];
            int count = awards.Count;
            for (int idx = 0; idx < count; idx++)
            {
                Assert.IsTrue(awards[idx].Quality == 80);
                Assert.IsTrue(awards[idx].OriginalExpiresIn == awards[idx].ExpiresIn);

                awards[idx].UpdateQuality();

                // Never Expires
                Assert.IsTrue(awards[idx].OriginalExpiresIn == awards[idx].ExpiresIn);
                // Never Changes
                Assert.IsTrue(awards[idx].Quality == 80);
            }
        }

        /// <summary>
        /// Covers Blue Distinction Plus
        /// Execute an UpdateQuality and make sure Quality remains 80 when not expired
        /// </summary>
        [TestMethod]
        public void Test_BlueDistinctionPlus_UpdateQuality_Unexpired()
        {
            var bdp = AllAwards["Blue Distinction Plus"].Find(x => x.OriginalExpiresIn == 5 && x.Quality == 80);

            Assert.IsNotNull(bdp);
            Assert.IsTrue(bdp.Name == "Blue Distinction Plus");
            Assert.IsTrue(bdp.OriginalExpiresIn == 5);
            Assert.IsTrue(bdp.ExpiresIn == 5);
            Assert.IsTrue(bdp.Quality == 80);

            bdp.UpdateQuality();

            // Never Expires
            Assert.IsTrue(bdp.OriginalExpiresIn == 5);
            Assert.IsTrue(bdp.ExpiresIn == 5);
            Assert.IsTrue(bdp.Quality == 80);
        }

        /// <summary>
        /// Covers Blue Distinction Plus
        /// Execute an UpdateQuality and make sure Quality remains at 80 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueDistinctionPlus_UpdateQuality_Expired()
        {
            var bdp = AllAwards["Blue Distinction Plus"].Find(x => x.OriginalExpiresIn == -1 && x.Quality == 80);

            Assert.IsNotNull(bdp);
            Assert.IsTrue(bdp.Name == "Blue Distinction Plus");
            Assert.IsTrue(bdp.OriginalExpiresIn == -1);
            Assert.IsTrue(bdp.ExpiresIn == -1);
            Assert.IsTrue(bdp.Quality == 80);

            bdp.UpdateQuality();

            // Never Expires
            Assert.IsTrue(bdp.OriginalExpiresIn == -1);
            Assert.IsTrue(bdp.ExpiresIn == -1);
            Assert.IsTrue(bdp.Quality == 80);
        }

        /// <summary>
        /// Covers Blue Distinction Plus
        /// Execute an UpdateQuality and make sure Quality remains 80 when not expired
        /// Execute an UpdateQuality and make sure Quality remains 80 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueDistinctionPlus_UpdateQuality_UnexpiredExpired()
        {
            var bdp = AllAwards["Blue Distinction Plus"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 80);

            Assert.IsNotNull(bdp);
            Assert.IsTrue(bdp.Name == "Blue Distinction Plus");
            Assert.IsTrue(bdp.OriginalExpiresIn == 1);
            Assert.IsTrue(bdp.ExpiresIn == 1);
            Assert.IsTrue(bdp.Quality == 80);

            bdp.UpdateQuality();

            //Never Expires
            Assert.IsTrue(bdp.OriginalExpiresIn == 1);
            Assert.IsTrue(bdp.ExpiresIn == 1);
            Assert.IsTrue(bdp.Quality == 80);

            bdp.UpdateQuality();

            //Never Expires
            Assert.IsTrue(bdp.OriginalExpiresIn == 1);
            Assert.IsTrue(bdp.ExpiresIn == 1);
            Assert.IsTrue(bdp.Quality == 80);
        }

        #endregion //END - Blue Distinction Plus

        #region Blue First

        /// <summary>
        /// Covers Blue First
        /// Execute an UpdateQuality and make sure Quality increases by 1 when not expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_UpdateQuality_Unexpired()
        {
            var bf = AllAwards["Blue First"].Find(x => x.OriginalExpiresIn == 2 && x.Quality == 0);

            Assert.IsNotNull(bf);
            Assert.IsTrue(bf.Name == "Blue First");
            Assert.IsTrue(bf.OriginalExpiresIn == 2);
            Assert.IsTrue(bf.ExpiresIn == 2);
            Assert.IsTrue(bf.Quality == 0);

            bf.UpdateQuality();

            // Unexpired
            // Increased by 1 when not expired
            Assert.IsTrue(bf.OriginalExpiresIn == 2);
            Assert.IsTrue(bf.ExpiresIn == 1);
            Assert.IsTrue(bf.Quality == 1);
        }

        /// <summary>
        /// Covers Blue First
        /// Execute an UpdateQuality and make sure Quality increases by 2 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_UpdateQuality_Expired()
        {
            var bf = AllAwards["Blue First"].Find(x => x.OriginalExpiresIn == 0 && x.Quality == 10);

            Assert.IsNotNull(bf);
            Assert.IsTrue(bf.Name == "Blue First");
            Assert.IsTrue(bf.OriginalExpiresIn == 0);
            Assert.IsTrue(bf.ExpiresIn == 0);
            Assert.IsTrue(bf.Quality == 10);

            bf.UpdateQuality();

            // Expired
            // Increased by 2 when not expired
            Assert.IsTrue(bf.OriginalExpiresIn == 0);
            Assert.IsTrue(bf.ExpiresIn == -1);
            Assert.IsTrue(bf.Quality == 12);
        }

        /// <summary>
        /// Covers Blue First
        /// Execute an UpdateQuality and make sure Quality increases by 1 when not expired
        /// Execute an UpdateQuality and make sure Quality increases by 2 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_UpdateQuality_UnexpiredExpired()
        {
            var bf = AllAwards["Blue First"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 10);

            Assert.IsNotNull(bf);
            Assert.IsTrue(bf.Name == "Blue First");
            Assert.IsTrue(bf.OriginalExpiresIn == 1);
            Assert.IsTrue(bf.ExpiresIn == 1);
            Assert.IsTrue(bf.Quality == 10);

            bf.UpdateQuality();

            // Unexpired
            // Increased by 1 when not expired
            Assert.IsTrue(bf.OriginalExpiresIn == 1);
            Assert.IsTrue(bf.ExpiresIn == 0);
            Assert.IsTrue(bf.Quality == 11);

            bf.UpdateQuality();

            // Expired
            // Increased by 2 when not expired
            Assert.IsTrue(bf.OriginalExpiresIn == 1);
            Assert.IsTrue(bf.ExpiresIn == -1);
            Assert.IsTrue(bf.Quality == 13);
        }

        /// <summary>
        /// Covers Blue First
        /// Execute an UpdateQuality and make sure Quality increases by 2 the day it expires
        /// Execute an UpdateQuality and make sure Quality increases by 1 when expired to max of 50
        /// Execute an UpdateQuality and make sure Quality does not exceed 50 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_UpdateQuality_MaxQuality()
        {
            var bf = AllAwards["Blue First"].Find(x => x.OriginalExpiresIn == 0 && x.Quality == 47);

            Assert.IsNotNull(bf);
            Assert.IsTrue(bf.Name == "Blue First");
            Assert.IsTrue(bf.OriginalExpiresIn == 0);
            Assert.IsTrue(bf.ExpiresIn == 0);
            Assert.IsTrue(bf.Quality == 47);

            bf.UpdateQuality();

            // Expired
            // Increased by 2 on the day it expires
            Assert.IsTrue(bf.OriginalExpiresIn == 0);
            Assert.IsTrue(bf.ExpiresIn == -1);
            Assert.IsTrue(bf.Quality == 49);

            bf.UpdateQuality();

            // Expired
            // Increased by 1 when expired
            // Now at max value
            // Cannot exceed 50
            Assert.IsTrue(bf.OriginalExpiresIn == 0);
            Assert.IsTrue(bf.ExpiresIn == -2);
            Assert.IsTrue(bf.Quality == 50);

            bf.UpdateQuality();

            // Expired and at Max
            // Cannot exceed 50
            Assert.IsTrue(bf.OriginalExpiresIn == 0);
            Assert.IsTrue(bf.ExpiresIn == -3);
            Assert.IsTrue(bf.Quality == 50);
        }

        #endregion //END - Blue First

        #region Generic Awards

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// Execute an UpdateQuality and make sure Quality decreases by 1 when not expired
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_UpdateQuality_Unexpired()
        {
            var acme = AllAwards["ACME Partner Facility"].Find(x => x.OriginalExpiresIn == 5 && x.Quality == 7);
            var gov = AllAwards["Gov Quality Plus"].Find(x => x.OriginalExpiresIn == 10 && x.Quality == 20);
            var top = AllAwards["Top Connected Providers"].Find(x => x.OriginalExpiresIn == 3 && x.Quality == 6);

            Assert.IsNotNull(acme);
            Assert.IsTrue(acme.Name == "ACME Partner Facility");
            Assert.IsTrue(acme.OriginalExpiresIn == 5);
            Assert.IsTrue(acme.ExpiresIn == 5);
            Assert.IsTrue(acme.Quality == 7);

            Assert.IsNotNull(gov);
            Assert.IsTrue(gov.Name == "Gov Quality Plus");
            Assert.IsTrue(gov.OriginalExpiresIn == 10);
            Assert.IsTrue(gov.ExpiresIn == 10);
            Assert.IsTrue(gov.Quality == 20);

            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.OriginalExpiresIn == 3);
            Assert.IsTrue(top.ExpiresIn == 3);
            Assert.IsTrue(top.Quality == 6);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Decreased by 1 when not expired
            Assert.IsTrue(acme.OriginalExpiresIn == 5);
            Assert.IsTrue(acme.ExpiresIn == 4);
            Assert.IsTrue(acme.Quality == 6);

            // Decreased by 1 when not expired
            Assert.IsTrue(gov.OriginalExpiresIn == 10);
            Assert.IsTrue(gov.ExpiresIn == 9);
            Assert.IsTrue(gov.Quality == 19);

            // Decreased by 1 when not expired
            Assert.IsTrue(top.OriginalExpiresIn == 3);
            Assert.IsTrue(top.ExpiresIn == 2);
            Assert.IsTrue(top.Quality == 5);
        }

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// Execute an UpdateQuality and make sure Quality decreases by 2 when expired
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_UpdateQuality_Expired()
        {
            var acme = AllAwards["ACME Partner Facility"].Find(x => x.OriginalExpiresIn == 0 && x.Quality == 7);
            var gov = AllAwards["Gov Quality Plus"].Find(x => x.OriginalExpiresIn == 0 && x.Quality == 20);
            var top = AllAwards["Top Connected Providers"].Find(x => x.OriginalExpiresIn == 0 && x.Quality == 6);

            Assert.IsNotNull(acme);
            Assert.IsTrue(acme.Name == "ACME Partner Facility");
            Assert.IsTrue(acme.OriginalExpiresIn == 0);
            Assert.IsTrue(acme.ExpiresIn == 0);
            Assert.IsTrue(acme.Quality == 7);

            Assert.IsNotNull(gov);
            Assert.IsTrue(gov.Name == "Gov Quality Plus");
            Assert.IsTrue(gov.OriginalExpiresIn == 0);
            Assert.IsTrue(gov.ExpiresIn == 0);
            Assert.IsTrue(gov.Quality == 20);

            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.OriginalExpiresIn == 0);
            Assert.IsTrue(top.ExpiresIn == 0);
            Assert.IsTrue(top.Quality == 6);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Decreased by 2 when expired
            Assert.IsTrue(acme.OriginalExpiresIn == 0);
            Assert.IsTrue(acme.ExpiresIn == -1);
            Assert.IsTrue(acme.Quality == 5);

            // Decreased by 2 when expired
            Assert.IsTrue(gov.OriginalExpiresIn == 0);
            Assert.IsTrue(gov.ExpiresIn == -1);
            Assert.IsTrue(gov.Quality == 18);

            // Decreased by 2 when expired
            Assert.IsTrue(top.OriginalExpiresIn == 0);
            Assert.IsTrue(top.ExpiresIn == -1);
            Assert.IsTrue(top.Quality == 4);
        }

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// Execute an UpdateQuality and make sure Quality decreases by 1 when not expired
        /// Execute an UpdateQuality and make sure Quality decreases by 2 when expired
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_UpdateQuality_UnexpiredExpired()
        {
            var acme = AllAwards["ACME Partner Facility"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 7);
            var gov = AllAwards["Gov Quality Plus"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 20);
            var top = AllAwards["Top Connected Providers"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 6);

            Assert.IsNotNull(acme);
            Assert.IsTrue(acme.Name == "ACME Partner Facility");
            Assert.IsTrue(acme.OriginalExpiresIn == 1);
            Assert.IsTrue(acme.ExpiresIn == 1);
            Assert.IsTrue(acme.Quality == 7);

            Assert.IsNotNull(gov);
            Assert.IsTrue(gov.Name == "Gov Quality Plus");
            Assert.IsTrue(gov.OriginalExpiresIn == 1);
            Assert.IsTrue(gov.ExpiresIn == 1);
            Assert.IsTrue(gov.Quality == 20);

            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == 1);
            Assert.IsTrue(top.Quality == 6);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Unexpired
            // Decreased by 1 when not expired
            Assert.IsTrue(acme.OriginalExpiresIn == 1);
            Assert.IsTrue(acme.ExpiresIn == 0);
            Assert.IsTrue(acme.Quality == 6);

            // Decreased by 1 when not expired
            Assert.IsTrue(gov.OriginalExpiresIn == 1);
            Assert.IsTrue(gov.ExpiresIn == 0);
            Assert.IsTrue(gov.Quality == 19);

            // Decreased by 1 when not expired
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == 0);
            Assert.IsTrue(top.Quality == 5);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Expired
            // Decreased by 2 when expired
            Assert.IsTrue(acme.OriginalExpiresIn == 1);
            Assert.IsTrue(acme.ExpiresIn == -1);
            Assert.IsTrue(acme.Quality == 4);

            // Decreased by 2 when expired
            Assert.IsTrue(gov.OriginalExpiresIn == 1);
            Assert.IsTrue(gov.ExpiresIn == -1);
            Assert.IsTrue(gov.Quality == 17);

            // Decreased by 2 when expired
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == -1);
            Assert.IsTrue(top.Quality == 3);
        }

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// Execute an UpdateQuality and make sure Quality decreases by 1 when not expired
        /// Execute an UpdateQuality and make sure Quality decreases by 2 when expired
        /// If less than min value, set to min value
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_UpdateQuality_MinQuality()
        {
            var acme = AllAwards["ACME Partner Facility"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 3);
            var gov = AllAwards["Gov Quality Plus"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 3);
            var top = AllAwards["Top Connected Providers"].Find(x => x.OriginalExpiresIn == 1 && x.Quality == 3);

            Assert.IsNotNull(acme);
            Assert.IsTrue(acme.Name == "ACME Partner Facility");
            Assert.IsTrue(acme.OriginalExpiresIn == 1);
            Assert.IsTrue(acme.ExpiresIn == 1);
            Assert.IsTrue(acme.Quality == 3);

            Assert.IsNotNull(gov);
            Assert.IsTrue(gov.Name == "Gov Quality Plus");
            Assert.IsTrue(gov.OriginalExpiresIn == 1);
            Assert.IsTrue(gov.ExpiresIn == 1);
            Assert.IsTrue(gov.Quality == 3);

            Assert.IsNotNull(top);
            Assert.IsTrue(top.Name == "Top Connected Providers");
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == 1);
            Assert.IsTrue(top.Quality == 3);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Unexpired
            // Decreased by 1 when not expired
            Assert.IsTrue(acme.OriginalExpiresIn == 1);
            Assert.IsTrue(acme.ExpiresIn == 0);
            Assert.IsTrue(acme.Quality == 2);

            // Decreased by 1 when not expired
            Assert.IsTrue(gov.OriginalExpiresIn == 1);
            Assert.IsTrue(gov.ExpiresIn == 0);
            Assert.IsTrue(gov.Quality == 2);

            // Decreased by 1 when not expired
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == 0);
            Assert.IsTrue(top.Quality == 2);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Expired
            // Decreased by 2 when expired
            Assert.IsTrue(acme.OriginalExpiresIn == 1);
            Assert.IsTrue(acme.ExpiresIn == -1);
            Assert.IsTrue(acme.Quality == 0);

            // Decreased by 2 when expired
            Assert.IsTrue(gov.OriginalExpiresIn == 1);
            Assert.IsTrue(gov.ExpiresIn == -1);
            Assert.IsTrue(gov.Quality == 0);

            // Decreased by 2 when expired
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == -1);
            Assert.IsTrue(top.Quality == 0);

            acme.UpdateQuality();
            gov.UpdateQuality();
            top.UpdateQuality();

            // Expired
            // Decreased by 2 when expired, if less than min value, set to min value
            Assert.IsTrue(acme.OriginalExpiresIn == 1);
            Assert.IsTrue(acme.ExpiresIn == -2);
            Assert.IsTrue(acme.Quality == 0);

            // Decreased by 2 when expired, if less than min value, set to min value
            Assert.IsTrue(gov.OriginalExpiresIn == 1);
            Assert.IsTrue(gov.ExpiresIn == -2);
            Assert.IsTrue(gov.Quality == 0);

            // Decreased by 2 when expired, if less than min value, set to min value
            Assert.IsTrue(top.OriginalExpiresIn == 1);
            Assert.IsTrue(top.ExpiresIn == -2);
            Assert.IsTrue(top.Quality == 0);
        }

        #endregion //END - Generic Awards

        #endregion //END - Set of tests for new Award.UpdateQuality refactor, matching the entire suite of tests, 1 for 1, from UpdateQualityAwardsTests

    }
}
