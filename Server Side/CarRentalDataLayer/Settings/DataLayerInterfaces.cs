using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDataLayer.Settings
{
    public class DataLayerInterfaces
    {
        public interface IUserData
        {
            public int? AddNewUser(DTO.UserDTO User);
            public bool UpdateUser(DTO.UserDTO User);
            public List<DTO.UserDTO> GetAllUsersBy(DTO.UserDTO User);
            public bool IsThisUserCanManageUsers(int userId);
            public DTO.UserDTO GetUserInfoByUserName(string UserName);
        }
        public interface ICustomerData
        {
            public int AddNewCustomer(DTO.CustomerDTO.CustomerInfo Customer);
            public bool UpdateCustomer(DTO.CustomerDTO Customer);
            public bool UpdateCustomerPasswordByid(string NewPassword, int CustomerId);
            public DTO.CustomerDTO GetCustomerInfoById(int CustomerId);
            public DTO.CustomerDTO GetCustomerInfoByEmail(string Email);
            public bool CheckEmailExists(string Email);

        }
        public interface IPeopleData
        {
            public DTO.PersonDTO.personInfo GetPersonInfoById(int personid);
        }
        public interface ILicenseData
        {
            public int AddNewLicense(DTO.LicenseDTO NewLicense);
            public bool UpdateLicense(DTO.LicenseDTO NewLicense);
            public List<DTO.LicenseDTO> GetAllLicenseForThiCustomerId(int Customerid);
            public bool DeleteLicense(int LicenseId);

        }
        public interface IVehicleStatusData
        {
              public int AddNewVehicleStatus(DTO.VehicleStatusDTO NewVehicleStatus);
             public bool UpdateVehicleStatus(DTO.VehicleStatusDTO NewVehicleStatus);
            public bool DeleteVehicleStatus(int VehicleStatusId);
            public List<DTO.VehicleStatusDTO> GetAllVehicleStatus();
            public DTO.VehicleStatusDTO GetVehicleStatusBy(int? VehicleStatusId,string StatusName);
        }
        public interface IFuelTypeData
        {
            public int AddNewFuelType(DTO.FuelTypeDTO NewFuelType);
            public bool UpdateFuelType(DTO.FuelTypeDTO NewFuelType);
            public bool DeleteFuelType(int FuelTypeId);
            public DTO.FuelTypeDTO GetFuelTypeBy(int? FuelTypeId,string FuelType); 
            public List<DTO.FuelTypeDTO> GetAllFuelType();

        }
        public interface IVehicleCategoryData
        {
            public int AddNewVehicleCategory(DTO.VehicleCategoryDTO NewVehicleCategory);
            public bool UpdateVehicleCategory(DTO.VehicleCategoryDTO NewVehicleCategory);
            public bool DeleteVehicleCategory(int VehicleCategoryId);
            public DTO.VehicleCategoryDTO GetVehicleCategoryBy(int? VehicleCategoryId,string CategoryName);
            public List<DTO.VehicleCategoryDTO> GetAllVehicleCategory();
        }
        public interface ILocationData
        {
            public int AddNewLocation(DTO.LocationDTO NewLocation);
            public bool UpdateLocation(DTO.LocationDTO NewLocation);
            public bool DeleteLocation(int LocationId);
            public DTO.LocationDTO GetLocationById(int LocationId);
            public List<DTO.LocationDTO> GetAllLocations();
            public List<DTO.LocationDTO> GetAllLocationsByLocationNameOrAdress(string LocationName,string Adress);
        }
        public interface IBookingStatusData
        {
            int? AddNewBookingStatus(DTO.BookingStatusDTO newBookingStatus);
            bool UpdateBookingStatus(DTO.BookingStatusDTO updatedBookingStatus);
            bool DeleteBookingStatus(int bookingStatusId);
            List<DTO.BookingStatusDTO> GetAllBookingStatuses();
            DTO.BookingStatusDTO GetBookingStatusBy(int? bookingStatusId, string statusName);

        }
        public interface IVehicleData
        {
            public int? AddNewVehicle(DTO.UserViewVehicleDTO Vehicle);
            public bool UpdateVehicle(DTO.UserViewVehicleDTO Vehicle);
            public bool DeleteVehicle(int VehicleId);
            public List<DTO.UserViewVehicleDTO> GetVehicleby(DTO.UserViewVehicleDTO vehicle);
            public List<DTO.UserViewVehicleDTO> GetVehiclebyPriceBetween(decimal StartPrice, decimal EndPrice);

        }
        public interface IBookingVehicleData
        {
            public int? AddNewBookingVehicle(DTO.AddingBookingInfoDTO bookinginfo);
            public bool UpdateBookingVehicleInfoFromCustomer(DTO.BookingInfoFromCustomer bookinginfo,int bookingid);
            public bool UpdateBookingVehicleInfoFromUserOrAddingInitialCheckNotes(DTO.UserViewBookingInfo bookinginfo);
            public List<DTO.MiniInfoOfBookingDTO> GetAllBookingVehiclesForACustomer(int CustomerId);
            public List<DTO.MiniInfoOfBookingDTO> GetAllBookingWaitingApproval();
            public DTO.GettingBookingInfoDTO GetBookingVehicleByBookingId(int bookingid);

        }

        public interface ICardData
        {
            public int? AddNewCard(DTO.CardDTO card);
            public bool UpdateCard(DTO.CardDTO card);
            public bool DeleteCard(int cardid);
            public List<DTO.CardDTO> GetAllCardsForACustomer(int customerid);
            public DTO.CardDTO GetCardById(int cardid);
        }
        public interface IPaymentMethodData
        {
            int? AddNewPaymentMethod(DTO.PaymentMethodDTO paymentMethod);
            bool UpdatePaymentMethod(DTO.PaymentMethodDTO paymentMethod);
            bool DeletePaymentMethod(int paymentMethodId);
            public DTO.PaymentMethodDTO GetPaymentMethodByIdOrName(int? VehicleCategoryId, string CategoryName);
            List<DTO.PaymentMethodDTO> GetAllPaymentMethods();
        }
        public interface IPaymentStatusData
        {
            int? AddNewPaymentStatus(DTO.PaymentStatusDTO paymentStatus);
            bool UpdatePaymentStatus(DTO.PaymentStatusDTO paymentStatus);
            bool DeletePaymentStatus(int paymentStatusId);
            DTO.PaymentStatusDTO GetPaymentStatusBy(int? paymentStatusId, string statusName);
            List<DTO.PaymentStatusDTO> GetAllPaymentStatuses();
        }
        public interface ITransactionTypeData
        {
            int? AddNewTransactionType(DTO.TransactionTypeDTO transactionType);
            bool UpdateTransactionType(DTO.TransactionTypeDTO transactionType);
            bool DeleteTransactionType(int transactionTypeId);
            DTO.TransactionTypeDTO GetTransactionTypeBy(int? transactionTypeId, string typeName);
            List<DTO.TransactionTypeDTO> GetAllTransactionTypes();
        }
        public interface IRentalTransactionData
        {
            int? AddNewRentalTransaction(DTO.RentalTransactionDTO rentalTransaction);
            bool UpdateRentalTransaction(DTO.RentalTransactionDTO rentalTransaction);
            List<DTO.RentalTransactionDTO> GetAllRentalTransactionsBy(DTO.RentalTransactionDTO rentaltransaction);
        }
        public interface IVehicleReturnLogData
        {
            int? AddNewVehicleReturnLog(DTO.VehicleReturnLog vehicleReturnLog);
            bool UpdateVehicleReturnLog(DTO.VehicleReturnLog vehicleReturnLog);
            List<DTO.VehicleReturnLog> GetAllVehicleReturnLogsBy(DTO.VehicleReturnLog VreturnInfo);
        }



    }
}
