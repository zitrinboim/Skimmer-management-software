using Dal;
using DalApi;
using DO;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
namespace DalXml
{
    public class DalXml : IDal
    {
        private static string DroneXml = @"DroneXml.Xml";
        private static string StationXml = @"StationXml.Xml";
        private static string CustomerXml = @"CustomerXml.Xml";
        private static string ParcelXml = @"ParcelXml.Xml";
        private static string DroneChargeXml = @" DroneChargeXml.Xml";

        static internal DalXml instatnce = new DalXml();

        private DalXml() => DataSource.Initialize();

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
        public bool addCustomer(Customer customer)
        {
            XElement elements = XMLTools.LoadListFromXMLElement(CustomerXml);
            XElement customerElement = (from element in elements.Elements()
                                        where element.Element("Id").Value == customer.Id.ToString()
                                        select element).FirstOrDefault() != default ?
                                        throw new IdExistExeptions("Sorry, i have already a customer with this id:" + customer.Id) :
                                        new XElement("Parcel", new XElement
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
        public int addParsel(Parcel parcel)
        {
            parcel.Id = DataSource.Config.ParcelIdRun;

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

            XMLTools.SaveListToXMLElement(elements, ParcelXml);


            return (DataSource.Config.ParcelIdRun++);
        }
        /// <summary>
        ///  This function add element to the list of drinecarge.
        /// </summary>
        /// <param name="droneCarge"></param>
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
        public bool AssignPackageToDrone(int parcelId, int droneId)
        {
            Parcel parcelFind = DataSource.parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = DataSource.parcels.FindIndex(Parcel => Parcel.Id == parcelId);
            int droneFindIndex = DataSource.drones.FindIndex(Drone => Drone.Id == droneId);

            if (parcelFindIndex != -1 && droneFindIndex != -1)
            {
                parcelFind.DroneId = droneId;
                parcelFind.Scheduled = DateTime.Now;
                DataSource.parcels[parcelFindIndex] = parcelFind;
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
        public bool PackageCollectionByDrone(int parcelId)
        {
            Parcel parcelFind = DataSource.parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = DataSource.parcels.FindIndex(Parcel => Parcel.Id == parcelId);

            if (parcelFindIndex != -1)
            {
                parcelFind.PickedUp = DateTime.Now;
                DataSource.parcels[parcelFindIndex] = parcelFind;
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
        public bool DeliveryPackageToCustomer(int parcelId)
        {
            Parcel parcelFind = DataSource.parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = DataSource.parcels.FindIndex(Parcel => Parcel.Id == parcelId);

            if (parcelFindIndex != -1)
            {
                parcelFind.Delivered = DateTime.Now;
                DataSource.parcels[parcelFindIndex] = parcelFind;
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
        public bool ReleaseDroneCarge(int droneId)
        {
            int indexDrone = DataSource.droneCarges.FindIndex(DroneCarge => DroneCarge.DroneID == droneId);//
            if (indexDrone != -1)
            {
                DataSource.droneCarges.RemoveAt(indexDrone);
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
        public bool addingCargeSlotsToStation(int stationId)
        {
            Station stationFind = DataSource.stations.Find(Station => Station.Id == stationId);
            int stationFindIndex = DataSource.stations.FindIndex(Station => Station.Id == stationId);

            if (stationFindIndex != -1)
            {
                stationFind.freeChargeSlots++;
                DataSource.stations[stationFindIndex] = stationFind;
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
        public bool reductionCargeSlotsToStation(int stationId)
        {
            Station stationFind = DataSource.stations.Find(Station => Station.Id == stationId);
            int stationFindIndex = DataSource.stations.FindIndex(Station => Station.Id == stationId);

            if (stationFindIndex != -1)
            {
                stationFind.freeChargeSlots--;
                DataSource.stations[stationFindIndex] = stationFind;
                return true;
            }
            else
                throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function transmits the data of the requested station according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Station getStation(int Id)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            Station getStation = stations.Find(Station => Station.Id == Id);
            return getStation.Id != default ? getStation : throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        public DroneCarge getDroneCargeByStationId(int stationId)
        {
            List<DroneCarge> droneCarges = XMLTools.LoadListFromXMLSerializer<DroneCarge>(DroneChargeXml);
            DroneCarge getDroneCarge = droneCarges.Find(DroneCarge => DroneCarge.StationId == stationId);
            return getDroneCarge.StationId != default ? getDroneCarge : throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
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
        public Customer getCustomer(int Id)
        {
            XElement elements = XMLTools.LoadListFromXMLElement(CustomerXml);
            Customer customer  = (from custom in elements.Elements()
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
        public IEnumerable<Station> DisplaysIistOfStations(Predicate<Station> p = null)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            return stations.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing drones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> DisplaysTheListOfDrons(Predicate<Drone> p = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);

            return drones.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing customers.
        /// </summary>
        /// <returns></returns>
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
                    }
                        ).Where(d => p == null || p(d)).ToList();      
        }
        /// <summary>
        /// This function transmits data of all existing parcels.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> DisplaysIistOfparcels(Predicate<Parcel> p = null)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            return parcels.Where(d => p == null ? true : p(d)).ToList();
        }
        public double[] PowerConsumptionRate()////////////////////////////////////////////////////האם זה צריך להיות פה או לעבור לקונפיג אקסאםעל
        {
            double[] powerConsumptionRate = new double[] {
                DataSource.Config.available, DataSource.Config.easy,
                DataSource.Config.medium, DataSource.Config.Heavy,
                DataSource.Config.ChargingRate
            };
            return powerConsumptionRate;
        }
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
        public bool removeCustomer(int id)
        {
            XElement elements = XMLTools.LoadListFromXMLElement(CustomerXml);
           XElement xElement= elements.Elements().Where(elements => elements.Element("id").Value == id.ToString()).FirstOrDefault();
            if (xElement != default)
            {
                elements.Elements().Where(elements => elements.Element("id").Value == id.ToString()).Remove();
                XMLTools.SaveListToXMLElement(elements, CustomerXml);
                return true;
            }
            else
            {
            throw new IdNotExistExeptions("sorry, this customer is not found.");
            }
        }
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

/*


namespace DalXml
{
    class DalXml : IDal
    {
       
        

        static DalXml()
        {
            //DataSource.Initialize();
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> p = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return drones.Where(d => p == null || p(d)).ToList();
        }

        public IEnumerable<Customer> GetCustomers(Predicate<Customer> p = null)
        {
            List<Customer> cutomers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            return cutomers.Where(d => p == null || p(d)).ToList();
        }

        public IEnumerable<Station> GetStations(Predicate<Station> p = null)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            return stations.Where(d => p == null || p(d)).ToList();
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> p = null)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return parcels.Where(d => p == null || p(d)).ToList();
        }

        public IEnumerable<DroneCharge> GetDroneCharges(Predicate<DroneCharge> p = null)
        {
            XElement element = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            return (from ch in element.Elements()
                    select new DroneCharge()
                    {
                        DroneId = (int?)Int32.Parse(ch.Element("DroneId").Value),
                        StationId = (int?)Int32.Parse(ch.Element("StationId").Value),
                        StartCharge = DateTime.Parse(ch.Element("StartCharge").Value)
                    }
                        ).Where(d => p == null || p(d)).ToList();
        }

        public void AddDrone(Drone newDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);

            if (drones.Exists(d => d.Id == newDrone.Id))
                throw new InsertingAnExistingItemException($"ID drone {newDrone.Id}: alraedy exist");
            drones.Add(newDrone);

            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);

            if (customers.Exists(d => d.Id == newCustomer.Id))
                throw new InsertingAnExistingItemException($"ID drone {newCustomer.Id}: alraedy exist");
            customers.Add(newCustomer);

            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }

        public void AddStation(Station newStation)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            if (stations.Exists(s => s.Id == newStation.Id) == true)
                throw new InsertingAnExistingItemException($"station ID {newStation.Id}: alraedy exist");

            stations.Add(newStation);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public void AddParcel(Parcel newParcel)///////////
        {
            throw new NotImplementedException();
        }

        public void AddDroneCharge(DroneCharge newDroneCharge)
        {

            XElement elements = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            XElement droneCharge = (from element in elements.Elements()
                                    where element.Element("StationId").Value == newDroneCharge.StationId.ToString() &&
                                    element.Element("DroneId").Value == newDroneCharge.DroneId.ToString()
                                    select element).FirstOrDefault() != default ?
                                    throw new InsertingAnExistingItemException($"charger with drone ID {newDroneCharge.DroneId}: alraedy exist in charge list") :
                                    new XElement("DroneCharge", new XElement
                                    ("DroneId", newDroneCharge.DroneId)
                                    , new XElement("StationId", newDroneCharge.StationId)
                                    , new XElement("StartCharge", DateTime.Now));


            elements.Add(droneCharge);

            XMLTools.SaveListToXMLElement(elements, DroneChargeXml);
        }

        public Drone GetDrone(int? ID)
        {
            if (ID == null)
                throw new NullReferenceException("ID  drone == null");

            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int index = drones.FindIndex(d => d.Id == ID);

            if (index == -1)
                throw new NotExistingItemException($"drone ID {ID}: don't exist");

            return drones[index];
        }

        public Station GetStation(int? ID)
        {
            if (ID == null)
                throw new NullReferenceException("ID station == null");

            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            int index = stations.FindIndex(s => s.Id == ID);

            if (index == -1)
                throw new NotExistingItemException($"station ID {ID}: don't exist");

            return stations[index];
        }

        public Parcel GetParcel(int? ID)
        {
            if (ID == null)
                throw new NullReferenceException("ID parcel == null");

            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int index = parcels.FindIndex(p => p.Id == ID);

            if (index == -1)
                throw new NotExistingItemException($"parcel ID {ID}: don't exist");

            return parcels[index];
        }

        public Customer GetCustomer(int? ID)
        {
            if (ID == null)
                throw new NullReferenceException("ID customer == null");

            List<Customer> cutomers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            int index = cutomers.FindIndex(c => c.Id == ID);

            if (index == -1)
                throw new NotExistingItemException($"customer ID {ID}: don't exist");

            return cutomers[index];
        }

        public DroneCharge GetDroneCharge(int? droneID)
        {

            XElement element = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            DroneCharge charge = (from ch in element.Elements()
                                  where ch.Element("DroneId").Value == droneID.ToString()
                                  select new DroneCharge()
                                  {
                                      DroneId = (int?)Int32.Parse(ch.Element("DroneId").Value),
                                      StationId = (int?)Int32.Parse(ch.Element("StationId").Value),
                                      StartCharge = DateTime.Parse(ch.Element("StartCharge").Value)
                                  }
                        ).FirstOrDefault();

            return charge.DroneId == droneID ? charge : throw new NotExistingItemException($"Charge with drone ID {droneID}: don't exist");
        }

        public void UpdateDrone(Drone UDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);

            int index = drones.FindIndex(d => d.Id == UDrone.Id);

            if (index == -1)
                throw new NotExistingItemException($"drone ID {UDrone.Id}: don't exist");

            drones[index] = UDrone;
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void UpdateParcel(Parcel UParcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            int index = parcels.FindIndex(p => p.Id == UParcel.Id);
            if (index == -1)
                throw new NotExistingItemException($"parcel ID {UParcel.Id}: don't exist");

            parcels[index] = UParcel;

            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void UpdateStation(Station UStation)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);

            int index = stations.FindIndex(s => s.Id == UStation.Id);
            if (index == -1)
                throw new NotExistingItemException($"station ID {UStation.Id}: don't exist");

            stations[index] = UStation;

            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public void UpdateCustomer(Customer UCustomer)
        {
            List<Customer> cutomers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);

            int index = cutomers.FindIndex(c => c.Id == UCustomer.Id);
            if (index == -1)
                throw new NotExistingItemException($"customer ID {UCustomer.Id}: don't exist");

            cutomers[index] = UCustomer;

            XMLTools.SaveListToXMLSerializer(cutomers, CustomerXml);
        }

        public void DeleteDroneCharge(int? droneId)//////////////////////////////
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            int index = droneCharges.FindIndex(c => c.DroneId == droneId);

            if (index == -1)
                throw new NotExistingItemException($"charger with drone ID {droneId }: don't exist");

            droneCharges.RemoveAt(index);
            XMLTools.SaveListToXMLSerializer(droneCharges, DroneChargeXml);
        }

        public void DeleteDrone(int droneId)/////
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int index = drones.FindIndex(d => d.Id == droneId);

            if (index == -1)
                throw new NotExistingItemException($"drone ID {droneId }: don't exist");

            drones.RemoveAt(index);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void DeleteParcel(int parcelId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int index = parcels.FindIndex(p => p.Id == parcelId);

            if (index == -1)
                throw new NotExistingItemException($"parcel ID {parcelId }: don't exist");

            parcels.RemoveAt(index);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void DeleteCustomer(int customerId)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            int index = customers.FindIndex(c => c.Id == customerId);

            if (index == -1)
                throw new NotExistingItemException($"customer ID {customerId }: don't exist");

            customers.RemoveAt(index);

            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }

        public void DeleteStation(int stationId)////////
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>( StationXml);
            int index =  stations.FindIndex(s => s.Id == stationId);

            if (index == -1)
                throw new NotExistingItemException($"station ID {stationId }: don't exist");

            stations.RemoveAt(index);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public double[] GetElectricity()
        {
            return new double[] {1,2,3,4,100000 };
        }//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
 
 */
