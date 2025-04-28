using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalBusinessLayer.Vehicle
{
    public class VehicleStatusBusiness : BusinessLayerInterfaces.IVehicleStatusBusiness
    {

        private readonly DataLayerInterfaces.IVehicleStatusData _VehicleStatusData;
        public enum enMode { AddNewMode = 1, UpdateMode = 2, DeleteMode = 3 }

        public enMode Mode;
        public DTO.VehicleStatusDTO VehicleStatusInfo { get; set; }
        public VehicleStatusBusiness(DataLayerInterfaces.IVehicleStatusData vehiclestatusdata)
        {
            _VehicleStatusData = vehiclestatusdata;
            VehicleStatusInfo = new DTO.VehicleStatusDTO();
        }
        public void Initialize(DTO.VehicleStatusDTO Vehiclestatusinfo, enMode mode = enMode.AddNewMode)
        {
            VehicleStatusInfo = Vehiclestatusinfo;
            Mode = mode;
        }

        private bool _AddNewVehicleStatus()
        {
            VehicleStatusInfo.VehicleStatusId = _VehicleStatusData.AddNewVehicleStatus(VehicleStatusInfo);
            return VehicleStatusInfo.VehicleStatusId != -1;
        }

        private bool _UpdateVehicleStatus()
        {
            return _VehicleStatusData.UpdateVehicleStatus(VehicleStatusInfo);
        }
        private bool _DeleteVehicleStatus()
        {
            return _VehicleStatusData.DeleteVehicleStatus(VehicleStatusInfo.VehicleStatusId);
        }

        public DTO.VehicleStatusDTO GetVehicleStatusInfoBy(int? VehicleStatusId,string StatusName)
        {
            return _VehicleStatusData.GetVehicleStatusBy(VehicleStatusId,StatusName);
        }
        public List<DTO.VehicleStatusDTO> GetAllVehicleStatus()
        {
            return _VehicleStatusData.GetAllVehicleStatus();    
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewVehicleStatus())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateVehicleStatus())
                    {
                        return true;
                    }
                    else { return false; }
                case enMode.DeleteMode:
                    if (_DeleteVehicleStatus())
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
