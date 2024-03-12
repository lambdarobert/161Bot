using System;
using System.Collections.Generic;
using System.Text;

namespace _161Bot.Polls
{
    //container class for poll data
    public class Poll
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> OptionData { get; set; }
        public bool ShowResults { get; set; }
        public int MinOptions { get; set; }

        public int MaxOptions { get; set; }

        public bool PollLocked { get; set; }

        public ulong MessageId;

        //Key: User id of person
        //Value: How they voted
        public Dictionary<ulong, List<string>> VotingData;

        //should be random set using Guid
        public string Id { get; set; }

    }
}
