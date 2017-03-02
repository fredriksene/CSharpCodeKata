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
                for (int quality = 0; quality <= 50; quality++)
                {
                    //AwardFactory.Get()

                }
            }

        }

        [TestCleanup]
        public void TestCleanUp()
        {
            AwardTypes.Clear();
            AwardTypes = null;

            AllAwards.Clear();
            AllAwards = null;
        }

        [TestMethod]
        public void TestAllAwardsValidation()
        {
            Assert.IsNotNull(AllAwards);
            Assert.IsTrue(AllAwards.Keys.Count == 6);
            Assert.IsTrue(AllAwards["ACME Partner Facility"].Count == 50);
            //Assert.IsTrue(app.Awards.Count == 1);
            //Assert.IsTrue(app.Awards[0].Name == "Blue Distinction Plus");
            //Assert.IsTrue(app.Awards[0].Quality == 80);

            //app.UpdateQuality();

            //Assert.IsTrue(app.Awards[0].Quality == 80);
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











    }
}
