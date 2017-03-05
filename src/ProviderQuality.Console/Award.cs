using System;

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
            // Reduce the CurrentDay by 1
            CurrentDay--;

            if (Quality > Award._MinQuality)
            {
                if (!IsExpired)
                {
                    // If it is not Expired and still has a positive Quality value, reduce the Quality by 1
                    Quality -= 1;
                }
                else
                {
                    // If it is expired, and if it still has a positive Quality value, reduce the Quality by 2
                    Quality -= 2;
                }
            }

            // Quality can never be less than 0 (MinQuality)
            if (Quality < Award._MinQuality)
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

            ValidateQuality();
        }

        /// <summary>
        /// Validate the Quality property
        /// </summary>
        public virtual void ValidateQuality()
        {
            if (Quality < _MinQuality)
            {
                throw new ArgumentException("Quality must be >= " + _MinQuality.ToString());
            }

            if (Quality > _MaxQuality)
            {
                throw new ArgumentException("Quality must be <= " + _MaxQuality.ToString());
            }
        }

        /// <summary>
        /// Validate the Name property
        /// </summary>
        public virtual void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                if (Name == null)
                {
                    throw new ArgumentNullException("Name cannot be NULL");
                }
                else
                {
                    throw new ArgumentException("Name cannot be EMPTY");
                }
            }
        }

        #endregion //END - Methods

        #region Properties

        /// <summary>
        /// Used to update the Quality at different rates
        /// </summary>
        public enum QualityRate : int
        {
            Single = 1,
            Double = 2,
            Triple = 3
        };

        /// <summary>
        /// The immutable maximum quality value
        /// </summary>
        public const int _MaxQuality = 50;

        /// <summary>
        /// The immutable minimum quality value
        /// </summary>
        public const int _MinQuality = 0;

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

        /// <summary>
        /// ExpiresIn represents the number of days remaining
        /// CurrentDay is the offset from ExpiresIn
        /// ex. ExpiresIn = 5, in decremented 1 for each UpdateQuality call
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
        }

        #endregion //END - Properties
    }

    /// <summary>
    /// The Blue Compare Award
    /// </summary>
    public class BlueCompareAward : Award
    {
        /// <summary>
        /// Calculates the Appreciate Rate based on the CurrentDay
        /// </summary>
        public QualityRate AppreciationRate
        {
            get
            {
                QualityRate rate = QualityRate.Single;
                // Quality increases by 2 when there are 10 days or less left, and by 3 where there are 5 days or less left
                if (CurrentDay < 6 && CurrentDay >= 0)
                {
                    // Quality increases by 3 where there are 5 days or less left
                    rate = QualityRate.Triple;
                }
                else if (CurrentDay < 11 && CurrentDay >= 6)
                {
                    // Quality increases by 2 when there are 6 to 10 days remaining
                    rate = QualityRate.Double;
                }
                return rate;
            }
        }

        /// <summary>
        /// Blue Compare specific Update for the Quality property
        /// </summary>
        public override void UpdateQuality()
        {
            // If the Quality is not at Max yet, increase it by the AppreciationRate
            if (!IsExpired && Quality < Award._MaxQuality)
            {
                Quality += (int)AppreciationRate;

                // Quality cannot be greater than Max so make sure the AppreciationRate does not put it over.
                Quality = Quality > Award._MaxQuality ? Award._MaxQuality : Quality;
            }

            // Reduce the CurrentDay by 1
            CurrentDay--;

            if (IsExpired)
            {
                // but Quality value drops to 0 after the expiration date.
                Quality = _MinQuality;
            }
        }
    }

    /// <summary>
    /// The Blue Distinction Plus Award
    /// </summary>
    public class BlueDistinctionPlusAward : Award
    {
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

        /// <summary>
        /// Blue Distinction Plus specific Update for the Quality property
        /// </summary>
        public override void UpdateQuality()
        {
            //Do Nothing - Quality and ExpiresIn do not change
        }
    }

    /// <summary>
    /// The Blue First Award
    /// </summary>
    public class BlueFirstAward : Award
    {
        /// <summary>
        /// Blue First specific Update for the Quality property
        /// Increases Quality by 1 for each day while Unexpired and Expired
        /// On the day it expires, Quality is increased by 2
        /// </summary>
        public override void UpdateQuality()
        {
            if (!IsExpired && Quality < Award._MaxQuality)
            {
                // Increase Quality by one until MaxQuality
                Quality += (int)QualityRate.Single;
            }

            // Reduce the CurrentDay by 1
            CurrentDay--;

            if (IsExpired && Quality < Award._MaxQuality)
            {
                // Increase Quality by one until MaxQuality
                Quality += (int)QualityRate.Single;
            }
        }
    }
}
