using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwEngHomework.Commissions.Models
{
    /// <summary>
    /// JSON Input File Model
    /// </summary>
    public class AdvisorsData
    {
        /// <summary>
        /// Gets or sets the list of advisors.
        /// </summary>
        public List<Advisor>? Advisors { get; set; }
        /// <summary>
        /// Gets or sets the list of accounts.
        /// </summary>
        public List<Account>? Accounts { get; set; }
    }
}
