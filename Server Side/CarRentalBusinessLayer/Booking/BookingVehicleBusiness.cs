using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalBusinessLayer.Booking
{
    public class BookingVehicleBusiness : BusinessLayerInterfaces.IBookingVehicleBusiness
    {
        private readonly DataLayerInterfaces.IBookingVehicleData _BookingVehicleData;
        private readonly BusinessLayerInterfaces.IVehicleBusiness _VehicleBusiness;
        private readonly BusinessLayerInterfaces.IBookingStatusBusiness _BookingStatusBusiness;
        
        public enum enMode { AddNewMode = 1, UpdateFromUserMode = 2, UpdateFromCustomerMode = 3 }

        public enum enBookingStatus {Cancelled = 3, AwaitingApproval = 4 }

        public enMode Mode;
        public DTO.AddingBookingInfoDTO BookingInfo { get; set; }

        public BookingVehicleBusiness(DataLayerInterfaces.IBookingVehicleData BookingVehicleData,
                                      BusinessLayerInterfaces.IVehicleBusiness vehiclebusiness,
                                      BusinessLayerInterfaces.IBookingStatusBusiness bookingstatusbuiness)
        {
            _BookingVehicleData = BookingVehicleData;
            _VehicleBusiness = vehiclebusiness;
            _BookingStatusBusiness = bookingstatusbuiness;
            BookingInfo = new DTO.AddingBookingInfoDTO();
        }

        public void Initialize(DTO.AddingBookingInfoDTO BookingVehicle, enMode mode = enMode.AddNewMode)
        {
            BookingInfo = BookingVehicle;
            Mode = mode;
        }

        private bool _AddNewBookingVehicle()
        {
            BookingInfo.UserViewBookingInfo.BookingId = _BookingVehicleData.AddNewBookingVehicle(BookingInfo);
            return BookingInfo.UserViewBookingInfo.BookingId.HasValue;
        }

        private bool _UpdateBookingVehicleInfoFromCustomer()
        {
            if (!BookingInfo.UserViewBookingInfo.BookingId.HasValue)
                return false;
            return _BookingVehicleData.UpdateBookingVehicleInfoFromCustomer(BookingInfo.BookingInfoFromCustomer,
                                                                                       BookingInfo.UserViewBookingInfo.BookingId.Value);
        }
        private bool _UpdateBookingVehicleInfoFromUserOrAddingInitialCheckNotes()
        {
            return _BookingVehicleData.UpdateBookingVehicleInfoFromUserOrAddingInitialCheckNotes(BookingInfo.UserViewBookingInfo);
        }

        public bool CancelBooking(int bookingId)
        {
            DTO.UserViewBookingInfo bookingUserView = new DTO.UserViewBookingInfo();
            bookingUserView.BookingId = bookingId;

            // Cancel Status Id is 3
            bookingUserView.BookingStatusId = (int)enBookingStatus.Cancelled;
            return _BookingVehicleData.UpdateBookingVehicleInfoFromUserOrAddingInitialCheckNotes(bookingUserView);
        }

        // Changing the Status of Booking To Awaiting Approval from the User After The Customer Pays the fee 
        public bool WaitingforConfirmation(int bookingId)
        {
            DTO.UserViewBookingInfo bookingUserView = new DTO.UserViewBookingInfo();
            bookingUserView.BookingId = bookingId;

            // Awaiting Approval Status Id is 4
            bookingUserView.BookingStatusId = (int)enBookingStatus.AwaitingApproval;
            return _BookingVehicleData.UpdateBookingVehicleInfoFromUserOrAddingInitialCheckNotes(bookingUserView);
        }

        public DTO.CustomerViewBookingInfo GetBookingVehicleByBookingId(int bookingId)
        {
            var result = _BookingVehicleData.GetBookingVehicleByBookingId(bookingId);

            var vehicleObject = new DTO.UserViewVehicleDTO();
            vehicleObject.VehiclePropertyIdies.VehicleId = result.UserViewBookingInfo.VehicleId;

            result.CustomerViewBookingInfo.VehicleInfo = _VehicleBusiness.GetVehicleByForCustomerView(vehicleObject,null,null)[0];
            result.CustomerViewBookingInfo.BookingStatusInfo = _BookingStatusBusiness.GetBookingStatusBy(
                                                                      result.UserViewBookingInfo.BookingStatusId,"").BookingStatusInfor;

            return result.CustomerViewBookingInfo;
        }
        public List<DTO.MiniInfoOfBookingDTO.SendingInfo> GetAllBookingVehiclesForACustomer(int CustomerId)
        {
            List<DTO.MiniInfoOfBookingDTO.SendingInfo> AllBooking = new List<DTO.MiniInfoOfBookingDTO.SendingInfo>();
            DTO.MiniInfoOfBookingDTO.SendingInfo Booking = new DTO.MiniInfoOfBookingDTO.SendingInfo();

            var result = _BookingVehicleData.GetAllBookingVehiclesForACustomer(CustomerId);
            foreach (var item in result)
            {
                Booking.BookingId = item.SendingMiniBookingInfo.BookingId;
                Booking.BookingStatus = _BookingStatusBusiness.GetBookingStatusBy(item.BookingStatusId,"").BookingStatusInfor;

                var vehicleObject = new DTO.UserViewVehicleDTO();
                vehicleObject.VehiclePropertyIdies.VehicleId = item.VehicleId;
                var vehicles = _VehicleBusiness.GetVehicleBy(vehicleObject,null,null);

                if (vehicles.Count > 0 && vehicles != null)
                {
                    Booking.VehicleName = vehicles[0].VehicleInfo.VehicleName;
                }
                else
                {
                    throw new Exception($"No Vehicle found Whith VehicleId: {item.VehicleId} ");
                }

                AllBooking.Add(Booking);
            }
          
            return AllBooking;
        }
        public List<DTO.MiniInfoOfBookingDTO> GetAllBookingWaitingApproval()
        {
           

            var result = _BookingVehicleData.GetAllBookingWaitingApproval();
            foreach (var item in result)
            {
                item.SendingMiniBookingInfo.BookingStatus = _BookingStatusBusiness.GetBookingStatusBy(item.BookingStatusId, "").BookingStatusInfor;

                var vehicleObject = new DTO.UserViewVehicleDTO();
                vehicleObject.VehiclePropertyIdies.VehicleId = item.VehicleId;
                var vehicles = _VehicleBusiness.GetVehicleBy(vehicleObject, null, null);

                if (vehicles.Count > 0 && vehicles != null)
                {
                    item.SendingMiniBookingInfo.VehicleName = vehicles[0].VehicleInfo.VehicleName;
                }
                else
                {
                    throw new Exception($"No Vehicle found Whith VehicleId: {item.VehicleId} ");
                }

            }

            return result;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewBookingVehicle())
                    {
                        Mode = enMode.UpdateFromCustomerMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateFromCustomerMode:
                    if (_UpdateBookingVehicleInfoFromCustomer())
                    {
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateFromUserMode:
                    if (_UpdateBookingVehicleInfoFromUserOrAddingInitialCheckNotes())
                    {
                        return true;
                    }
                    else { return false; }
                default:
                    return false;
            }
        }
    }
}
