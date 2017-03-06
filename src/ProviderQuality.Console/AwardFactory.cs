using System;

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
                            Quality = quality,
                            ExpiresIn = expiresIn,
                            OriginalQuality = quality,
                            OriginalExpiresIn = expiresIn
                        };
                        break;
                    case "blue compare":
                        createdAward = new BlueCompareAward()
                        {
                            Name = awardName,
                            Quality = quality,
                            ExpiresIn = expiresIn,
                            OriginalQuality = quality,
                            OriginalExpiresIn = expiresIn
                        };
                        break;
                    case "blue star":
                        createdAward = new BlueStarAward()
                        {
                            Name = awardName,
                            Quality = quality,
                            ExpiresIn = expiresIn,
                            OriginalQuality = quality,
                            OriginalExpiresIn = expiresIn
                        };
                        break;
                    case "blue distinction plus":
                        createdAward = new BlueDistinctionPlusAward()
                        {
                            Name = awardName,
                            Quality = BlueDistinctionPlusAward._BlueDistinctionPlusQuality,
                            ExpiresIn = expiresIn,
                            OriginalQuality = quality,
                            OriginalExpiresIn = expiresIn
                        };
                        break;
                    case "blue first":
                        createdAward = new BlueFirstAward()
                        {
                            Name = awardName,
                            Quality = quality,
                            ExpiresIn = expiresIn,
                            OriginalQuality = quality,
                            OriginalExpiresIn = expiresIn
                        };
                        break;
                    default:
                        createdAward = new Award()
                        {
                            Name = "Unknown",
                            Quality = quality,
                            ExpiresIn = expiresIn,
                            OriginalQuality = quality,
                            OriginalExpiresIn = expiresIn
                        };
                        break;
                }

                // Make sure thew new Award is valid
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
