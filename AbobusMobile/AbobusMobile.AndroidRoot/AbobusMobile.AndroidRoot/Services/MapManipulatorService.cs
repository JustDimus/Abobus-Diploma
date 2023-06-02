using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Maps;

namespace AbobusMobile.AndroidRoot.Services
{
    public class MapManipulatorService
    {
        private readonly Map _map;

        private readonly Dictionary<int, Pin> _pins = new Dictionary<int, Pin>();
        private Polyline currentPolyline;

        public MapManipulatorService(
            Map targetMap)
        {
            _map = targetMap ?? throw new ArgumentNullException(nameof(targetMap));
        }

        public void ClearAll()
        {
            _map.Pins.Clear();
            _map.MapElements.Clear();
        }

        public void RemovePin(int pinId)
        {
            var pinExist = _pins.ContainsKey(pinId);

            if (pinExist)
            {
                var pin = _pins[pinId];

                _map.Pins.Remove(pin);

                _pins.Remove(pinId);
            }
        }

        public void AddPin(MapPinModel pinModel)
        {
            var pin = new Pin()
            {
                Position = new Position(pinModel.Coordinate.Latitude, pinModel.Coordinate.Longitude),
                Label = pinModel.Label,
                Type = pinModel.PinType,
            };

            _pins.Add(pinModel.Id, pin);

            _map.Pins.Add(pin);
        }

        public void UpdatePolyline(MapPolylineModel polylineModel)
        {
            if (currentPolyline != null
                && _map.MapElements.Contains(currentPolyline))
            {
                _map.MapElements.Remove(currentPolyline);
            }

            currentPolyline = new Polyline();
            
            foreach (var coordinate in polylineModel.Coordinates)
            {
                currentPolyline.Geopath.Add(new Position(coordinate.Latitude, coordinate.Longitude));
            }

            _map.MapElements.Add(currentPolyline);
        }

        public void RemovePolyline()
        {
            if (currentPolyline != null
                && _map.MapElements.Contains(currentPolyline))
            {
                _map.MapElements.Remove(currentPolyline);
                currentPolyline = null;
            }
        }
    }
}
