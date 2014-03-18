using System;
using System.Text;
using NorwegianBlue.Analysis.Samples;
using NorwegianBlue.Analysis.Utilities;

namespace NorwegianBlue.Analysis.CommonAlgorithms
{
 
    public class CommonStatAnalysis
    {
        // MigraDoc/PDFSharp don't wrap cell contents... so this needs quite a lot of work to be a reasonable
        // presentation
        // TODO: make this usable
        public void SummaryStatComparisonAsTables(ISampleSetAnalysis statSet, ISampleAnalysis stat)
        {
            var stdDevs = new StringBuilder();
            var missingFields = new StringBuilder();

            const int numStdDevCols = 1;
            const int numMissingValCols = 1;

            var stdDevHeader = new StringBuilder("*Fields more than 1 std dev from mean*:\n\n");
            var stdDevTableHeader1 = new StringBuilder("Field|Standard-Deviations-from-Mean");
            var stdDevTableHeader2 = new StringBuilder("------------------|-----------------");
            for (int i = 0; i < numStdDevCols - 1; i++)
            {
                stdDevTableHeader1.Append("|Field|Standard-Deviations-from-Mean");
                stdDevTableHeader2.Append("|------------------|-----------------");
            }
            stdDevTableHeader1.Append("\n");
            stdDevTableHeader2.Append("\n");
            stdDevHeader.Append(stdDevTableHeader1);
            stdDevHeader.Append(stdDevTableHeader2);

            var missingValHeader = new StringBuilder("*Missing fields:*\n\n");
            var missingValTableHeader1 = new StringBuilder("Field");
            var missingValTableHeader2 = new StringBuilder("------------------");
            for (int i = 0; i < numMissingValCols - 1; i++)
            {
                missingValTableHeader1.Append("|Field");
                missingValTableHeader2.Append("|------------------");
            }
            missingValTableHeader1.Append("|\n");
            missingValTableHeader2.Append("|\n");
            missingValHeader.Append(missingValTableHeader1);
            missingValHeader.Append(missingValTableHeader2);

            int stdDevCol = 0;
            int missingValCol = 0;
            foreach (string item in statSet.AnalysisScratchPad.AllStatsHeaders)
            {
                var hyphenatedItem = item.InsertSoftHyphens();

                if (stat.ContainsKey(item))
                {
                    var numberOfSigmaFromMean = Math.Abs(stat[item] - statSet.AnalysisScratchPad.Averages[item]) / statSet.AnalysisScratchPad.StdDeviations[item];
                    if (numberOfSigmaFromMean > 1)
                    {
                        stdDevCol++;
                        stdDevs.AppendFormat("``{0}``|{1}", hyphenatedItem, Math.Round(numberOfSigmaFromMean, 2));
                        if (stdDevCol< numStdDevCols)
                        {
                            stdDevs.Append("|");
                        } 
                        else
                        {
                            stdDevCol = 0;
                            stdDevs.Append("\n");
                        }
                    }
                }
                else
                {
                    missingValCol++;
                    missingFields.AppendFormat("``{0}``", hyphenatedItem);
                    if (missingValCol < numMissingValCols)
                    {
                        missingFields.Append("|");
                    }
                    else
                    {
                        missingValCol = 0;
                        missingFields.Append("|\n");
                    }
                }
            }
            
            if (stdDevs.Length > 0)
            {
                if (stdDevCol != 0)
                {
                    for (int i = stdDevCol; i < numStdDevCols; i++)
                    {
                        stdDevs.Append("||");
                    }
                    stdDevs.Append("|\n");
                }
                stdDevs.Insert(0, stdDevHeader);
                stdDevs.Append("\n\n");
                var analysisNote = new AnalysisNote("Stat Summary", stdDevs.ToString());
                stat.AddAnalysisNote(analysisNote);
            }

            if (missingFields.Length > 0)
            {
                if (missingValCol != 0)
                {
                    for (int i = missingValCol; i < numMissingValCols; i++)
                    {
                        missingFields.Append("|");
                    }
                    missingFields.Append("\n");
                }
                missingFields.Insert(0, missingValHeader);
                missingFields.Append("\n\n");
                var analysisNote = new AnalysisNote("Missing Fields", missingFields.ToString());
                stat.AddAnalysisNote(analysisNote);
            }

        }

        public void SummaryStatComparison(ISampleSetAnalysis statSet, ISampleAnalysis stat)
        {
            var stdDevs = new StringBuilder();
            var missingFields = new StringBuilder();

            foreach (string item in statSet.AnalysisScratchPad.AllStatsHeaders)
            {
                var hyphenatedItem = item.InsertSoftHyphens();

                if (stat.ContainsKey(item))
                {
                    var numberOfSigmaFromMean = Math.Abs(stat[item] - statSet.AnalysisScratchPad.Averages[item]) / statSet.AnalysisScratchPad.StdDeviations[item];
                    if (numberOfSigmaFromMean > 1)
                    {
                        stdDevs.AppendFormat("``{0}`` \\[{1}\\]\n", hyphenatedItem, Math.Round(numberOfSigmaFromMean, 2));
                    }
                }
                else
                {
                    missingFields.AppendFormat("``{0}``\n", hyphenatedItem);
                }
            }

            if (stdDevs.Length > 0)
            {
                stdDevs.Insert(0, "####Fields more than 1 std dev from mean:\n");
                stdDevs.Append("\n\n");
                var analysisNote = new AnalysisNote("Stat Summary", stdDevs.ToString());
                stat.AddAnalysisNote(analysisNote);
            }

            if (missingFields.Length > 0)
            {
                missingFields.Insert(0, "####Missing fields:\n");
                missingFields.Append("\n\n");
                var analysisNote = new AnalysisNote("Missing Fields", missingFields.ToString());
                stat.AddAnalysisNote(analysisNote);
            }

        }
    }
}

