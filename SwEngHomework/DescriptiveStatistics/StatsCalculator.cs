using System.Globalization;

namespace SwEngHomework.DescriptiveStatistics
{
    public class StatsCalculator : IStatsCalculator
    {
        /// <summary>
        /// Calculates statistical measures for the given semicolon-delimited contributions.
        /// </summary>
        /// <param name="semicolonDelimitedContributions">The semicolon-delimited string of contributions.</param>
        /// <returns>A Stats object containing the calculated statistical measures.</returns>
        public Stats Calculate(string semicolonDelimitedContributions)
        {
            // List to store parsed contribution amounts
            List<double> contributions = new();
            try
            {
                if (!string.IsNullOrEmpty(semicolonDelimitedContributions))
                {
                    string[] contributionArray = semicolonDelimitedContributions.Split(';');

                    // Parse each contribution amount and add it to the contributions list
                    foreach (string contribution in contributionArray)
                    {
                        double amount = ParseContributionAmount(contribution.Trim());
                        if (amount > 0)
                        {
                            contributions.Add(amount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Handle exception during parsing
                Console.WriteLine($"Error parsing contributions: {ex.Message}");
                return new Stats();
            }

            // Calculate statistical measures
            double average = contributions.Count > 0 ? contributions.Average() : 0;
            double median = contributions.Count > 0 ? CalculateMedian(contributions) : 0;
            double range = contributions.Count > 0 ? contributions.Max() - contributions.Min() : 0;

            // Create and return a Stats object with the calculated measures
            return new Stats
            {
                Average = Math.Round(average, 2),
                Median = Math.Round(median, 2),
                Range = Math.Round(range, 2)
            };
        }

        /// <summary>
        /// Parses a contribution amount from a string representation.
        /// </summary>
        /// <param name="contribution">The string representation of the contribution amount.</param>
        /// <returns>The parsed contribution amount as a double value.</returns>
        private static double ParseContributionAmount(string contribution)
        {
            double amount = 0;

            // Check if the contribution string is not null or empty
            if (!string.IsNullOrEmpty(contribution))
            {
                // Remove any currency symbols, commas, and leading/trailing whitespaces
                contribution = contribution.Replace("$", "").Replace(",", "").Trim();

                // Attempt to parse the contribution as a double in en-US culture format
                if (double.TryParse(contribution, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out amount))
                {
                    // return the parsed amount
                    return amount;
                }
            }

            return amount;
        }

        /// <summary>
        /// Calculates the median value from a list of contributions.
        /// </summary>
        /// <param name="contributions">The list of contributions.</param>
        /// <returns>The calculated median value.</returns>
        private static double CalculateMedian(List<double> contributions)
        {
            // Sort the contributions in ascending order
            contributions.Sort();

            // Check if the number of contributions is even
            if (contributions.Count % 2 == 0)
            {
                int middle = contributions.Count / 2;
                // Calculate the average of the two middle values
                return (contributions[middle - 1] + contributions[middle]) / 2;
            }
            else
            {
                int middle = contributions.Count / 2;
                // Return the middle value
                return contributions[middle];
            }
        }
    }
}