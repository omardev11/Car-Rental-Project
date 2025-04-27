using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;
namespace CarRentalBusinessLayer.Vehicle
{
    public class VehicleBusiness : IVehicleBusiness 
    {
        private readonly IVehicleData _VehicleData;
        private readonly IVehicleStatusBusiness _VehicleStatusBusiness;
        private readonly IVehicleCategoryBusiness _VehicleCategoryBusiness;
        private readonly IFuelTypeBusiness _FuelTypeBusiness;
        private readonly ILocationBusiness _LocationBusiness;

        public enum enMode { AddNewMode = 1, UpdateMode = 2 }

        public enMode Mode;
        public DTO.UserViewVehicleDTO Vehicle { get; set; }

        public VehicleBusiness(IVehicleData vehicledata,IVehicleStatusBusiness vehicleStatus,IVehicleCategoryBusiness vehicleCategory,
                       IFuelTypeBusiness fuelType , ILocationBusiness location)
        {
            _VehicleData = vehicledata;
            _VehicleStatusBusiness = vehicleStatus;
            _VehicleCategoryBusiness = vehicleCategory;
            _FuelTypeBusiness = fuelType;
            _LocationBusiness = location;
            Vehicle = new DTO.UserViewVehicleDTO();
        }

        public void Initialize(DTO.UserViewVehicleDTO vehicle, enMode mode = enMode.AddNewMode)
        {
            Vehicle = vehicle;
            Mode = mode;
        }

        private bool _AddNewVehicle()
        {
            Vehicle.VehiclePropertyIdies.VehicleId = _VehicleData.AddNewVehicle(Vehicle);
            return Vehicle.VehiclePropertyIdies.VehicleId.HasValue;
        }

        private bool _UpdateVehicle()
        {
            return _VehicleData.UpdateVehicle(Vehicle);
        }

        public bool DeleteVehicle(int vehicleid)
        {
            return _VehicleData.DeleteVehicle(vehicleid);
        }

        public List<DTO.UserViewVehicleDTO> GetVehicleBy(DTO.UserViewVehicleDTO vehicle)
        {
            return _VehicleData.GetVehicleby(vehicle);
        }
        public List<DTO.CustomerViewVehicleDTO> GetVehicleByForCustomerView(DTO.UserViewVehicleDTO vehicle)
        {
            var userViewVehicle = _VehicleData.GetVehicleby(vehicle);
            List<DTO.CustomerViewVehicleDTO> CustomerViewVehicle = new List<DTO.CustomerViewVehicleDTO>();
            DTO.VehicleInfo VehicleDetailinfo;   
            foreach (var V in userViewVehicle)
            {
                VehicleDetailinfo = new DTO.VehicleInfo();
                VehicleDetailinfo.Status = _VehicleStatusBusiness.GetVehicleStatusInfoBy(V.VehiclePropertyIdies.VehicleStatusId,"").VehicleStatus;
                VehicleDetailinfo.Category = _VehicleCategoryBusiness.GetVehicleCategoryInfoBy(V.VehiclePropertyIdies.VehicleCategoryId, "").CategoryInfo;
                VehicleDetailinfo.FuelType = _FuelTypeBusiness.GetFuelTypeInfoBy(V.VehiclePropertyIdies.FuelTypeId, "").FuelTypeInfor;
                VehicleDetailinfo.Location = _LocationBusiness.GetLocationInfoById(V.VehiclePropertyIdies.VehicleCategoryId.Value).LocationInfor;

                CustomerViewVehicle.Add(new DTO.CustomerViewVehicleDTO(V.VehicleInfo, VehicleDetailinfo));
            }
            return CustomerViewVehicle;
        }
        public List<DTO.CustomerViewVehicleDTO> GetVehicleByPriceBetweenForCustomerView(decimal StartPrice,decimal EndPrice)
        {
            var userViewVehicle = _VehicleData.GetVehiclebyPriceBetween(StartPrice,EndPrice);
            List<DTO.CustomerViewVehicleDTO> CustomerViewVehicle = new List<DTO.CustomerViewVehicleDTO>();
            DTO.VehicleInfo VehicleDetailinfo;
            foreach (var V in userViewVehicle)
            {
                VehicleDetailinfo = new DTO.VehicleInfo();
                VehicleDetailinfo.Status = _VehicleStatusBusiness.GetVehicleStatusInfoBy(V.VehiclePropertyIdies.VehicleStatusId, "").VehicleStatus;
                VehicleDetailinfo.Category = _VehicleCategoryBusiness.GetVehicleCategoryInfoBy(V.VehiclePropertyIdies.VehicleCategoryId, "").CategoryInfo;
                VehicleDetailinfo.FuelType = _FuelTypeBusiness.GetFuelTypeInfoBy(V.VehiclePropertyIdies.FuelTypeId, "").FuelTypeInfor;
                VehicleDetailinfo.Location = _LocationBusiness.GetLocationInfoById(V.VehiclePropertyIdies.VehicleCategoryId.Value).LocationInfor;

                CustomerViewVehicle.Add(new DTO.CustomerViewVehicleDTO(V.VehicleInfo, VehicleDetailinfo));
            }
            return CustomerViewVehicle;
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewVehicle())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateVehicle())
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
