using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class Constants
    {
        public const int RES_MIN = 10;
        public const int RES_MAX = 50;

        public const int L1time = 15;
        public const int L2time = 30;
        public const int L3time = 40;
        public static int[] Punishment = { 50, 500,5000 };

        public const double P_consume_per_s = 2;

        public const double CollectBaseSpeed = 5;
        public const double CollectL1Factor = 1.2;
        public const double CollectL2Factor = 1.5;
        public const double CollectL3Factor = 2;

        public const double PopL1Factor = 1.2;
        public const double PopL2Factor = 1.5;
        public const double PopL3Factor = 2;
    }
}
