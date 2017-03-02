using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderQuality.Console
{
    /// <summary>
    /// Base class for all Awards
    /// </summary>
    public class Award
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Award() { }

        public Award(string name, int expiresIn, int quality)
        {
            //Validate(name, expiresIn, quality);

            Name = name;
            ExpiresIn = expiresIn;
            Quality = quality;
        }

        /// <summary>
        /// Validate all the inputs for creating a new Award
        /// </summary>
        /// <param name="name"></param>
        /// <param name="expiresIn"></param>
        /// <param name="quality"></param>
        public virtual void Validate(string name, int expiresIn, int quality)
        {
            ValidateName(name);

            ValidateExpiresIn(expiresIn);

            ValidateQuality(quality);
        }

        /// <summary>
        /// Validate the quality parameter
        /// </summary>
        /// <param name="quality"></param>
        public virtual void ValidateQuality(int quality)
        {
            if (quality < 0)
            {
                throw new ArgumentException("quality must be >= 0");
            }

            if (quality > 50)
            {
                throw new ArgumentException("quality must be < 50");
            }
        }

        /// <summary>
        /// Validate the expiresIn parameter
        /// </summary>
        /// <param name="expiresIn"></param>
        public virtual void ValidateExpiresIn(int expiresIn)
        {
            if (expiresIn < 0)
            {
                throw new ArgumentException("expiresIn must be >= 0");
            }
        }

        /// <summary>
        /// Validate the name parameter
        /// </summary>
        /// <param name="name"></param>
        public virtual void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name cannot be NULL");
                }
                else
                {
                    throw new ArgumentException("name cannot be EMPTY");
                }
            }
        }

        /// <summary>
        /// The Name property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The ExpiresIn property
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// The Quality property
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// The immutable maximum quality value
        /// </summary>
        public const int _MaxQuality = 50;

        /// <summary>
        /// The immutable minimum quality value
        /// </summary>
        public const int _MinQuality = 0;

        /// <summary>
        /// The immutable Blue Distinction Plus quality value
        /// </summary>
        public const int _BlueDistinctionPlusQuality = 80;

        /// <summary>
        /// The Quality of the Award when it was created 
        /// </summary>
        public int OriginalQuality { get; set; }

        /// <summary>
        /// The Quality of the Award for the CurrentDay 
        /// </summary>
        public int CurrentQuality { get; set; }

        /// <summary>
        /// ExpiresIn represents the number of days remaining
        /// CurrentDay is the offset from ExpiresIn
        /// ex. ExpiresIn = 5, CurrentDay will have values ranging from 5 - 0
        /// </summary>
        public int CurrentDay { get; set; }

        /// <summary>
        /// True when CurrentDay is a negative value
        /// </summary>
        public bool IsExpired
        {
            get
            {
                return CurrentDay < 0;
            }
            set { }
        }
    }

    public class BlueCompareAward : Award
    {
        private const string BlueCompare_AwardName = "Blue Compare";

        public BlueCompareAward(int expiresIn, int quality) : base(BlueCompare_AwardName, expiresIn, quality)
        {
            base.Validate(BlueCompare_AwardName, expiresIn, quality);
        }
    }

    public class BlueDistinctionPlusAward : Award
    {
        private const string BlueDistinctionPlus_AwardName = "Blue Distinction Plus";

        public BlueDistinctionPlusAward(int expiresIn, int quality) : base(BlueDistinctionPlus_AwardName, expiresIn, quality)
        {
            base.Validate(BlueDistinctionPlus_AwardName, expiresIn, quality);
        }

        public override void ValidateQuality(int quality)
        {
            if (quality != Award._BlueDistinctionPlusQuality)
            {
                throw new ArgumentException("quality must be == " + Award._BlueDistinctionPlusQuality.ToString());
            }

            //base.ValidateQuality(quality);
        }
    }

    public class BlueFirstAward : Award
    {
        private const string BlueFirst_AwardName = "Blue First";

        public BlueFirstAward(int expiresIn, int quality) : base(BlueFirst_AwardName, expiresIn, quality)
        {
            base.Validate(BlueFirst_AwardName, expiresIn, quality);
        }
    }

    public class ACMEPartnerFacilityAward : Award
    {
        private const string ACMEPartnerFacility_AwardName = "ACME Partner Facility";

        public ACMEPartnerFacilityAward(int expiresIn, int quality) : base(ACMEPartnerFacility_AwardName, expiresIn, quality)
        {
            base.Validate(ACMEPartnerFacility_AwardName, expiresIn, quality);
        }
    }

    public class GovQualityPlusAward : Award
    {
        private const string GovQualityPlus_AwardName = "Gov Quality Plus";

        public GovQualityPlusAward(int expiresIn, int quality) : base(GovQualityPlus_AwardName, expiresIn, quality)
        {
            base.Validate(GovQualityPlus_AwardName, expiresIn, quality);
        }
    }

    public class TopConnectedProvidersAward : Award
    {
        private const string TopConnectedProviders_AwardName = "Top Connected Providers";

        public TopConnectedProvidersAward(int expiresIn, int quality) : base(TopConnectedProviders_AwardName, expiresIn, quality)
        {
            base.Validate(TopConnectedProviders_AwardName, expiresIn, quality);
        }
    }
}
