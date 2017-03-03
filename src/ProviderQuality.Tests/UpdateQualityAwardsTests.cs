using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProviderQuality.Console;

namespace ProviderQuality.Tests
{
    [TestClass]
    public class UpdateQualityAwardsTests
    {
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

        //Types of Awards
        //Blue First
        //Blue Compare
        //Blue Distinction Plus
        //Gov Quality Plus
        //ACME Partner Facility
        //Top Connected Providers

        /// <summary>
        /// Covers Blue Compare, Blue Distinction Plus and Blue First
        /// </summary>
        [TestMethod]
        public void TestAllBlueAwardsQualityUpdateUnexpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Compare", ExpiresIn = 15, Quality = 20},
                    new Award {Name = "Blue Distinction Plus", ExpiresIn = 5, Quality = 80},
                    new Award {Name = "Blue First", ExpiresIn = 2, Quality = 0}
                }
            };

            Assert.IsTrue(app.Awards.Count == 3);
            Assert.IsTrue(app.Awards[0].Name == "Blue Compare");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 15);
            Assert.IsTrue(app.Awards[0].Quality == 20);
            Assert.IsTrue(app.Awards[1].Name == "Blue Distinction Plus");
            Assert.IsTrue(app.Awards[1].ExpiresIn == 5);
            Assert.IsTrue(app.Awards[1].Quality == 80);
            Assert.IsTrue(app.Awards[2].Name == "Blue First");
            Assert.IsTrue(app.Awards[2].ExpiresIn == 2);
            Assert.IsTrue(app.Awards[2].Quality == 0);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 21);
            Assert.IsTrue(app.Awards[1].Quality == 80);
            Assert.IsTrue(app.Awards[2].Quality == 1);
        }

        /// <summary>
        /// Covers Blue Compare, Blue Distinction Plus and Blue First
        /// </summary>
        [TestMethod]
        public void TestAllBlueAwardsQualityUpdateExpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Compare", ExpiresIn = 0, Quality = 20},
                    new Award {Name = "Blue Distinction Plus", ExpiresIn = 0, Quality = 80},
                    new Award {Name = "Blue First", ExpiresIn = 0, Quality = 10}
                }
            };

            Assert.IsTrue(app.Awards.Count == 3);
            Assert.IsTrue(app.Awards[0].Name == "Blue Compare");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[0].Quality == 20);
            Assert.IsTrue(app.Awards[1].Name == "Blue Distinction Plus");
            Assert.IsTrue(app.Awards[1].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[1].Quality == 80);
            Assert.IsTrue(app.Awards[2].Name == "Blue First");
            Assert.IsTrue(app.Awards[2].ExpiresIn == 0);
            Assert.IsTrue(app.Awards[2].Quality == 10);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 0);
            Assert.IsTrue(app.Awards[1].Quality == 80);
            Assert.IsTrue(app.Awards[2].Quality == 12);
        }

        /// <summary>
        /// Covers Blue Compare, Blue Distinction Plus and Blue First
        /// </summary>
        [TestMethod]
        public void TestAllBlueAwardsQualityUpdateUnexpiredExpired()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Compare", ExpiresIn = 1, Quality = 20},
                    new Award {Name = "Blue Distinction Plus", ExpiresIn = 1, Quality = 80},
                    new Award {Name = "Blue First", ExpiresIn = 1, Quality = 10}
                }
            };

            Assert.IsTrue(app.Awards.Count == 3);
            Assert.IsTrue(app.Awards[0].Name == "Blue Compare");
            Assert.IsTrue(app.Awards[0].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[0].Quality == 20);
            Assert.IsTrue(app.Awards[1].Name == "Blue Distinction Plus");
            Assert.IsTrue(app.Awards[1].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[1].Quality == 80);
            Assert.IsTrue(app.Awards[2].Name == "Blue First");
            Assert.IsTrue(app.Awards[2].ExpiresIn == 1);
            Assert.IsTrue(app.Awards[2].Quality == 10);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 23);
            Assert.IsTrue(app.Awards[1].Quality == 80);
            Assert.IsTrue(app.Awards[2].Quality == 11);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 0);
            Assert.IsTrue(app.Awards[1].Quality == 80);
            Assert.IsTrue(app.Awards[2].Quality == 13);
        }

        /// <summary>
        /// Covers Blue Compare
        /// </summary>
        [TestMethod]
        public void TestBlueCompareAwardQualityUpdateMaxMinQuality()
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
        }

        /// <summary>
        /// Covers Blue First
        /// </summary>
        [TestMethod]
        public void TestBlueFirstAwardQualityUpdateMaxQuality()
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

        /// <summary>
        /// Covers Gov Quality Plus, ACME Partner Facility and Top Connected Providers
        /// These Awards all have the same business rules for UpdateQuality
        /// </summary>
        [TestMethod]
        public void TestGenericAwardQualityUpdateUnexpired()
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
        public void TestGenericAwardQualityUpdateExpired()
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
        public void TestGenericAwardQualityUpdateUnexpiredExpired()
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
        public void TestGenericAwardQualityUpdateMinQuality()
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

    }
}
