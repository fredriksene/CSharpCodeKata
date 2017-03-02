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

        #region Methods

        /// <summary>
        /// Update the Quality property
        /// </summary>
        public virtual void UpdateQuality()
        {
            //if (Quality > 0)

        }

        /// <summary>
        /// Validate all the inputs for creating a new Award
        /// </summary>
        public virtual void Validate()
        {
            ValidateName();

            ValidateExpiresIn();

            ValidateQuality();
        }

        /// <summary>
        /// Validate the Quality property
        /// </summary>
        public virtual void ValidateQuality()
        {
            if (this.Quality < 0)
            {
                throw new ArgumentException("Quality must be >= 0");
            }

            if (this.Quality > 50)
            {
                throw new ArgumentException("Quality must be < 50");
            }
        }

        /// <summary>
        /// Validate the ExpiresIn property
        /// </summary>
        public virtual void ValidateExpiresIn()
        {
            if (this.ExpiresIn < -1)
            {
                throw new ArgumentException("ExpiresIn must be >= -1");
            }
        }

        /// <summary>
        /// Validate the Name property
        /// </summary>
        public virtual void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                if (this.Name == null)
                {
                    throw new ArgumentNullException("Name cannot be NULL");
                }
                else
                {
                    throw new ArgumentException("Name cannot be EMPTY");
                }
            }
        }

        #endregion // Methods

        #region Properties

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

        #endregion //Properties
    }

    public class BlueCompareAward : Award
    {
        //private const string BlueCompare_AwardName = "Blue Compare";
    }

    public class BlueDistinctionPlusAward : Award
    {
        //private const string BlueDistinctionPlus_AwardName = "Blue Distinction Plus";

        public override void ValidateQuality()
        {
            if (this.Quality != Award._BlueDistinctionPlusQuality)
            {
                throw new ArgumentException("Quality must be == " + Award._BlueDistinctionPlusQuality.ToString());
            }

            //base.ValidateQuality(quality);
        }
    }

    public class BlueFirstAward : Award
    {
        //private const string BlueFirst_AwardName = "Blue First";
    }

    //public class ACMEPartnerFacilityAward : Award
    //{
    //    private const string ACMEPartnerFacility_AwardName = "ACME Partner Facility";

    //    public ACMEPartnerFacilityAward(int expiresIn, int quality) : base(ACMEPartnerFacility_AwardName, expiresIn, quality)
    //    {
    //        base.Validate(ACMEPartnerFacility_AwardName, expiresIn, quality);
    //    }
    //}

    //public class GovQualityPlusAward : Award
    //{
    //    private const string GovQualityPlus_AwardName = "Gov Quality Plus";

    //    public GovQualityPlusAward(int expiresIn, int quality) : base(GovQualityPlus_AwardName, expiresIn, quality)
    //    {
    //        base.Validate(GovQualityPlus_AwardName, expiresIn, quality);
    //    }
    //}

    //public class TopConnectedProvidersAward : Award
    //{
    //    private const string TopConnectedProviders_AwardName = "Top Connected Providers";

    //    public TopConnectedProvidersAward(int expiresIn, int quality) : base(TopConnectedProviders_AwardName, expiresIn, quality)
    //    {
    //        base.Validate(TopConnectedProviders_AwardName, expiresIn, quality);
    //    }
    //}
}
