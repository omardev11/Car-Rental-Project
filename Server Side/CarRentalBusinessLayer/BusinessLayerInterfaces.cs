using CarRentalBusinessLayer.Booking;
using CarRentalBusinessLayer.CardsAndPayments;
using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace CarRentalBusinessLayer
{
    public class BusinessLayerInterfaces
    {
        public interface IUserBusiness
        {
            public List<DTO.UserDTO> GetAllUsersBy(DTO.UserDTO user);
            public bool Save();
            public void Initialize(DTO.UserDTO User, UserBusiness.enMode Mode = UserBusiness.enMode.AddNewMode);
            public DTO.UserDTO _UserInfo { get; set; }
            public bool IsThisUserCanManageUsers(int userId);
            public DTO.UserDTO GetUserInfoByUserNameAndPassword(string UserName,string Password);


        }
        public interface ICustomerBusiness
        {
           
            public bool UpdateCustomerPasswordByid(string NewPassword, int CustomerId);
            public DTO.CustomerDTO GetCustomerInfoById(int CustomerId);
            public bool CheckEmailExists(string Email);
            public DTO.CustomerDTO GetCustomerByEmailAndPassword(string Email,string Password);
            public bool Save();
            public void Initialize(DTO.CustomerDTO Customer,CustomerBusiness.enMode Mode = CustomerBusiness.enMode.AddNewMode);
            public DTO.CustomerDTO Customer { get; set; }


        }
        public interface ILicenseBusiness
        {
            public DTO.LicenseDTO LicenseInfo { get; set; }
            public bool Save();
            public void Initialize(DTO.LicenseDTO Customer, LicenseBusiness.enMode Mode = LicenseBusiness.enMode.AddNewMode);
            public List<DTO.LicenseDTO> GetLicenseByCustomerId(int LicenseId);
            

        }
        public interface IVehicleStatusBusiness
        {
            public DTO.VehicleStatusDTO VehicleStatusInfo { get; set; }
            public bool Save(); 
            public void Initialize(DTO.VehicleStatusDTO VehicleStatus, VehicleStatusBusiness.enMode Mode = VehicleStatusBusiness.enMode.AddNewMode);
            public DTO.VehicleStatusDTO GetVehicleStatusInfoBy(int? VehicleId,string StausName);
            public List<DTO.VehicleStatusDTO> GetAllVehicleStatus();
        }
        public interface IVehicleCategoryBusiness
        {
            public DTO.VehicleCategoryDTO VehicleCategoryInfo { get; set; }
            public bool Save();
            public void Initialize(DTO.VehicleCategoryDTO VehicleCategory, VehicleCategoryBusiness.enMode Mode = VehicleCategoryBusiness.enMode.AddNewMode);
            public DTO.VehicleCategoryDTO GetVehicleCategoryInfoBy(int? CategoryId, string CategoryName);
            public List<DTO.VehicleCategoryDTO> GetAllVehicleCategoryInfo();
        }
        public interface IFuelTypeBusiness
        {
            public DTO.FuelTypeDTO FuelTypeInfo { get; set; }
            public bool Save();
            public void Initialize(DTO.FuelTypeDTO FuelType, FuelTypeBusiness.enMode Mode = FuelTypeBusiness.enMode.AddNewMode);
            public DTO.FuelTypeDTO GetFuelTypeInfoBy(int? FuelTypeId, string FuelTypeName);
            public List<DTO.FuelTypeDTO> GetAllFuelTypeInfo();
        }
        public interface ILocationBusiness
        {
            public DTO.LocationDTO LocationInfo { get; set; }
            public bool Save();
            public void Initialize(DTO.LocationDTO Location, LocationBusiness.enMode Mode = LocationBusiness.enMode.AddNewMode);
            public DTO.LocationDTO GetLocationInfoById(int LocationId);
            public List<DTO.LocationDTO> GetAllLocationsByLocationNameOrAdress(string LocationName,string Address);
            public List<DTO.LocationDTO> GetAllLocations();
        }
        public interface IBookingStatusBusiness
        {
            public DTO.BookingStatusDTO BookingStatusInfo { get; set; }
            public bool Save();
            public void Initialize(DTO.BookingStatusDTO bookingStatus, BookingStatusBusiness.enMode Mode = BookingStatusBusiness.enMode.AddNewMode);
            public DTO.BookingStatusDTO GetBookingStatusBy(int? bookinstatusId, string StausName);
            public List<DTO.BookingStatusDTO> GetAllBookingStatus();
            public bool DeleteVehicleStatus(int bookinstatusid);
        }
        public interface IVehicleBusiness
        {
            public DTO.UserViewVehicleDTO Vehicle { get; set; }
            public bool Save();
            public bool DeleteVehicle(int vehicleid);
            public void Initialize(DTO.UserViewVehicleDTO vehicle, VehicleBusiness.enMode Mode = VehicleBusiness.enMode.AddNewMode);
            public List<DTO.UserViewVehicleDTO> GetVehicleBy(DTO.UserViewVehicleDTO vehicle);
            public List<DTO.CustomerViewVehicleDTO> GetVehicleByForCustomerView(DTO.UserViewVehicleDTO vehicle);
            public List<DTO.CustomerViewVehicleDTO> GetVehicleByPriceBetweenForCustomerView(decimal StartPrice, decimal EndPrice);

        }
        public interface IBookingVehicleBusiness
        {
            public DTO.AddingBookingInfoDTO BookingInfo { get; set; }
            public bool Save();
            public bool CancelBooking(int bookingId);
            public bool WaitingforConfirmation(int bookingId);
            public void Initialize(DTO.AddingBookingInfoDTO bookinginfo, BookingVehicleBusiness.enMode Mode = BookingVehicleBusiness.enMode.AddNewMode);
            public List<DTO.MiniInfoOfBookingDTO.SendingInfo> GetAllBookingVehiclesForACustomer(int CustomerId);
            public List<DTO.MiniInfoOfBookingDTO> GetAllBookingWaitingApproval();
            public DTO.CustomerViewBookingInfo GetBookingVehicleByBookingId(int bookingId);
        }

        public interface ICardBusiness
        {
            public DTO.CardDTO _CardInfo { get; set; }
            public bool Save();
            public void Initialize(DTO.CardDTO cardinfo, CardBusiness.enMode Mode = CardBusiness.enMode.AddNewMode);
            public DTO.CardDTO GetCardInfoById(int cardid);
            public bool DeleteCardInfoById(int cardid);

            public List<DTO.CardDTO> GetAllCardsForAcustomer(int customerid);
        }
        public interface IPaymentMethodBusiness
        {
            void Initialize(DTO.PaymentMethodDTO paymentMethodInfo, PaymentMethodBusiness.enMode mode = PaymentMethodBusiness.enMode.AddNewMode);
            DTO.PaymentMethodDTO GetPaymentMethodById(int? paymentMethodId, string paymentMethod);
            List<DTO.PaymentMethodDTO> GetAllPaymentMethods();
            bool Save();
            public bool DeletePaymentMethod(int paymentmethodid);

        }
        public interface IPaymentStatusBusiness
        {
            void Initialize(DTO.PaymentStatusDTO paymentStatusInfo, PaymentStatusBusiness.enMode mode = PaymentStatusBusiness.enMode.AddNewMode);
            DTO.PaymentStatusDTO GetPaymentStatusInfoBy(int? paymentStatusId, string statusName);
            List<DTO.PaymentStatusDTO> GetAllPaymentStatuses();
            bool Save();
            public bool DeletePaymentStatus(int paymentstatusid);
        }
        public interface ITransactionTypeBusiness
        {
            void Initialize(DTO.TransactionTypeDTO transactionTypeInfo, TransactionTypeBusiness.enMode mode = TransactionTypeBusiness.enMode.AddNewMode);
            DTO.TransactionTypeDTO GetTransactionTypeBy(int? transactionTypeId, string typeName);
            List<DTO.TransactionTypeDTO> GetAllTransactionTypes();
            bool Save();
            public bool DeleteTransactionType(int transactiontypeId);
        }
        public interface IRentalTransactionBusiness
        {
            void Initialize(DTO.RentalTransactionDTO rentalTransaction, RentalTransactionBusiness.enMode mode = RentalTransactionBusiness.enMode.AddNewMode);
            public List<DTO.RentalTransactionDTO> GetAllRentalTransactionsBy(DTO.RentalTransactionDTO rentaltransaction);
            bool Save();
        }
        public interface IVehicleReturnLogBusiness
        {
            void Initialize(DTO.VehicleReturnLog vehicleReturnLog, VehicleReturnLogBusiness.enMode mode = VehicleReturnLogBusiness.enMode.AddNewMode);
            public List<DTO.VehicleReturnLog> GetAllVehicleReturnLogsBy(DTO.VehicleReturnLog vehiclereturninfo);
            bool Save();
        }


    }
}
