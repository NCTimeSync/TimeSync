﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TimeSync.Model
{
    public class Timeregistration
    {
        //TODO: Supporting setting a specified time slot for a given toolkit. E.g. at FTFA the can be queried, but not at HK.
        public int CaseId { get; set; }

        public DateTime DoneDate { get; set; }
        public string ToolkitDisplayName { get; set; }
        public string Team { get; set; }
        public double Hours { get; set; }
        public bool IsSynchronized { get; set; }
        public string Duration { get; set; }
        public string Timeslot { get; set; }

        public List<Toolkit> ListOfToolkits { get; set; }
        public List<string> ListOfToolkitNames { get; set; }
        public ObservableCollection<string> ListOfTeams { get; set; }
        public ObservableCollection<string> ListOfTimeslots { get; set; }

        public Timeregistration()
        {
            ListOfToolkitNames = new List<string>();
            DoneDate = DateTime.Today;
        }

        public bool CouldConvertDurationToHours()
        {
            var durationIsNumberOfHours = DurationIsNumberOfHours(Duration);
            double hours;
            bool stringFormattedAsExpected;
            if (durationIsNumberOfHours)
            {
                Duration = Duration.Replace(',', '.');
                stringFormattedAsExpected = double.TryParse(Duration, NumberStyles.Any, CultureInfo.InvariantCulture, out hours);
            }
            else
            {
                stringFormattedAsExpected = DurationIsTimeInterval(Duration, out hours);
            }

            if (stringFormattedAsExpected) Hours = hours;
            return stringFormattedAsExpected;
        }

        private static bool DurationIsNumberOfHours(string duration)
        {
            return duration.Trim().Length < 5;
        }


        private static bool DurationIsTimeInterval(string duration, out double hours)
        {
            hours = 0;
            // expected string format "08(:,.)00 - 08(:,.)30"
            var result = Regex.Matches(duration, @"([\d]+)[\:\,\.]([\d]+)|([\d]+)");
            if (result.Count == 0) return false;

            var fromTime = result[0].Value;
            var toTime = result[1].Value;

            DateTime from = ConvertToDateTime(fromTime);
            DateTime to = ConvertToDateTime(toTime);
            var timespan = to.Subtract(from);
            hours = timespan.TotalHours;

            return true;
        }

        private static DateTime ConvertToDateTime(string time)
        {
            DateTime dt;
            if (time.Contains(":"))
            {
                var couldParse = DateTime.TryParse(time, out dt);
            }
            else if (time.Contains(",") || time.Contains("."))
            {
                var idx = time.IndexOfAny(",.".ToCharArray());
                var newTime = $"{time.Substring(0, idx)}:{time.Substring(idx)}";
                var couldParse = DateTime.TryParse(newTime, out dt);
            }
            else
            {
                var newTime = $"{time.Substring(0, 2)}:{time.Substring(3)}";
                var couldParse = DateTime.TryParse(newTime, out dt);
            }

            return dt;
        }
    }
}
