using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderQuality.Console
{
    public static class AwardFactory
    {
        /// <summary>
        /// Decides which class to instantiate.
        /// </summary>
        public static Award Get(string awardName, int expiresIn, int quality)
        {
            switch (awardName)
            {
                case "ACME Partner Facility":
                    return new ACMEPartnerFacilityAward(expiresIn,quality);
                case "Blue Compare":
                    return new BlueCompareAward(expiresIn, quality);
                case "Blue Distinction Plus":
                    return new BlueDistinctionPlusAward(expiresIn, quality);
                case "Blue First":
                    return new BlueFirstAward(expiresIn, quality);
                case "Gov Quality Plus":
                    return new GovQualityPlusAward(expiresIn, quality);
                case "Top Connected Providers":
                    return new TopConnectedProvidersAward(expiresIn, quality);
                default:
                    return new Award("Unknown", 0, 0);
            }
        }
    }
}
