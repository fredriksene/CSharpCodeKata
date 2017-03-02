using System.Collections.Generic;

namespace ProviderQuality.Console
{
    public class Program
    {
        public IList<Award> Awards
        {
            get;
            set;
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Updating award metrics...!");

            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "ACME Partner Facility", ExpiresIn = 5, Quality = 7},
                    new Award {Name = "Blue Compare", ExpiresIn = 15, Quality = 20},
                    new Award {Name = "Blue Distinction Plus", ExpiresIn = 0, Quality = 80},
                    new Award {Name = "Blue First", ExpiresIn = 2, Quality = 0},
                    new Award {Name = "Gov Quality Plus", ExpiresIn = 10, Quality = 20},
                    new Award {Name = "Top Connected Providers", ExpiresIn = 3, Quality = 6}
                }

            };

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void UpdateQuality()
        {
            //Loop through all the awards
            for (var i = 0; i < Awards.Count; i++)
            {
                // All awards that are NOT "Blue First, Blue Compare, Blue Distinction Plus - if the Quality is > 0, reduce it by 1
                if (Awards[i].Name != "Blue First" && Awards[i].Name != "Blue Compare")
                {
                    if (Awards[i].Quality > 0)
                    {
                        if (Awards[i].Name != "Blue Distinction Plus")
                        {
                            Awards[i].Quality = Awards[i].Quality - 1;
                        }
                    }
                }
                // "Blue First, Blue Compare"
                else
                {
                    // If the quality is not at Max yet, increase it by 1
                    if (Awards[i].Quality < 50)
                    {
                        Awards[i].Quality = Awards[i].Quality + 1;

                        if (Awards[i].Name == "Blue Compare")
                        {
                            // If Blue Compare expires in 10 days or less and the quality is not at Max yet, increase it by another 1 (doubling the rate)
                            if (Awards[i].ExpiresIn < 11)
                            {
                                if (Awards[i].Quality < 50)
                                {
                                    Awards[i].Quality = Awards[i].Quality + 1;
                                }
                            }

                            // If Blue Compare expires in 5 days or less and the quality is not at Max yet, increase it by another 1 (tripling the rate)
                            if (Awards[i].ExpiresIn < 6)
                            {
                                if (Awards[i].Quality < 50)
                                {
                                    Awards[i].Quality = Awards[i].Quality + 1;
                                }
                            }
                        }
                    }
                }

                // All awards except "Blue Distinction Plus" reduce the remaining expires in value by 1
                if (Awards[i].Name != "Blue Distinction Plus")
                {
                    Awards[i].ExpiresIn = Awards[i].ExpiresIn - 1;
                }

                // if the award has expired
                if (Awards[i].ExpiresIn < 0)
                {
                    // All awards except "Blue First"
                    if (Awards[i].Name != "Blue First")
                    {
                        if (Awards[i].Name != "Blue Compare")
                        {
                            if (Awards[i].Quality > 0)
                            {
                                if (Awards[i].Name != "Blue Distinction Plus")
                                {
                                    Awards[i].Quality = Awards[i].Quality - 1;
                                }
                            }
                        }
                        // "Blue Compare"
                        else
                        {
                            // Quality becomes 0 after it has expired
                            Awards[i].Quality = Awards[i].Quality - Awards[i].Quality;
                        }
                    }
                    else
                    {
                        // "Blue First"
                        if (Awards[i].Quality < 50)
                        {
                            Awards[i].Quality = Awards[i].Quality + 1;
                        }
                    }
                }
            }
        }

    }

}
