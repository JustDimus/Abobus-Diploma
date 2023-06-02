using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace AbobusMobile.AndroidRoot.Services
{
    public class MapPinModel
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public MapCoordinateModel Coordinate { get; set; }

        public PinType PinType { get; set; }
    }
}
