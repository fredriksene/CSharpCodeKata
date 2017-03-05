using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProviderQuality.Console;

namespace ProviderQuality.Tests
{
    [TestClass]
    public class UpdateQualityAwardsTests
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestImmutabilityOfBlueDistinctionPlus()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Distinction Plus", ExpiresIn = 0, Quality = 80}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue Distinction Plus");
            Assert.IsTrue(app.Awards[0].Quality == 80);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 80);
        }

        // +++To Do - 1/10/2013: Discuss with team about adding more tests.  Seems like a lot of work for something
        //                       that probably won't change.  I watched it all in the debugger and know everything works
        //                       plus QA has already signed off and no one has complained.

        #region Blue Compare

        /// <summary>
        /// Covers Blue Compare
        /// Execute a QualityUpdate and make sure Quality increases by 1 when not expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_AppQualityUpdate_Unexpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Compare", ExpiresIn = 15, Quality = 20}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue Compare");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 15);
            Assert.IsTrue(app.Awards[0].Quality == 20);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 21);
        }

        /// <summary>
        /// Covers Blue Compare
        /// Execute a QualityUpdate and make sure Quality drops to 0 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_AppQualityUpdate_Expired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Compare", ExpiresIn = 0, Quality = 20}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue Compare");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[0].Quality == 20);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 0);
        }

        /// <summary>
        /// Covers Blue Compare
        /// Execute a QualityUpdate and make sure Quality increases by 3 the day before the award expires
        /// Execute a QualityUpdate and make sure Quality drops to 0 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_AppQualityUpdate_UnexpiredExpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Compare", ExpiresIn = 1, Quality = 20}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue Compare");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[0].Quality == 20);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 23);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 0);
        }

        /// <summary>
        /// Covers Blue Compare
        /// Execute a QualityUpdate and make sure Quality does not exceed 50 when not expired
        /// Execute a QualityUpdate and make sure Quality drops to 0 when expired
        /// Execute a QualityUpdate and make sure Quality remains 0 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueCompare_AppQualityUpdate_MaxMinQuality()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Compare", ExpiresIn = 1, Quality = 50}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue Compare");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[0].Quality == 50);

            app.UpdateQuality();

            // Cannot exceed 50
            Assert.IsTrue(app.Awards[0].Quality == 50);

            app.UpdateQuality();

            // Drops to 0 after it expires
            Assert.IsTrue(app.Awards[0].Quality == 0);

            app.UpdateQuality();

            // Remains 0 after it expires
            Assert.IsTrue(app.Awards[0].Quality == 0);
        }

        #endregion //END - Blue Compare

        #region Blue Distinction Plus

        /// <summary>
        /// Covers Blue Distinction Plus
        /// Execute a QualityUpdate and make sure Quality remains 80 when not expired
        /// </summary>
        [TestMethod]
        public void Test_BlueDistinctionPlus_AppQualityUpdate_Unexpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Distinction Plus", ExpiresIn = 5, Quality = 80}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue Distinction Plus");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 5);
            Assert.IsTrue(app.Awards[0].Quality == 80);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 80);
        }

        /// <summary>
        /// Covers Blue Distinction Plus
        /// Execute a QualityUpdate and make sure Quality remains at 80 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueDistinctionPlus_AppQualityUpdate_Expired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Distinction Plus", ExpiresIn = 0, Quality = 80}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue Distinction Plus");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[0].Quality == 80);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 80);
        }

        /// <summary>
        /// Covers Blue Distinction Plus
        /// Execute a QualityUpdate and make sure Quality remains 80 when not expired
        /// Execute a QualityUpdate and make sure Quality remains 80 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueDistinctionPlus_AppQualityUpdate_UnexpiredExpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Distinction Plus", ExpiresIn = 1, Quality = 80}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue Distinction Plus");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[0].Quality == 80);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 80);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 80);
        }

        #endregion //END - Blue Distinction Plus

        #region Blue First

        /// <summary>
        /// Covers Blue First
        /// Execute a QualityUpdate and make sure Quality increases by 1 when not expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_AppQualityUpdate_Unexpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue First", ExpiresIn = 2, Quality = 0}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue First");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 2);
            Assert.IsTrue(app.Awards[0].Quality == 0);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 1);
        }

        /// <summary>
        /// Covers Blue First
        /// Execute a QualityUpdate and make sure Quality increases by 2 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_AppQualityUpdate_Expired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue First", ExpiresIn = 0, Quality = 10}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue First");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[0].Quality == 10);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 12);
        }

        /// <summary>
        /// Covers Blue First
        /// Execute a QualityUpdate and make sure Quality increases by 1 when not expired
        /// Execute a QualityUpdate and make sure Quality increases by 2 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_AppQualityUpdate_UnexpiredExpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue First", ExpiresIn = 1, Quality = 10}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue First");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[0].Quality == 10);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 11);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 13);
        }

        /// <summary>
        /// Covers Blue First
        /// Execute a QualityUpdate and make sure Quality increases by 2 the day it expires
        /// Execute a QualityUpdate and make sure Quality increases by 1 when expired to max of 50
        /// Execute a QualityUpdate and make sure Quality does not exceed 50 when expired
        /// </summary>
        [TestMethod]
        public void Test_BlueFirst_AppQualityUpdate_MaxQuality()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue First", ExpiresIn = 0, Quality = 47}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue First");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[0].Quality == 47);

            app.UpdateQuality();

            // Now Expired
            Assert.IsTrue(app.Awards[0].Quality == 49);

            app.UpdateQuality();

            // Expired and at Max
            Assert.IsTrue(app.Awards[0].Quality == 50);

            app.UpdateQuality();

            // Cannot exceed 50
            Assert.IsTrue(app.Awards[0].Quality == 50);
        }

        #endregion //END - Blue First

        #region Generic Awards

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_AppQualityUpdate_Unexpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "ACME Partner Facility", ExpiresIn = 5, Quality = 7},
                    new Award {Name = "Gov Quality Plus", ExpiresIn = 10, Quality = 20},
                    new Award {Name = "Top Connected Providers", ExpiresIn = 3, Quality = 6}
                }
            };

            Assert.IsTrue(app.Awards.Count == 3);
            Assert.IsTrue(app.Awards[0].Name == "ACME Partner Facility");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 5);
            Assert.IsTrue(app.Awards[0].Quality == 7);
            Assert.IsTrue(app.Awards[1].Name == "Gov Quality Plus");
            Assert.IsTrue(app.Awards[1].ExpiresIn == 10);
            Assert.IsTrue(app.Awards[1].Quality == 20);
            Assert.IsTrue(app.Awards[2].Name == "Top Connected Providers");
            Assert.IsTrue(app.Awards[2].ExpiresIn == 3);
            Assert.IsTrue(app.Awards[2].Quality == 6);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 6);
            Assert.IsTrue(app.Awards[1].Quality == 19);
            Assert.IsTrue(app.Awards[2].Quality == 5);
        }

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_AppQualityUpdate_Expired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "ACME Partner Facility", ExpiresIn = 0, Quality = 7},
                    new Award {Name = "Gov Quality Plus", ExpiresIn = 0, Quality = 20},
                    new Award {Name = "Top Connected Providers", ExpiresIn = 0, Quality = 6}
                }
            };

            Assert.IsTrue(app.Awards.Count == 3);
            Assert.IsTrue(app.Awards[0].Name == "ACME Partner Facility");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[0].Quality == 7);
            Assert.IsTrue(app.Awards[1].Name == "Gov Quality Plus");
            Assert.IsTrue(app.Awards[1].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[1].Quality == 20);
            Assert.IsTrue(app.Awards[2].Name == "Top Connected Providers");
            Assert.IsTrue(app.Awards[2].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[2].Quality == 6);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 5);
            Assert.IsTrue(app.Awards[1].Quality == 18);
            Assert.IsTrue(app.Awards[2].Quality == 4);
        }

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// These Awards all have the same business rules for UpdateQuality
        /// Covers two calls to UpdateQuality
        ///     the first is when all three awards are unexpired
        ///     the second is when all three awards are expired
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_AppQualityUpdate_UnexpiredExpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "ACME Partner Facility", ExpiresIn = 1, Quality = 7},
                    new Award {Name = "Gov Quality Plus", ExpiresIn = 1, Quality = 20},
                    new Award {Name = "Top Connected Providers", ExpiresIn = 1, Quality = 6}
                }
            };

            Assert.IsTrue(app.Awards.Count == 3);
            Assert.IsTrue(app.Awards[0].Name == "ACME Partner Facility");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[0].Quality == 7);
            Assert.IsTrue(app.Awards[1].Name == "Gov Quality Plus");
            Assert.IsTrue(app.Awards[1].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[1].Quality == 20);
            Assert.IsTrue(app.Awards[2].Name == "Top Connected Providers");
            Assert.IsTrue(app.Awards[2].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[2].Quality == 6);

            app.UpdateQuality();
            // Unexpired
            Assert.IsTrue(app.Awards[0].Quality == 6);
            Assert.IsTrue(app.Awards[1].Quality == 19);
            Assert.IsTrue(app.Awards[2].Quality == 5);

            app.UpdateQuality();
            // Expired
            Assert.IsTrue(app.Awards[0].Quality == 4);
            Assert.IsTrue(app.Awards[1].Quality == 17);
            Assert.IsTrue(app.Awards[2].Quality == 3);
        }

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void Test_GenericAward_AppQualityUpdate_MinQuality()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "ACME Partner Facility", ExpiresIn = 1, Quality = 1},
                    new Award {Name = "Gov Quality Plus", ExpiresIn = 1, Quality = 1},
                    new Award {Name = "Top Connected Providers", ExpiresIn = 1, Quality = 1}
                }
            };

            Assert.IsTrue(app.Awards.Count == 3);
            Assert.IsTrue(app.Awards[0].Name == "ACME Partner Facility");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[0].Quality == 1);
            Assert.IsTrue(app.Awards[1].Name == "Gov Quality Plus");
            Assert.IsTrue(app.Awards[1].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[1].Quality == 1);
            Assert.IsTrue(app.Awards[2].Name == "Top Connected Providers");
            Assert.IsTrue(app.Awards[2].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[2].Quality == 1);

            app.UpdateQuality();
            // Unexpired
            Assert.IsTrue(app.Awards[0].Quality == 0);
            Assert.IsTrue(app.Awards[1].Quality == 0);
            Assert.IsTrue(app.Awards[2].Quality == 0);

            app.UpdateQuality();
            // Expired
            Assert.IsTrue(app.Awards[0].Quality == 0);
            Assert.IsTrue(app.Awards[1].Quality == 0);
            Assert.IsTrue(app.Awards[2].Quality == 0);
        }

        #endregion //END - Generic Awards
    }
}
