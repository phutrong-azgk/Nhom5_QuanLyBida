using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom5_QuanLyBida
{
    public static class TableStateManager
    {
        private static Dictionary<string, TableState> tableStates = new Dictionary<string, TableState>();

        public class TableState
        {
            public bool IsTimerActive { get; set; }
            public System.DateTime? StartTime { get; set; }
        }

        public static void SetTableState(string maBan, bool isTimerActive, System.DateTime? startTime = null)
        {
            if (!tableStates.ContainsKey(maBan))
            {
                tableStates[maBan] = new TableState();
            }
            tableStates[maBan].IsTimerActive = isTimerActive;
            tableStates[maBan].StartTime = startTime;
        }

        public static TableState GetTableState(string maBan)
        {
            if (tableStates.ContainsKey(maBan))
            {
                return tableStates[maBan];
            }
            return null;
        }

        public static void ClearTableState(string maBan)
        {
            if (tableStates.ContainsKey(maBan))
            {
                tableStates.Remove(maBan);
            }
        }

        public static void ClearAllStates()
        {
            tableStates.Clear();
        }
    }
}