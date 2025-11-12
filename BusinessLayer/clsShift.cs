using System;
using System.Data;
using RestaurantData;
using RestaurantDTOs;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsShift
    {
        public int ShiftID { get; private set; }
        public byte ShiftDay { get; set; }
        public TimeSpan ShiftStart {  get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public clsShiftDTO ShiftDTO
        {
            get { return (new clsShiftDTO(this.ShiftID, this.ShiftDay, this.ShiftStart, this.ShiftEnd)); }
        }
        private clsShift(int shiftID, byte shiftDay, TimeSpan shiftStart, TimeSpan shiftEnd)
        {
            this.ShiftID = shiftID;
            this.ShiftDay = shiftDay;
            this.ShiftStart = shiftStart;
            this.ShiftEnd = shiftEnd;
        }
        public clsShift()
        {
            this.ShiftID = -1;
            this.ShiftDay = 0;
            this.ShiftStart = TimeSpan.MinValue;
            this.ShiftEnd = TimeSpan.MinValue;
        }
        public static clsShift Find(int ID)
        {
            clsShiftDTO ShiftDTO = clsShiftsData.GetShiftByID(ID);

            if (ShiftDTO != null)
            {
                return new clsShift(ID, ShiftDTO.ShiftDay, ShiftDTO.ShiftStart, ShiftDTO.ShiftEnd);
            }
            else
                return null;
        }
        public static bool DeleteShift(int ID)
        {
            return clsShiftsData.DeleteShift(ID);
        }
        private bool _AddNewShift()
        {
            this.ShiftID = clsShiftsData.AddShift(this.ShiftDTO);

            return (this.ShiftID != -1);
        }
        private bool _UpdateShift()
        {
            return clsShiftsData.UpdateShift(this.ShiftDTO);
        }
        public bool Save()
        {
            if (this.ShiftID == -1)
            {
                return _AddNewShift();
            }
            else
            {
                return _UpdateShift();
            }
        }
        public static List<clsShiftDTO> GetAllShifts()
        {
            return clsShiftsData.GetAllShifts();
        }
        private static void _GetRestaurantOpeningHours(ref string OpeningTimeSpan, List<clsShiftDTO> ShiftsList, clsShiftDTO TodaysShift, TimeSpan CurrentTime)
        {
            int ShiftListCount = ShiftsList.Count;

            if (CurrentTime < TodaysShift.ShiftStart)
            {
                OpeningTimeSpan = $"Restaurant Opens At {TodaysShift.ShiftStart}";
                return;
            }

            DayOfWeek Day;
            if (ShiftListCount == 1)
            {
                if (CurrentTime > TodaysShift.ShiftEnd)
                {
                    OpeningTimeSpan = $"Restaurant Opens At Next Week {DateTime.Today.DayOfWeek.ToString()} At {TodaysShift.ShiftStart}";
                    return;
                }
                if (TodaysShift == null)
                {
                    Day = (DayOfWeek)ShiftsList[0].ShiftDay;
                    OpeningTimeSpan = $"Restaurant Opens At {Day.ToString()} {ShiftsList[0].ShiftStart}";
                    return;
                }
            }

            byte DayIndex = (byte)DateTime.Today.DayOfWeek;
            DayIndex = DayIndex + 1 == 7 ? (byte)0 : DayIndex++;
            Day = (DayOfWeek)DayIndex;

            clsShiftDTO NextAvailableShiftDTO = ShiftsList.FirstOrDefault(p => p.ShiftDay == DayIndex);

            if (NextAvailableShiftDTO != null)
            {
                OpeningTimeSpan = $"Restaurant Opens {Day.ToString()} At {NextAvailableShiftDTO.ShiftStart}";
                return;
            }
            for (byte i = 1; i < 7; i++)
            {
                if (DayIndex + i == 7)
                {
                    DayIndex = 0;
                    Day = (DayOfWeek)DayIndex;
                }

                NextAvailableShiftDTO = ShiftsList.FirstOrDefault(p => p.ShiftDay == DayIndex + i);

                if (NextAvailableShiftDTO != null)
                {
                    Day = (DayOfWeek)(DayIndex + i);
                    OpeningTimeSpan = $"Restaurant Opens {Day.ToString()} At {NextAvailableShiftDTO.ShiftStart}";
                    return;
                }
            }
        }
        public static bool IsRestaurantOpenNow(ref string OpeningHours)
        {
            //return clsShiftsData.IsRestaurantOpenNow();
            List<clsShiftDTO> ShiftList = GetAllShifts();

            if(ShiftList == null)
            {
                OpeningHours = "Unknown When The Restaurant Will Open";
                return false;
            }

            clsShiftDTO TodaysShift = ShiftList.FirstOrDefault(p => p.ShiftDay == (byte)DateTime.Today.DayOfWeek);
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            if (TodaysShift == null)
            {
                _GetRestaurantOpeningHours(ref OpeningHours, ShiftList, TodaysShift, CurrentTime);
                return false;
            }

            if (TodaysShift.ShiftStart <= TodaysShift.ShiftEnd)
            {
                if (CurrentTime >= TodaysShift.ShiftStart && CurrentTime <= TodaysShift.ShiftEnd)
                {
                    return true;
                }
            }

            if (TodaysShift.ShiftStart > TodaysShift.ShiftEnd)
            {
                if (CurrentTime >= TodaysShift.ShiftStart)
                {
                    return true;
                }
            }

            int Day = (int)DateTime.Today.DayOfWeek;
            Day = Day - 1 == -1 ? 6 : Day--;

            clsShiftDTO PreviousDayShift = ShiftList.FirstOrDefault(p => p.ShiftDay == ((byte)Day));
            if (PreviousDayShift == null)
            {
                _GetRestaurantOpeningHours(ref OpeningHours, ShiftList, TodaysShift, CurrentTime);
                return false;
            }

            if (PreviousDayShift.ShiftStart <= PreviousDayShift.ShiftEnd)
            {
                _GetRestaurantOpeningHours(ref OpeningHours, ShiftList, TodaysShift, CurrentTime);
                return false;
            }

            if (CurrentTime <= PreviousDayShift.ShiftEnd)
            {
                return true;
            }
            return false;
        }
        public static List<int> GetDaysWithoutSchedule()
        {
            return clsShiftsData.GetDaysWithoutSchedule();
        }
    }
}
//public class clsShiftDTO
//{
//    public int ID { get; set; }
//    public byte Day { get; set; }
//    public TimeSpan ShiftStart { get; set; }
//    public TimeSpan ShiftEnd { get; set; }
//    public string Currency { get; set; }
//    public clsShiftDTO(int iD, byte day, TimeSpan shiftStart, TimeSpan shiftEnd, string currency)
//    {
//        ID = iD;
//        Day = day;
//        ShiftStart = shiftStart;
//        ShiftEnd = shiftEnd;
//        Currency = currency;
//    }
//}