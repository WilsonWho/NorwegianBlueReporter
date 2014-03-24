﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NorwegianBlue.Analysis;
using NorwegianBlue.Analysis.Samples;

namespace NorwegianBlue.Integration.IIS.Samples
{
    public class IISSample : ISampleAnalysis
    {
        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; private set; }
        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out double value)
        {
            throw new NotImplementedException();
        }

        double IReadOnlyDictionary<string, double>.this[string key]
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Keys { get; private set; }
        public IEnumerable<double> Values { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public ReadOnlyDictionary<string, string> NonStats { get; private set; }
        public ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; private set; }
        public int CountFiltered(bool includeIgnored, List<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key, bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out double value, bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        double ISampleValues.this[string key, bool includeIgnored, List<string> extraIgnores]
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> FilteredKeys(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<double> FilteredValues(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        public dynamic AnalysisScratchPad { get; private set; }
        public void AddAnalysisNote(AnalysisNote note)
        {
            throw new NotImplementedException();
        }

        public void Analyze(IEnumerable<StatAnalyzer> analyzers)
        {
            throw new NotImplementedException();
        }
    }
}