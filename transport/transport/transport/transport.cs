using System.Collections.Generic;
using System.Linq;

namespace transport
{
    class CityTransport
    {
        public Map RoadMap;
        public Map Railways;
        public Map Subway;

        public CityTransport(List<string> stops)
        {
            RoadMap = new Map(stops);
            Railways = new Map(stops);
            Subway = new Map(stops);
        }
    }

    class Map
    {
        private List<string> Stops;
        private int[,] Roads;

        public Map(List<string> stops)
        {
            Stops = stops;
            int len = Stops.Count;
            Roads = new int[len, len];
            for(int i=0;i<len;i++)
            {
                for(int j=0;j<len;j++)
                {
                    Roads[i, j] = 0;
                }
            }
        }

        public void CreateRoad(string from, string to, int type = 1)
        {
            //type<=0 => road isn't exist
            //type==1 => common road
            //type==2 => trolleybus road (only for CityTransport.RoadMap)
            int indexFrom = Stops.IndexOf(from);
            int indexTo = Stops.IndexOf(to);
            Roads[indexFrom, indexTo] = type;
            Roads[indexTo, indexFrom] = type;
        }

        public bool Exists(List<string> route, int type = 1)
        {
            if (route.Count < 2)
                return false;
            if (route.Count == 2)
            {
                int indexFrom = Stops.IndexOf(route[0]);
                int indexTo = Stops.IndexOf(route[1]);
                if (indexFrom == -1 || indexTo == -1)
                    return false;
                if (Roads[indexFrom, indexTo] >= type)
                    return true;
                else
                    return false;
            }
            for (int i = 0; i < route.Count - 1; i++)
            {
                if (!Exists(new List<string> { route[i], route[i + 1] }, type))
                    return false;
            }
            return true;
        }
    }

    interface ITransport
    {
        bool SetRoute(List<string> route);
        List<string> GetRoute();
        string Info();
    }

    abstract class Transport:ITransport
    {
        public readonly string Name;
        protected List<string> Route = null;
        protected Map map = null;

        public Transport(string name)
        {
            Name = name;
        }

        virtual public bool SetRoute(List<string> route)
        {
            if (map.Exists(route))
            {
                Route = new List<string>(route);
                return true;
            }
            return false;
        }

        public List<string> GetRoute()
        {
            if (Route == null)
                return null;
            return new List<string>(Route);
        }

        abstract public string Info();
    }

    class Bus : Transport
    {
        public Bus(string name, CityTransport maps) : base(name)
        {
            map = maps.RoadMap;
        }

        public override string Info()
        {
            return "Bus " + Name;
        }
    }

    class Trolleybus : Transport
    {
        public Trolleybus(string name, CityTransport maps) : base(name)
        {
            map = maps.RoadMap;
        }

        public override bool SetRoute(List<string> route)
        {
            if (map.Exists(route, 2))
            {
                Route = new List<string>(route);
                return true;
            }
            return false;
        }

        public override string Info()
        {
            return "Trolleybus " + Name;
        }
    }

    class Tram : Transport
    {
        public Tram(string name, CityTransport maps) : base(name)
        {
            map = maps.Railways;
        }

        public override string Info()
        {
            return "Tram " + Name;
        }
    }

    class SubwayTrain : Transport
    {
        public SubwayTrain(string name, CityTransport maps) : base(name)
        {
            map = maps.Subway;
        }

        public override string Info()
        {
            return "SubwayTrain " + Name;
        }
    }
}