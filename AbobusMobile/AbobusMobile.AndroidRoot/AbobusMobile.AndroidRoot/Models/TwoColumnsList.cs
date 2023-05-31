using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.AndroidRoot.Models
{
    public class TwoColumnsList<T> : List<TwoColumnsElement<T>>
    {
        public static TwoColumnsList<T> FromList(List<T> sourceList)
        {
            TwoColumnsList<T> result = new TwoColumnsList<T>();

            TwoColumnsElement<T> currentElement = null;

            for (int i = 0; i < sourceList.Count; i++)
            {
                if (i % 2 == 0)
                {
                    currentElement = new TwoColumnsElement<T>();
                    currentElement.FirstExist = true;
                    currentElement.First = sourceList[i];
                }
                else
                {
                    currentElement.SecondExist = true;
                    currentElement.Second = sourceList[i];

                    result.Add(currentElement);
                    currentElement = null;
                }
            }

            if (currentElement != null)
            {
                result.Add(currentElement);
            }

            return result;
        }
    }
}
