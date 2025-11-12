using Microsoft.Data.SqlClient;
using RestaurantData;
using System;
using System.Data;
using RestaurantDTOs;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsOrder
    {
        public enum enServiceType { DineIn = 1, Delivery = 2, PickUp = 3 };
        public enum enPaymentMethod { Cash = 1, Card = 2, Coins = 3 };
        public enum enOrderStatus { OrderPlaced = 1, Preparing = 2, ReadyToPick = 3, ReadyToDeliver = 4, OnTheWay = 5, Arrived = 6, Completed = 7 };
        public int OrderID { get; private set; }
        public DateTime OrderDate { get; private set; }
        public byte ServiceType { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DeliveryFee { get; private set; }
        public int? LocationID { get; set; }
        public int UserID { get; set; }
        public int? DriverID { get; private set; }
        public bool IsComplete { get; private set; }
        public bool IsPaid { get; private set; }
        public clsOrderDTO OrderDTO
        {
            get { return (new clsOrderDTO(this.OrderID, this.OrderDate, this.ServiceType, this.TotalAmount, this.DeliveryFee, this.LocationID, this.UserID, this.DriverID, this.IsComplete)); }
        }
        private clsOrder(int OrderiD, DateTime orderDate, byte serviceType, decimal totalAmount, decimal? deliveryFee, int? locationID, int userID, int? driverID, bool isComplete, bool isPaid)
        {
            OrderID = OrderiD;
            OrderDate = orderDate;
            ServiceType = serviceType;
            TotalAmount = totalAmount;
            DeliveryFee = deliveryFee;
            LocationID = locationID;
            UserID = userID;
            DriverID = driverID;
            IsComplete = isComplete;
            IsPaid = isPaid;
        }
        public clsOrder()
        {
            OrderID = -1;
            OrderDate = DateTime.Now;
            ServiceType = 0;
            TotalAmount = 0;
            DeliveryFee = null;
            LocationID = null;
            UserID = -1;
            DriverID = null;
            IsComplete = false;
            IsPaid = false;
        }
        public static clsOrder Find(int ID)
        {
            bool IsPaid = false;
            clsOrderDTO OrderDTO = clsOrdersData.GetOrderByID(ID, ref IsPaid);

            if (OrderDTO != null)
            {
                return new clsOrder(ID, OrderDTO.OrderDate, OrderDTO.ServiceType, OrderDTO.TotalAmount, OrderDTO.DeliveryFee, OrderDTO.LocationID,OrderDTO.UserID,OrderDTO.DriverID,OrderDTO.IsComplete, IsPaid);
            }
            else
                return null;
        }
        private bool _AddNewOrder(clsPaymentInfoDTO PaymentInfoDTO, List<clsCheckoutCartItemsDTO> CheckoutCartItemsDTO)
        {
            this.OrderID = clsOrdersData.AddOrder(this.OrderDTO,PaymentInfoDTO,CheckoutCartItemsDTO);

            return (this.OrderID != -1);
        }
        public bool Save(clsPaymentInfoDTO PaymentInfoDTO, List<clsCheckoutCartItemsDTO> CheckoutCartItemsDTO)
        {
            if (this.OrderID == -1)
            {
                if (this.LocationID != null)
                {
                    clsSetting SettingData = clsSetting.GetSettingsInfo();
                    clsLocation Location = clsLocation.Find(this.LocationID.Value);
                    this.DriverID = clsOrdersData.GetDriverIDWithLeastAmountOfOrders();
                    double DeliveryFeeDouble = (double)SettingData.DeliveryFeePerKilo * GetDistanceInKm((double)SettingData.RestaurantLatitude, (double)SettingData.RestaurantLongitude, (double)Location.Latitude, (double)Location.Longitude);
                    this.DeliveryFee = (decimal)DeliveryFeeDouble;
                }
                this.TotalAmount = clsOrder.CalculateCartTotalAmount(CheckoutCartItemsDTO);
                return _AddNewOrder(PaymentInfoDTO,CheckoutCartItemsDTO);
            }
            else
            {
                return false;
            }
        }
        public bool UpdateOrderStatus()
        {
            return clsOrderStatusesData.UpdateOrderStatus(this.OrderID);
        }
        public bool UpdateOrderDriver(int NewDriverID)
        {
            return clsOrdersData.UpdateOrderDriver(this.OrderID,NewDriverID);
        }
        public static List<clsOrderDTO> GetAllOrders()
        {
            return clsOrdersData.GetAllOrders();
        }
        public static List<clsOrderDTO> GetAllOrders(int OrderStatus)
        {
            return clsOrdersData.GetAllOrders(OrderStatus);
        }
        public byte? GetOrderCurrentStatus()
        {
            return clsOrderStatusesData.GetOrderCurrentStatus(this.OrderID); ;
        }
        public static List<clsOrderStatusDTO> GetOrderStatuses(int OrderID)
        {
            return clsOrderStatusesData.GetOrderStatuses(OrderID);
        }
        public List<clsOrderStatusDTO> GetOrderStatuses()
        {
            return GetOrderStatuses(this.OrderID);
        }
        public static List<clsCartDTO> GetCartItemsByOrderID(int OrderID)
        {
            return clsCartsData.GetCartItemsByOrderID(OrderID);
        }
        public List<clsCartDTO> GetCartItemsByOrderID()
        {
            return GetCartItemsByOrderID(this.OrderID);
        }
        public static decimal CalculateCartTotalAmount(List<clsCheckoutCartItemsDTO> CheckoutCartItems)
        {
            return clsCartsData.CalculateCartTotalAmount(CheckoutCartItems);
        }
        public static List<clsCheckoutCartItemsDTO> SyncCartItemsWithDatabase(List<clsCheckoutCartItemsDTO> CheckoutCartItems)
        {
            return clsCartsData.SyncCartItemsWithDatabase(CheckoutCartItems);
        }
        public clsPaymentInfoDTO GetOrderPaymentsInfo()
        {
            return clsPaymentsInfoData.GetOrderPaymentsInfo(this.OrderID);
        }
        public decimal GetOrderRemainingAmount(int OrderID, int CoinsValue, decimal TotalAmount)
        {
            return clsOrdersData.GetOrderRemainingAmount(this.OrderID,CoinsValue,TotalAmount);
        }
        public static bool IsOrderAssignedToDriver(int OrderID, int DriverID)
        {
            return clsOrdersData.IsOrderAssignedToDriver(OrderID,DriverID);
        }
        private int GetOrderCoinAmount(int OrderID)
        {
            int OrderCoinAmount = 0;
            clsPaymentInfoDTO PaymentInfoDTO = clsPaymentsInfoData.GetOrderPaymentsInfo(this.OrderID);
            for (int i = 0; i < PaymentInfoDTO.PaymentMethod.Count; i++)
            {
                if (PaymentInfoDTO.PaymentMethod[i] == (byte)enPaymentMethod.Coins)
                {
                    OrderCoinAmount = (int)PaymentInfoDTO.Amount[i] * clsSetting.GetSettingsInfo().CoinWorth;
                }
            }
            return OrderCoinAmount;
        }
        public bool MarkOrderAsCompleted()
        {
            return clsOrdersData.MarkOrderAsCompleted(this.OrderID,GetOrderRemainingAmount(this.OrderID,GetOrderCoinAmount(this.OrderID),this.TotalAmount));
        }
        private static double GetDistanceInKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in km
            double lat1Rad = DegreesToRadians(lat1);
            double lon1Rad = DegreesToRadians(lon1);
            double lat2Rad = DegreesToRadians(lat2);
            double lon2Rad = DegreesToRadians(lon2);

            double dLat = lat2Rad - lat1Rad;
            double dLon = lon2Rad - lon1Rad;

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance in km
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        public static bool IsCartUpToDate(List<clsCheckoutCartItemsDTO> CheckoutCartItemsDTO, List<clsCheckoutCartItemsDTO> UpdatedCheckoutCartItemsDTO)
        {
            if (CheckoutCartItemsDTO.Count != UpdatedCheckoutCartItemsDTO.Count)
            {
                return false;
            }

            var OrderedByASCCheckoutCartItemsDTO = CheckoutCartItemsDTO.OrderBy(p => p.ProductID).ToList();

            for (int i = 0; i < UpdatedCheckoutCartItemsDTO.Count; i++)
            {
                if (UpdatedCheckoutCartItemsDTO[i].Price != OrderedByASCCheckoutCartItemsDTO[i].Price)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
