using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                int performanceAmount = GetPerformanceAmount(perf, play.Type);
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if (PlayType.Comedy == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(performanceAmount / 100), perf.Audience);
                totalAmount += performanceAmount;
            }
            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static int GetPerformanceAmount(Performance perf, PlayType playType)
        {
            var performanceAmount = 0;
            switch (playType)
            {
                case PlayType.Tragedy:
                    performanceAmount = 40000;
                    if (perf.Audience > 30)
                    {
                        performanceAmount += 1000 * (perf.Audience - 30);
                    }
                    break;
                case PlayType.Comedy:
                    performanceAmount = 30000;
                    if (perf.Audience > 20)
                    {
                        performanceAmount += 10000 + 500 * (perf.Audience - 20);
                    }
                    performanceAmount += 300 * perf.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + playType);
            }

            return performanceAmount;
        }
    }
}
