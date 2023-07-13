using SwEngHomework.Commissions.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SwEngHomework.Commissions
{
    public class CommissionCalculator : ICommissionCalculator
    {
        /// <summary>
        /// Tiered Basis Points
        /// </summary>
        private const double Tier1Rate = 0.0005; // 5 bps
        private const double Tier2Rate = 0.0006; // 6 bps
        private const double Tier3Rate = 0.0007; // 7 bps

        /// <summary>
        /// Seniority Levels Dictionary
        /// </summary>
        private static readonly Dictionary<string, double> SeniorityLevel = new()
        {
            { "Junior", 0.25 },
            { "Experienced", 0.5 },
            { "Senior", 1 }
        };
        /// <summary>
        /// Calculates commissions for advisors based on the provided JSON input.
        /// </summary>
        /// <param name="jsonInput">The JSON input containing advisors and accounts data.</param>
        /// <returns>A dictionary with advisor names as keys and their commissions as values.</returns>
        public IDictionary<string, double> CalculateCommissionsByAdvisor(string jsonInput)
        {
            try
            {
                bool isValid = ValidateJson(jsonInput);

                if (isValid)
                {
                    // Deserialize the input JSON into AdvisorsData object
                    var data = JsonConvert.DeserializeObject<AdvisorsData>(jsonInput);

                    var advisors = data?.Advisors ?? new List<Advisor>();
                    var accounts = data?.Accounts ?? new List<Account>();

                    var accountDict = new Dictionary<string, HashSet<Account>>();

                    // Group accounts by advisor using a Dictionary
                    if (accounts != null)
                    {
                        foreach (var account in accounts)
                        {
                            if (account != null && !string.IsNullOrEmpty(account.Advisor))
                            {
                                if (!accountDict.ContainsKey(account.Advisor))
                                {
                                    accountDict[account.Advisor] = new HashSet<Account>();
                                }
                                accountDict[account.Advisor].Add(account);
                            }
                        }
                    }

                    // Calculate commissions for each advisor based on their associated accounts
                    var commissions = new Dictionary<string, double>();
                    if (advisors != null)
                    {
                        foreach (var advisor in advisors)
                        {
                            if (advisor != null && !string.IsNullOrEmpty(advisor.Name) && accountDict.TryGetValue(advisor.Name, out var advisorAccounts))
                            {
                                // Calculate commission for each account and accumulate the total commission
                                double commission = 0;

                                foreach (var account in advisorAccounts)
                                {
                                    double accountCommission = CalculateCommission(account?.PresentValue ?? 0, advisor?.Level ?? string.Empty);
                                    commission += accountCommission;
                                }

                                // Add the calculated commission to the commissions dictionary
                                commissions.Add(advisor.Name, Math.Round(commission, 2));
                            }
                            else
                            {
                                // Advisor has no accounts
                                commissions.Add(advisor?.Name ?? string.Empty, 0);
                            }
                        }
                    }

                    // Return the commissions dictionary
                    return commissions;
                }
                else
                {
                    Console.WriteLine($"Input JSON is not valid.");
                    return new Dictionary<string, double>();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine($"Error occurred while calculating commissions: {ex.Message}");
                return new Dictionary<string, double>();
            }
        }

        /// <summary>
        /// Calculates the commission based on the present value and seniority level
        /// </summary>
        /// <param name="presentValue">The present value of the account.</param>
        /// <param name="level">The Seniority level of the Advisor</param>
        /// <returns>The calculated commission amount.</returns>
        private static double CalculateCommission(double presentValue, string level)
        {
            double commission;

            // Calculate commission based on the present value
            if (presentValue < 50000)
            {
                commission = presentValue * Tier1Rate;
            }
            else if (presentValue < 100000)
            {
                commission = presentValue * Tier2Rate;
            }
            else
            {
                commission = presentValue * Tier3Rate;
            }

            // Apply the commission rate based on the seniority level
            if (SeniorityLevel.TryGetValue(level, out double Rate))
            {
                commission *= Rate;
            }

            return commission;
        }

        /// <summary>
        /// Validates whether the provided string is a valid JSON.
        /// </summary>
        /// <param name="jsonString">The JSON string to validate.</param>
        /// <returns>
        ///     <c>true</c> if the JSON string is valid; otherwise, <c>false</c>.
        /// </returns>
        private static bool ValidateJson(string jsonString)
        {
            try
            {
                // Attempt to parse the JSON string
                JToken.Parse(jsonString);
                // JSON parsing successful, the string is a valid JSON
                return true;
            }
            catch (JsonReaderException)
            {
                // JSON parsing failed, the string is not a valid JSON
                return false;
            }
        }
    }
}
