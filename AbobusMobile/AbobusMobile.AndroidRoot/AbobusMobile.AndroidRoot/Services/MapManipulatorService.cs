using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace AbobusMobile.AndroidRoot.Services
{
    public class MapManipulatorService
    {
        private readonly Map _map;

        public MapManipulatorService(
            Map targetMap)
        {
            _map = targetMap ?? throw new ArgumentNullException(nameof(targetMap));
        }

        public void AddPin(double longitude, double latitude)
        {
            _map.Pins.Add(new Pin()
            {
                Position = new Position(latitude, longitude),
                Label = "Home"
            });
        }
    }
}
