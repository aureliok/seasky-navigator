using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaSkyNavigator.Records
{
    public record LocationData
    (
        double latitude,
        double longitude,
        string type,
        string name,
        object number,
        object postal_code,
        object street,
        double confidence,
        string region,
        string region_code,
        string county,
        string locality,
        string administrative_area,
        object neighbourhood,
        string country,
        string country_code,
        string continent,
        string label
    );

    public record Location
    (
        IReadOnlyList<LocationData> data
    );

}
