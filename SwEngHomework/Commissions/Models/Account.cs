using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwEngHomework.Commissions.Models
{
    /// <summary>
    /// Account Model
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets the advisor associated with the account.
        /// </summary>
        public string? Advisor { get; set; }
        /// <summary>
        /// Gets or sets the present value of the account.
        /// </summary>
        public double? PresentValue { get; set; }
    }
}
