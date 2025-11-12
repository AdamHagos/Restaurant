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
    public class clsUserRole
    {
        public enum enUserRole { Admin = 1, Staff = 2, Driver = 3};
        public int UserRoleID { get; private set; }
        public byte UserRole { get; set; }
        public int UserID { get; set; }
        public clsUserRoleDTO UserRoleDTO
        {
            get { return (new clsUserRoleDTO(this.UserRoleID, this.UserRole, this.UserID)); }
        }
        private clsUserRole(int userRoleID, byte userRole, int userID)
        {
            this.UserRoleID = userRoleID;
            this.UserRole = userRole;
            this.UserID = userID;
        }
        public clsUserRole()
        {
            this.UserRoleID = -1;
            this.UserRole = 0;
            this.UserID = -1;
        }
        public static clsUserRole Find(int ID)
        {
            clsUserRoleDTO UserRoleDTO = clsUserRolesData.GetUserRoleByID(ID);

            if (UserRoleDTO != null)
            {
                return new clsUserRole(ID, UserRoleDTO.UserRole, UserRoleDTO.UserID);
            }
            else
                return null;
        }
        public static bool DeleteUserRole(int ID)
        {
            return clsUserRolesData.DeleteUserRole(ID);
        }
        private bool _AddNewUserRole()
        {
            this.UserRoleID = clsUserRolesData.AddUserRole(this.UserRoleDTO);

            return (this.UserRoleID != -1);
        }
        private bool _UpdateUserRole()
        {
            return clsUserRolesData.UpdateUserRole(this.UserRoleDTO);
        }
        public bool Save()
        {
            if (this.UserRoleID == -1)
            {
                return _AddNewUserRole();
            }
            else
            {
                return _UpdateUserRole();
            }
        }
        public static List<clsUserRoleWithNameDTO> GetAllUserRoles()
        {
            return clsUserRolesData.GetAllUserRoles();
        }
        public static bool DoesUserHaveRole(int UserID)
        {
            return clsUserRolesData.DoesUserHaveRole(UserID);
        }
        public static int DistributeDriverOrders(int DriverID)
        {
            return clsUserRolesData.DistributeDriverOrders(DriverID);
        }
        public static List<clsUserRoleWithNameDTO> GetAllDrivers()
        {
            return clsUserRolesData.GetAllDrivers();
        }
    }
}