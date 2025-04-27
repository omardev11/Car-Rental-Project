using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace CarRentalBusinessLayer.Vehicle
{
    public class VehicleCategoryBusiness : IVehicleCategoryBusiness
    {
        private readonly IVehicleCategoryData _VehicleCategoryData;

        public enum enMode { AddNewMode = 1, UpdateMode = 2, DeleteMode = 3 }

        public enMode Mode;
        public DTO.VehicleCategoryDTO VehicleCategoryInfo { get; set; }

        public VehicleCategoryBusiness(IVehicleCategoryData vehicleCategoryData)
        {
            _VehicleCategoryData = vehicleCategoryData;
            VehicleCategoryInfo = new DTO.VehicleCategoryDTO();
        }

        public void Initialize(DTO.VehicleCategoryDTO VehicleCategoryInfo, enMode mode = enMode.AddNewMode)
        {
            this.VehicleCategoryInfo = VehicleCategoryInfo;
            Mode = mode;
        }

        private bool _AddNewVehicleCategory()
        {
            VehicleCategoryInfo.VehicleCategoryId = _VehicleCategoryData.AddNewVehicleCategory(VehicleCategoryInfo);
            return VehicleCategoryInfo.VehicleCategoryId != -1;
        }

        private bool _UpdateVehicleCategory()
        {
            return _VehicleCategoryData.UpdateVehicleCategory(VehicleCategoryInfo);
        }

        private bool _DeleteVehicleCategory()
        {
            return _VehicleCategoryData.DeleteVehicleCategory(VehicleCategoryInfo.VehicleCategoryId);
        }

        public DTO.VehicleCategoryDTO GetVehicleCategoryInfoBy(int? CategoryId, string CategoryName)
        {
            return _VehicleCategoryData.GetVehicleCategoryBy(CategoryId, CategoryName);
        }
        public List<DTO.VehicleCategoryDTO> GetAllVehicleCategoryInfo()
        {
            return _VehicleCategoryData.GetAllVehicleCategory();
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewVehicleCategory())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateVehicleCategory())
                    {
                        return true;
                    }
                    else { return false; }
                case enMode.DeleteMode:
                    if (_DeleteVehicleCategory())
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
