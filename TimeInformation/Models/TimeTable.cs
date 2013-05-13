using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeInformation.Models
{
    public class TimeTable
    {
        // 列車車次
        public string Train { get; set; }

        // 列車車種
        public string CarClass { get; set; }

        // 行駛 山、海線或無經過
        // 0: 不經過山海線
        // 1: 山線
        // 2: 海線
        public string Line { get; set; }

        // 起點站
        public string Origin { get; set; }

        // 終點站
        public string Dest { get; set; }

        // 離站時間
        public string DEPTime { get; set; }

        // 到站時間
        public string ARRTime { get; set; }

        // 備註
        public string Note { get; set; }
    }
}