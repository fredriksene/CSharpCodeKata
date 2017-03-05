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
            // If it still has a positive CurrentDay value, reduce the CurrentDay by 1
            if (CurrentDay >= 0)
            {
                CurrentDay -= 1;
            }

            // If it is not Expired
            if (!IsExpired)
            {
                // If it still has a positive Quality value, reduce the Quality by 1
                if (Quality > Award._MinQuality)
                {
                    Quality -= 1;
                }

            }

            if(IsExpired)
            {
                // If it is expired, and if it still has a positive Quality value, reduce the Quality by 2
                if (Quality > Award._MinQuality)
                { 
                    Quality -= 2;
                }
            }

            // Quality can never be less than 0 (MinQuality)
            if(Quality < Award._MinQuality)
            {
                Quality = Award._MinQuality;
            }
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
                throw new ArgumentException("Quality must be <= 50");
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
        /// The immutable Expired value (-1 represents the final ExpiresIn/CurrentDay value when an Award has Expired)
        /// </summary>
        public const int _Expired = -1;

        /// <summary>
        /// The Name property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The ExpiresIn property
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// The Quality of the Award for the CurrentDay 
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// The Quality of the Award when it was created 
        /// </summary>
        public int OriginalQuality { get; set; }

        ///// <summary>
        ///// The Quality of the Award for the CurrentDay 
        ///// </summary>
        //public int CurrentQuality { get; set; }

        /// <summary>
        /// ExpiresIn represents the number of days remaining
        /// CurrentDay is the offset from ExpiresIn
        /// ex. ExpiresIn = 5, CurrentDay will have values ranging from 5 to -1 (Expired)
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

    /// <summary>
    /// The Blue Compare Award
    /// </summary>
    public class BlueCompareAward : Award
    {
        //private const string BlueCompare_AwardName = "Blue Compare";

        /// <summary>
        /// Used to update the Quality at different rates
        /// </summary>
        public enum QualityAppreciationRate : int
        {
            Single = 1,
            Double = 2,
            Triple = 3
        };

        /// <summary>
        /// Calculates the Appreciate Rate based on the CurrentDay
        /// </summary>
        public QualityAppreciationRate AppreciationRate
        {
            get
            {
                QualityAppreciationRate rate = QualityAppreciationRate.Single;
                // Quality increases by 2 when there are 10 days or less left, and by 3 where there are 5 days or less left, but quality value drops to 0 after the expiration date.
                if (CurrentDay < 6 && CurrentDay >= 0)
                {
                    rate = QualityAppreciationRate.Triple;
                }
                else if(CurrentDay < 11 && CurrentDay >= 6)
                {
                    rate = QualityAppreciationRate.Double;
                }
                return rate;
            }
        }
        
        /// <summary>
        /// Blue Compare specific Update for the Quality property
        /// </summary>
        public override void UpdateQuality()
        {
            if (!IsExpired)
            {
                // If the Quality is not at Max yet, increase it by the AppreciationRate
                if (Quality < Award._MaxQuality)
                {
                    Quality += (int)AppreciationRate;
                    // Quality cannot be greater than Max so make sure the AppreciationRate does not put it over.
                    Quality = Quality > Award._MaxQuality ? Award._MaxQuality : Quality;
                }

                CurrentDay -= 1;
            }

            if(IsExpired)
            {
                // but Quality value drops to 0 after the expiration date.
                Quality = 0;
            }
        }
    }

    /// <summary>
    /// The Blue Distinction Plus Award
    /// </summary>
    public class BlueDistinctionPlusAward : Award
    {
        //private const string BlueDistinctionPlus_AwardName = "Blue Distinction Plus";

        /// <summary>
        /// The immutable Blue Distinction Plus Quality value (Highest Quality)
        /// </summary>
        public const int _BlueDistinctionPlusQuality = 80;

        /// <summary>
        /// Blue Distinction Plus override for validating Quality property
        /// </summary>
        public override void ValidateQuality()
        {
            if (this.Quality != BlueDistinctionPlusAward._BlueDistinctionPlusQuality)
            {
                throw new ArgumentException("Quality must be == " + BlueDistinctionPlusAward._BlueDistinctionPlusQuality.ToString());
            }
        }

        ///// <summary>
        ///// Blue Distinction Plus override for validating ExpiresIn property
        ///// </summary>
        //public override void ValidateExpiresIn()
        //{
        //    if (ExpiresIn != Award._Expired)
        //    {
        //        throw new ArgumentException("ExpiresIn must be == " + Award._Expired.ToString());
        //    }

        //    //base.ValidateExpiresIn();
        //}

        /// <summary>
        /// Blue Distinction Plus specific Update for the Quality property
        /// </summary>
        public override void UpdateQuality()
        {
            //Do Nothing - Leave the Values set to: Quality=80 and ExpiresIn=-1

        }
    }

    /// <summary>
    /// The Blue First Award
    /// </summary>
    public class BlueFirstAward : Award
    {
        //private const string BlueFirst_AwardName = "Blue First";

        /// <summary>
        /// Blue First specific Update for the Quality property
        /// </summary>
        public override void UpdateQuality()
        {
            if (!IsExpired)
            {
                // Always increases Quality by one until MaxQuality
                if (Quality < Award._MaxQuality)
                {
                    Quality += 1;
                }

                // If it still has a positive CurrentDay value, reduce the CurrentDay by 1
                if (CurrentDay >= 0)
                {
                    CurrentDay -= 1;
                }
            }

            if (IsExpired)
            {
                // Always increases Quality by one until MaxQuality
                if (Quality < Award._MaxQuality)
                {
                    Quality += 1;
                }
            }
        }
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
