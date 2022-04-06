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
            var result = "";
            GetHeader(invoice, ref result);
            CountPlays(invoice, plays, ref totalAmount, ref volumeCredits, ref result);
            GetFooter(totalAmount, volumeCredits, ref result);
            return result;
        }

        private static void GetHeader(Invoice invoice, ref string result)
        {
            result += string.Format("Statement for {0}\n", invoice.Customer);
        }

        private static void GetFooter(int totalAmount, int volumeCredits, ref string result)
        {
            result += string.Format(new CultureInfo("en-US"), "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
        }

        private static void CountPlays(Invoice invoice, Dictionary<string, Play> plays, ref int totalAmount, ref int volumeCredits, ref string result)
        {
            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                int performanceAmount = GetPerformanceAmount(perf, play.Type);
                volumeCredits = GetVolumeCredits(volumeCredits, perf, play.Type);
                totalAmount += performanceAmount;

                // print line for this order
                result += string.Format(new CultureInfo("en-US"), "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(performanceAmount / 100), perf.Audience);
            }
        }

        private static int GetVolumeCredits(int volumeCredits, Performance perf, PlayType playType) {
            volumeCredits += Math.Max(perf.Audience - 30, 0);
            if (PlayType.Comedy == playType)
                volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

            return volumeCredits;
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
