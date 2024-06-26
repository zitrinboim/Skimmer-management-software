﻿using Dal;
using DalApi;
using DO;
using DL;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Xml.Linq;

namespace DalXml
{
    class DalXml : IDal
    {
        private static string DroneXml = @"DroneXml.Xml";
        private static string StationXml = @"StationXml.Xml";
        private static string CustomerXml = @"CustomerXml.Xml";
        private static string ParcelXml = @"ParcelXml.Xml";
        private static string DroneChargeXml = @" DroneChargeXml.Xml";
        private static string configXml = @"configXML.Xml";


        internal static DalXml instatnce = new DalXml();
       
        private DalXml() { }
        /// <summary>
        /// This function allows the user to add a base station to the list.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool addStation(Station station)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);

            int find = stations.FindIndex(Station => Station.Id == station.Id);
            //Safety mechanism to prevent the overrun of an existing entity with the same ID.
            if (find == -1)
            {
                stations.Add(station);
                XMLTools.SaveListToXMLSerializer(stations, StationXml);
                return true;
            }
            else
            {
                throw new IdExistExeptions("Sorry, i have already a station with this id:" + station.Id);
            }
        }
        /// <summary>
        /// This function allows the user to add a drone to the list.
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool addDrone(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);

            int find = drones.FindIndex(Drone => Drone.Id == drone.Id);
            //Safety mechanism to prevent the overrun of an existing entity with the same ID.
            if (find == -1)
            {
                drones.Add(drone);
                XMLTools.SaveListToXMLSerializer(drones, DroneXml);

                return true;
            }
            throw new IdExistExeptions("Sorry, i have already a drone with this id:" + drone.Id);
        }
        /// <summary>
        /// This function allows the user to add a customer to the list.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool addCustomer(Customer customer)
        {
            XElement elements = XMLTools.LoadListFromXMLElement(CustomerXml);
            XElement customerElement = (from element in elements.Elements()
                                        where element.Element("Id").Value == customer.Id.ToString()
                                        select element).FirstOrDefault() != default ?
                                        throw new IdExistExeptions("Sorry, i have already a customer with this id:" + customer.Id) :
                                        new XElement("Customer", new XElement
                                        ("Id", customer.Id)
                                       , new XElement("name", customer.name)
                                       , new XElement("phone", customer.phone)
                                       , new XElement("longitude", customer.longitude)
                                     , new XElement("lattitude", customer.lattitude));
            elements.Add(customerElement);
            XMLTools.SaveListToXMLElement(elements, CustomerXml);
            return true;
        }
        /// <summary>
        ///This function allows the user to add a parcel to the list.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int addParsel(Parcel parcel)
        {
            XElement xElement = XMLTools.LoadListFromXMLElement(configXml);
            int idTmep = int.Parse(xElement.Value);
            parcel.Id = 1 + idTmep;

            XElement elements = XMLTools.LoadListFromXMLElement(ParcelXml);
            XElement parcelElement = new XElement("Parcel", new XElement
                                       ("Id", parcel.Id)
                                       , new XElement("SenderId", parcel.SenderId)
                                       , new XElement("TargetId", parcel.TargetId)
                                       , new XElement("weight", parcel.weight)
                                     , new XElement("priority", parcel.priority)
                                     , new XElement("DroneId", parcel.DroneId)
                                     , new XElement("Requested", parcel.Requested)
                                     , new XElement("Scheduled", parcel.Scheduled)
                                     , new XElement("PickedUp", parcel.PickedUp)
                                     , new XElement("Delivered", parcel.Delivered));

            elements.Add(parcelElement);
            xElement.Value = parcel.Id.ToString();
            XMLTools.SaveListToXMLElement(elements, ParcelXml);
            XMLTools.SaveListToXMLElement(xElement, configXml);
            return parcel.Id;
        }
        /// <summary>
        ///  This function add element to the list of drinecarge.
        /// </summary>
        /// <param name="droneCarge"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void addDroneCarge(DroneCarge droneCarge)
        {
            List<DroneCarge> droneCarges = XMLTools.LoadListFromXMLSerializer<DroneCarge>(DroneChargeXml);

            droneCarges.Add(droneCarge);
            XMLTools.SaveListToXMLSerializer(droneCarges, DroneChargeXml);
        }

        /// <summary>
        /// This function assigns a package to the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool AssignPackageToDrone(int parcelId, int droneId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);

            Parcel parcelFind = parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = parcels.FindIndex(Parcel => Parcel.Id == parcelId);
            int droneFindIndex = drones.FindIndex(Drone => Drone.Id == droneId);

            if (parcelFindIndex != -1 && droneFindIndex != -1)
            {
                parcelFind.DroneId = droneId;
                parcelFind.Scheduled = DateTime.Now;
                parcels[parcelFindIndex] = parcelFind;
                XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
                return true;
            }
            else
                throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function performs an update on packet collection by drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool PackageCollectionByDrone(int parcelId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            Parcel parcelFind = parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = parcels.FindIndex(Parcel => Parcel.Id == parcelId);

            if (parcelFindIndex != -1)
            {
                parcelFind.PickedUp = DateTime.Now;
                parcels[parcelFindIndex] = parcelFind;
                XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
                return true;
            }
            else
                throw new IdNotExistExeptions("sorry, this Parcel is not found.");
        }
        /// <summary>
        /// This function performs an update on delivering a package to the customer.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeliveryPackageToCustomer(int parcelId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            Parcel parcelFind = parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = parcels.FindIndex(Parcel => Parcel.Id == parcelId);

            if (parcelFindIndex != -1)
            {
                parcelFind.Delivered = DateTime.Now;
                parcels[parcelFindIndex] = parcelFind;
                XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
                return true;
            }
            else
                throw new IdNotExistExeptions("sorry, this Parcel is not found.");
        }
        /// <summary>
        /// This function remove object from list when drone release from charging .
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool ReleaseDroneCarge(int droneId)
        {
            List<DroneCarge> droneCarges = XMLTools.LoadListFromXMLSerializer<DroneCarge>(DroneChargeXml);
            int indexDrone = droneCarges.FindIndex(DroneCarge => DroneCarge.DroneID == droneId);//
            if (indexDrone != -1)
            {
                droneCarges.RemoveAt(indexDrone);
                XMLTools.SaveListToXMLSerializer(droneCarges, DroneChargeXml);
                return true;
            }
            else
                throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function performs adding a charging slot to the station  
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool addingCargeSlotsToStation(int stationId)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            Station stationFind = stations.Find(Station => Station.Id == stationId);
            int stationFindIndex = stations.FindIndex(Station => Station.Id == stationId);

            if (stationFindIndex != -1)
            {
                stationFind.freeChargeSlots++;
                stations[stationFindIndex] = stationFind;
                XMLTools.SaveListToXMLSerializer(stations, StationXml);
                return true;
            }
            else
                throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        /// <summary>
        /// This function performs reduction  carge slot to the station. 
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool reductionCargeSlotsToStation(int stationId)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            Station stationFind = stations.Find(Station => Station.Id == stationId);
            int stationFindIndex = stations.FindIndex(Station => Station.Id == stationId);

            if (stationFindIndex != -1)
            {
                stationFind.freeChargeSlots--;
                stations[stationFindIndex] = stationFind;
                XMLTools.SaveListToXMLSerializer(stations, StationXml);
                return true;
            }
            else
                throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        /// <summary>
        /// This function transmits the data of the requested station according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station getStation(int Id)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            Station getStation = stations.Find(Station => Station.Id == Id);
            return getStation.Id != default ? getStation : throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        /// <summary>
        /// This function returns a charging entity by ID.
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCarge getDroneCargeByStationId(int stationId)
        {
            List<DroneCarge> droneCarges = XMLTools.LoadListFromXMLSerializer<DroneCarge>(DroneChargeXml);
            DroneCarge getDroneCarge = droneCarges.Find(DroneCarge => DroneCarge.StationId == stationId);
            return getDroneCarge.StationId != default ? getDroneCarge : throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        /// <summary>
        /// This function returns a charging entity by ID.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCarge getDroneCargeByDroneId(int droneId)
        {
            List<DroneCarge> droneCarges = XMLTools.LoadListFromXMLSerializer<DroneCarge>(DroneChargeXml);
            DroneCarge getDroneCarge = droneCarges.Find(DroneCarge => DroneCarge.DroneID == droneId);
            return getDroneCarge.StationId != default ? getDroneCarge : throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function transmits the requested drone data according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone getDrone(int Id)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            Drone getDrone = drones.Find(Drone => Drone.Id == Id);
            return getDrone.Id != default ? getDrone : throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function transmits the requested customer data according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer getCustomer(int Id)
        {
            XElement elements = XMLTools.LoadListFromXMLElement(CustomerXml);
            Customer customer = (from custom in elements.Elements()
                                 where custom.Element("Id").Value == Id.ToString()
                                 select new Customer()
                                 {
                                     Id = (int)Int32.Parse(custom.Element("Id").Value),
                                     name = (custom.Element("name").Value),
                                     phone = (custom.Element("phone").Value),
                                     lattitude = (double)double.Parse(custom.Element("lattitude").Value),
                                     longitude = (double)double.Parse(custom.Element("longitude").Value)
                                 }
                        ).FirstOrDefault();

            return customer.Id == Id ? customer : throw new IdNotExistExeptions("sorry, this customer is not found.");
        }
        /// <summary>
        /// This function transmits the requested parcel data according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel getParcel(int Id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            Parcel getParcel = parcels.Find(Parcel => Parcel.Id == Id);
            return getParcel.Id != default ? getParcel : throw new IdNotExistExeptions("sorry, this Parcel is not found.");
        }
        /// <summary>
        /// This function transmits data of all existing stations.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> DisplaysIistOfStations(Predicate<Station> p = null)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            return stations.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing drones.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> DisplaysTheListOfDrons(Predicate<Drone> p = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);

            return drones.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing customers.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> DisplaysIistOfCustomers(Predicate<Customer> p = null)
        {
            XElement elements = XMLTools.LoadListFromXMLElement(CustomerXml);
            return (from custom in elements.Elements()
                    select new Customer()
                    {
                        Id = (int)Int32.Parse(custom.Element("Id").Value),
                        name = (custom.Element("name").Value),
                        phone = (custom.Element("phone").Value),
                        lattitude = (double)double.Parse(custom.Element("lattitude").Value),
                        longitude = (double)double.Parse(custom.Element("longitude").Value)
                    }).Where(d => p == null || p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing parcels.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> DisplaysIistOfparcels(Predicate<Parcel> p = null)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            return parcels.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function returns information about the battery consumption in the drone.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] PowerConsumptionRate()
        {
            double[] powerConsumptionRate = new double[] {
                DataSource.Config.available, DataSource.Config.easy,
                DataSource.Config.medium, DataSource.Config.Heavy,
                DataSource.Config.ChargingRate
            };
            return powerConsumptionRate;
        }
        /// <summary>
        /// This function returns a login password.
        /// </summary>
        /// <returns></returns>
        public int PasswordDL() { return DataSource.Config.password; }
        /// <summary>
        /// This function deletes an object by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool removeDrone(int id)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);

            int find = drones.FindIndex(Drone => Drone.Id == id);
            if (find != -1)
            {
                drones.RemoveAt(find);
                XMLTools.SaveListToXMLSerializer(drones, DroneXml);
                return true;
            }
            throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function deletes an object by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool removeStation(int id)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            int find = stations.FindIndex(Station => Station.Id == id);
            if (find != -1)
            {
                stations.RemoveAt(find);
                XMLTools.SaveListToXMLSerializer(stations, StationXml);
                return true;
            }
            throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        /// <summary>
        /// This function deletes an object by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool removeCustomer(int id)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);

            int find = customers.FindIndex(Customer => Customer.Id == id);
            if (find != -1)
            {
                customers.RemoveAt(find);
                XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
                return true;
            }
            throw new IdNotExistExeptions("sorry, this customer is not found.");
        }
        /// <summary>
        ///This function deletes an object by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool removeParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            int find = parcels.FindIndex(Parcel => Parcel.Id == id);
            if (find != -1)
            {
                parcels.RemoveAt(find);
                XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
                return true;
            }
            throw new IdNotExistExeptions("sorry, this Parcel is not found.");
        }
    }
}
