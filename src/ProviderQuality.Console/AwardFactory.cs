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
            try
            {
                Award createdAward = null;
                switch (awardName.ToLowerInvariant())
                {
                    case "acme partner facility":
                    case "gov quality plus":
                    case "top connected providers":
                        createdAward = new Award()
                        {
                            Name = awardName,
                            ExpiresIn = expiresIn,
                            Quality = quality,
                            OriginalQuality = quality,
                            CurrentQuality = quality,
                            CurrentDay = expiresIn,
                        };
                        break;
                    case "blue compare":
                        createdAward = new BlueCompareAward()
                        {
                            Name = awardName,
                            ExpiresIn = expiresIn,
                            Quality = quality
                        };
                        break;
                    case "blue distinction plus":
                        createdAward = new BlueDistinctionPlusAward()
                        {
                            Name = awardName,
                            ExpiresIn = -1,
                            Quality = Award._BlueDistinctionPlusQuality
                        };
                        break;
                    case "blue first":
                        createdAward = new BlueFirstAward()
                        {
                            Name = awardName,
                            ExpiresIn = expiresIn,
                            Quality = quality
                        };
                        break;
                    default:
                        createdAward = new Award()
                        {
                            Name = "Unknown",
                            ExpiresIn = expiresIn,
                            Quality = quality
                        };
                        break;
                }
                createdAward.Validate();
                return createdAward;
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Failed to create Award - Name: {0}, ExpiresIn: {1}, Quality: {2}", awardName, expiresIn, quality), ex);
            }
        }
    }
}
